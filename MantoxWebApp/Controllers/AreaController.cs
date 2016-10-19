using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MantoxWebApp.Models;
using System.Collections;
using FileHelper;
using System.Reflection;

namespace MantoxWebApp.Controllers
{
    public class AreaController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Areas";
        public string NombreObjeto = "Area";

        /// <summary>
        /// Lista de áreas de un edificio, piso, sede o empresa en particular
        /// </summary>
        IEnumerable areas; //Almacenará la lista de áreas

        /// <summary>
        /// Lista de estados de objetos. Los estados disponibles varían según el tipo de objeto al cual están asociados. Ejemplos: Activo, inactivo, pendiente, suspendido, etc.
        /// </summary>
        IEnumerable estados; //Almacenará la lista de estados

        /// <summary>
        /// Lista de empresas registradas en Mantox
        /// </summary>
        IEnumerable empresas; //Almacenará la lista de empresas

        /// <summary>
        /// Index modificado,redirige a Ver()
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Ver");
        }

        /// <summary>
        /// Obtiene una vista con la lista de elementos registrados y sus detalles
        /// </summary>
        /// <returns>Vista Areas</returns>
        public async Task<ActionResult> Ver()
        {
            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase", this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Areas.ToListAsync()), RolDeUsuario.Reportes);

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Devuelve un PartialView que contiene una lista JSon de las áreas filtradas por concepto de búsqueda, o por rango
        /// </summary>
        /// <param name="searchString">Términos de búsqueda</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>PartialView</returns>
        public PartialViewResult BuscarAreas(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Reportes)) { return PartialView("Error401"); }

            try
            {
                //Creamos nueva instancia de la clase parcial "v_areas"
                V_Areas miVistaAreas = new V_Areas();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaAreas.BuscarAreas(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosAreas = new DataTable();

                //Asignamos el valor tablaResultadosAreas tomando el valor del dicccionario
                tablaResultadosAreas = (DataTable)diccionarioResultados["TablaResultados"];

                //Creamos enteros para almacenar los diferentes valores requeridos por el paginador
                int totalFilas = 0;
                int filasPorPagina = 0;
                int paginaActual = 0;
                int totalPaginas = 0;

                //Asignamos el valor a las variables tomando los valores del diccionario de resultados
                totalFilas = (int)diccionarioResultados["TotalFilas"];
                filasPorPagina = (int)diccionarioResultados["FilasPorPagina"];
                paginaActual = (int)diccionarioResultados["PaginaActual"];
                totalPaginas = (int)diccionarioResultados["TotalPaginas"];

                //Adjuntamos estos datos a la vista
                ViewBag.TablaResultadosAreas = tablaResultadosAreas;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarAreas"), RolDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }

        /// <summary>
        /// Devuelve un formulario de edición del área especificado por medio del Id
        /// </summary>
        /// <param name="id">Id del area</param>
        /// <returns></returns>
        public ActionResult Editar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Plantilla = "formTemplate";
                ViewBag.Accion = "Editar";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                //Objeto de tipo V_Areas con los datos del area que se está editando
                ViewData.Add("AreaActual", bdMantox.V_Areas.FirstOrDefault(u => u.Id == id));

                //Se devuelve el formulario de creación de area con un objeto de tipo V_Areas con los datos del area que se está editando
                return VistaAutenticada(View("Crear", (V_Areas)bdMantox.Areas.Find(id)), RolDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Muestra un formulario para crear un area nueva
        /// </summary>
        /// <returns></returns>
        public ActionResult Crear()
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Plantilla = "formTemplate";

                ViewBag.Accion = "Crear";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                return VistaAutenticada(View("Crear", new V_Areas()), RolDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Recibe los datos del formulario de creación de areas, los valida y los inserta a la base de datos.
        /// Este método también es usado para actualizar un area existente.
        /// </summary>
        /// <param name="areavm">CrearEditarAreaViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear([Bind(Include = "Id,Nombre,Piso,Id_Empresa,Id_Sede,Id_Edificio,Id_Estado")] CrearEditarAreaViewModel areavm)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            try
            {
                //Creamos una instancia de area con los datos que recibimos del formulario
                Area areaRecibida = new Area();
                areaRecibida = (Area)areavm;

                //Si el area no es nueva (el id > 0) entonces validamos primero si ya existe un área
                //con un id diferente pero con los mismos datos, esto identificaria un duplicado.
                if (areaRecibida.Id > 0)
                {
                    //Buscamos un duplicado de la siguiente manera
                    if (bdMantox.V_Areas.FirstOrDefault(
                        va =>
                            va.Nombre.Trim().ToLower() == areaRecibida.Nombre.Trim().ToLower() &&
                            (int)va.Id_Edificio == (int)areaRecibida.Id_Edificio &&
                            (int)va.Piso == (int)areaRecibida.Piso &&
                            (int)va.Id != (int)areaRecibida.Id
                        ) != null)
                    {
                        //Si existe, se añade error al modelo.
                        ModelState.AddModelError("Nombre", "El área ingresada ya existe en el edificio y piso seleccionados.");
                    }
                }
                else
                {
                    //Si el area es nueva (id = 0), validamos que el no exista una con los mismos datos.
                    if (bdMantox.V_Areas.FirstOrDefault(
                        va =>
                            va.Nombre.Trim().ToString() == areaRecibida.Nombre.Trim().ToString() &&
                            (int)va.Id_Edificio == (int)areaRecibida.Id_Edificio &&
                            (int)va.Piso == (int)areaRecibida.Piso
                        ) != null)
                    {
                        //Si existe, se añade error al modelo
                        ModelState.AddModelError("Nombre", "El área ingresada ya existe.");
                    }
                }

                //Validamos que no haya errores en el modelo
                if (ModelState.IsValid)
                {
                    //Si es area nueva...
                    if (areavm.Id <= 0)
                    {
                        //La añadirmos a la base de datos
                        bdMantox.Areas.Add(areaRecibida);
                    }
                    else //Si es area existente (id > 0)
                    {
                        //Lo ponemos en estado modificado
                        bdMantox.Entry(areaRecibida).State = EntityState.Modified;
                    }

                    //Enviamos los cambios a la base de datos
                    await bdMantox.SaveChangesAsync();

                    //Redirigimos a la página de creación de area.
                    return RedirectToAction("Crear");
                }

                //Si el modelo tiene errores de validación, se crea nuevamente el
                //formulario y se muestra con los errores

                llenarListasDesplegables();

                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Accion = "Editar";
                ViewBag.Plantilla = "formTemplate";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("AreaActual", (V_Areas)areavm);

                return VistaAutenticada(View((V_Areas)areaRecibida), RolDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }


        /// <summary>
        /// Elimina el area especificado por medio de la id
        /// </summary>
        /// <param name="id">Id del area que se va a eliminar</param>
        /// <returns></returns>
        public async Task<ActionResult> Eliminar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            try
            {
                //Se valida si el id es nulo
                if (id == null)
                {
                    //Si es nulo se envía Error "Bad Request"
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Si no es nulo, se busca el id en la base de datos
                Area area = await bdMantox.Areas.FindAsync(id);

                //Se valida si se encontró o no
                if (area == null)
                {
                    //Si no se encuentra, se devuelve error 404
                    return HttpNotFound();
                }

                //Finalmente, si se encuentra el area, se elimina
                bdMantox.Areas.Remove(area);
                //Se guardan los cambios
                await bdMantox.SaveChangesAsync();
                //Se devuelve error 200 (ok)
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("ErrorInterno", "Error");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bdMantox.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Este método se encarga de llenar las listas desplegables
        /// </summary>
        private void llenarListasDesplegables()
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return; }
            try
            {

                //Llenar lista de Areas
                areas = bdMantox.Areas.Select(area => new
                {
                    AreaId = area.Id,
                    AreaNombre = area.Nombre,
                    EstadoId = area.Id_Estado
                })
                .Where //Se añade condición para mostrar sólo áreas activas
                    (a => (int)a.EstadoId == (int)EstadoMantox.Activo)
                .ToList();

                //Llenar lista de Estados
                estados = bdMantox.Estados.Select(estado => new
                {
                    EstadoId = estado.Id,
                    EstadoNombre = estado.Nombre,
                    EstadoTipo = estado.Tipo
                }).Where(e => e.EstadoTipo == "General").ToList();


                //El filtrado por empresa NO debe estar activado para usuarios no desarrolladores:
                switch ((RolDeUsuario)System.Web.HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                        //No se añaden restricciones a las listas que puede ver el desarrollador

                        //Llenar lista de Empresas
                        empresas = bdMantox.Empresas.Select(empresa => new
                        {
                            EmpresaId = empresa.Id,
                            EmpresaNombre = empresa.Nombre,
                            EstadoId = empresa.Id_Estado
                        })
                        .Where //Se añade condición para mostrar sólo empreas activas
                            (e => (int)e.EstadoId == (int)EstadoMantox.Activo)
                        .ToList();
                        break;
                    case RolDeUsuario.Administrador:
                    default:
                        //Almacenar id del rol del usuario actual en variable int
                        int idRolDeUsuarioActual = (int)Session["Id_Rol"];

                        //Almacenar id de la empresa del usuario actual en variable int
                        int idEmpresaDeUsuarioActual = (int)Session["Id_Empresa"];

                        //Llenar lista de Empresas
                        empresas = bdMantox.Empresas.Select(empresa => new
                        {
                            EmpresaId = empresa.Id,
                            EmpresaNombre = empresa.Nombre,
                            EstadoId = empresa.Id_Estado
                        })
                        .Where //Se añade condición para elegir solo empresas activas
                            (e => (int)e.EstadoId == (int)EstadoMantox.Activo)
                        .Where //Se añade condición de manera que solo pueda ver la empresa propia
                            (e => (int)e.EmpresaId == idEmpresaDeUsuarioActual || (int)e.EmpresaId == 1)
                        .ToList();
                        break;
                }
            }

            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
            }

        }

        /// <summary>
        /// Devuelve un PartialView que contiene un MultiSelect de las Áreas filtradas por un id de edificio o un piso de edificio
        /// </summary>
        /// <param name="idEdificio">Id del edificio</param>
        /// <param name="numPiso">Numero del piso</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarAreas(string idEdificio = "1",  string numPiso = "0")
        {
            var id_edificio = int.Parse(idEdificio);
            var num_piso = int.Parse(numPiso);

            List<Area> areas = (from a in bdMantox.Areas
                               where a.Id_Edificio  == id_edificio
                               where a.Piso == num_piso
                               select a).ToList();

            ViewBag.Areas = new MultiSelectList(areas, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarAreas");

        }
    }
}
