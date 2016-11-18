using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MantoxWebApp.Models;
using System.Collections;
using FileHelper;
using System.Reflection;

namespace MantoxWebApp.Controllers
{
    public class EdificioController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Edificios";
        public string NombreObjeto = "Edificio";
        
        /// <summary>
        /// Lista de estados de objetos. Los estados disponibles varían según el tipo de objeto al cual están asociados. Ejemplos: Activo, inactivo, pendiente, suspendido, etc.
        /// </summary>
        IEnumerable estados; //Almacenará la lista de estados

        /// <summary>
        /// Lista de sedes. Las sedes disponibles de uso asociadas por las empresas 
        /// </summary>
        IEnumerable sedes; //Almacenará la lista de sedes

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
        /// <returns>Vista Edificios</returns>
        public async Task<ActionResult> Ver()
        {
            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase", this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Edificios.ToListAsync()), RolDeUsuario.Desarrollador);

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
        public PartialViewResult BuscarEdificios(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            try
            {
                //Creamos nueva instancia de la clase parcial "v_edificios"
                V_Edificios miVistaEdificios = new V_Edificios();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaEdificios.BuscarEdificios(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosEdificios = new DataTable();

                //Asignamos el valor tablaResultadosEdificios tomando el valor del dicccionario
                tablaResultadosEdificios = (DataTable)diccionarioResultados["TablaResultados"];

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
                ViewBag.TablaResultadosEdificios = tablaResultadosEdificios;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarEdificios"), RolDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }


        //// GET: Edificio
        //public async Task<ActionResult> Index()
        //{
        //    return VistaAutenticada(View(await bdMantox.Edificios.ToListAsync()), RolDeUsuario.Reportes);
        //}

        /// <summary>
        /// Devuelve un formulario de edición del área especificado por medio del Id
        /// </summary>
        /// <param name="id">Id del edificio</param>
        /// <returns></returns>
        public ActionResult Editar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Sedes = new MultiSelectList(sedes, "SedeId", "SedeNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Plantilla = "formTemplate";
                ViewBag.Accion = "Editar";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                //Objeto de tipo V_Edificios con los datos del edificio que se está editando
                ViewData.Add("EdificioActual", bdMantox.V_Edificios.FirstOrDefault(u => u.Id == id));

                //Se devuelve el formulario de creación del edificio con un objeto de tipo V_Edificios con los datos del edificio que se está editando
                return VistaAutenticada(View("Crear", (V_Edificios)bdMantox.Edificios.Find(id)), RolDeUsuario.Desarrollador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }


        /// <summary>
        /// Muestra un formulario para crear un edificio nuevo
        /// </summary>
        /// <returns></returns>
        public ActionResult Crear()
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Edificios = new MultiSelectList(sedes, "SedeId", "SedeNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Plantilla = "formTemplate";

                ViewBag.Accion = "Crear";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                return VistaAutenticada(View("Crear", new V_Edificios()), RolDeUsuario.Desarrollador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }


        // POST: Edificio/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear([Bind(Include = "Id,Nombre,Id_Sede,Id_Estado")]  CrearEditarEdificioViewModel edificiovm)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            try
            {
                //Creamos una instancia del edificio con los datos que recibimos del formulario
                Edificio edificioRecibido = new Edificio();
                edificioRecibido = (Edificio)edificiovm;

                //Si el edificio no es nuevo (el id > 0) entonces validamos primero si ya existe un edificio
                //con un id diferente pero con los mismos datos, esto identificaria un duplicado.
                if (edificioRecibido.Id > 0)
                {
                    //Buscamos un duplicado de la siguiente manera
                    if (bdMantox.V_Edificios.FirstOrDefault(
                        ve =>
                            ve.Nombre.Trim().ToLower() == edificiovm.Nombre.Trim().ToLower() &&
                            (int)ve.Id_Empresa == (int)edificiovm.Id_Empresa &&
                            (int)ve.Id != (int)edificioRecibido.Id
                        ) != null)
                    {
                        //Si existe, se añade error al modelo.
                        ModelState.AddModelError("Nombre", "El área ingresada ya existe en el edificio y piso seleccionados.");
                    }
                }
                else
                {
                    //Si el edificio es nuevo (id = 0), validamos que el no exista una con los mismos datos.
                    if (bdMantox.V_Edificios.FirstOrDefault(
                        ve =>
                            ve.Nombre.Trim().ToLower() == edificiovm.Nombre.Trim().ToLower()
                        ) != null)
                    {
                        //Si existe, se añade error al modelo
                        ModelState.AddModelError("Nombre", "El edificio ingresado ya existe.");
                    }
                }

                //Validamos que no haya errores en el modelo
                if (ModelState.IsValid)
                {
                    //Si el edificio es nuevo...
                    if (edificiovm.Id <= 0)
                    {
                        //La añadirmos a la base de datos
                        bdMantox.Edificios.Add(edificioRecibido);
                    }
                    else //Si es un edificio existente (id > 0)
                    {
                        //Lo ponemos en estado modificado
                        bdMantox.Entry(edificioRecibido).State = EntityState.Modified;
                    }

                    //Enviamos los cambios a la base de datos
                    await bdMantox.SaveChangesAsync();

                    //Redirigimos a la página de creación de edificios.
                    return RedirectToAction("Crear");
                }

                //Si el modelo tiene errores de validación, se crea nuevamente el
                //formulario y se muestra con los errores

                llenarListasDesplegables();

                ViewBag.Sedes = new MultiSelectList(sedes, "SedeId", "SedeNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Accion = "Editar";
                ViewBag.Plantilla = "formTemplate";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("EdificioActual", (V_Edificios)edificiovm);

                return VistaAutenticada(View((V_Edificios)edificioRecibido), RolDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

      

        // POST: Edificio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Sede,Id_Estado")] Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(edificio).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(edificio);
        }

        


        /// <summary>
        /// Elimina el edificio especificado por medio de la id
        /// </summary>
        /// <param name="id">Id del edificio que se va a eliminar</param>
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
                Edificio edificio = await bdMantox.Edificios.FindAsync(id);

                //Se valida si se encontró o no
                if (edificio == null)
                {
                    //Si no se encuentra, se devuelve error 404
                    return HttpNotFound();
                }

                //Finalmente, si se encuentra el edificio, se elimina
                bdMantox.Edificios.Remove(edificio);
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
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return; }
            try
            {

                //Llenar lista de Sedes
                sedes = bdMantox.Sedes.Select(sede => new
                {
                    SedeId = sede.Id,
                    SedeNombre = sede.Nombre,
                    EstadoId = sede.Id_Estado
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
                })
                .Where(e => e.EstadoTipo == "General" || e.EstadoTipo == "Equipo")
                .ToList();

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
        /// Devuelve un PartialView que contiene un MultiSelect de los Edificios filtrados por un id de sede
        /// </summary>
        /// <param name="idSede">Id de la sede</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarEdificios(string idSede = "1")
        {
            var id = int.Parse(idSede);
            List<Edificio> sedes = bdMantox.Edificios.Where(s => s.Id_Sede == id).ToList();

            ViewBag.Edificios = new MultiSelectList(sedes, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarEdificios");
        }
    }
}
