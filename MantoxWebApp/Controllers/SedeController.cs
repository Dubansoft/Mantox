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
    public class SedeController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Sedes";
        public string NombreObjeto = "Sede";


        /// <summary>
        /// Lista de empresas disponibles en la lista desplegable
        /// </summary>
        IEnumerable empresas; //Almacenará la lista de empresas

        /// <summary>
        /// Lista de estados de objetos. Los estados disponibles varían según el tipo de objeto al cual están asociados. Ejemplos: Activo, inactivo, pendiente, suspendido, etc.
        /// </summary>
        IEnumerable estados; //Almacenará la lista de estados

        /// <summary>
        /// Lista de paises disponibles en la lista desplegable
        /// </summary>
        IEnumerable paises; //Almacenará la lista de paises

        /// <summary>
        /// Lista de departamentos disponibles en la lista desplegable
        /// </summary>
        IEnumerable Departamentos; //Almacenará la lista de departamentos

        /// <summary>
        /// Lista de ciudades disponibles en la lista desplegable
        /// </summary>
        IEnumerable ciudades; //Almacenará la lista de ciudades

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
        /// <returns>Vista Sedes</returns>
        public async Task<ActionResult> Ver()
        {
            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase", this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Sedes.ToListAsync()), RolDeUsuario.Reportes);

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
        public PartialViewResult BuscarSedes(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Reportes)) { return PartialView("Error401"); }

            try
            {
                //Creamos nueva instancia de la clase parcial "v_sedes"
                V_Sedes miVistaSedes = new V_Sedes();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaSedes.BuscarSedes(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosSedes = new DataTable();

                //Asignamos el valor tablaResultadosSedes tomando el valor del dicccionario
                tablaResultadosSedes = (DataTable)diccionarioResultados["TablaResultados"];

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
                ViewBag.TablaResultadosSedes = tablaResultadosSedes;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarSedes"), RolDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }



        /// <summary>
        /// Muestra un formulario para crear una sede nueva
        /// </summary>
        /// <returns></returns>
        public ActionResult Crear()
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Paises = new MultiSelectList(paises, "PaisId", "PaisNombre");
                ViewBag.Ciudades = new MultiSelectList(ciudades, "CiudadId", "CiudadNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Plantilla = "formTemplate";

                ViewBag.Accion = "Crear";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                return VistaAutenticada(View("Crear", new V_Sedes()), RolDeUsuario.Desarrollador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }


        // POST: Sede/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear([Bind(Include = "Id,Nombre,Ciudad,Departamento,Id_Empresa,Id_Estado")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sede);
        }


        // POST: Sede/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Ciudad,Departamento,Id_Empresa,Id_Estado")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(sede).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sede);
        }

        // GET: Sede/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = await bdMantox.Sedes.FindAsync(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
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

                //Llenar lista de Estados
                estados = bdMantox.Estados.Select(estado => new
                {
                    EstadoId = estado.Id,
                    EstadoNombre = estado.Nombre,
                    EstadoTipo = estado.Tipo
                })
                .Where(e => e.EstadoTipo == "General").ToList();

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

                        //Llenar lista de Paises
                        paises = bdMantox.Paises.Select(pais => new
                        {
                            PaisId = pais.Id,
                            PaisNombre = pais.Nombre
                        });

                        //Llenar lista de Paises
                        ciudades = bdMantox.Ciudades.Select(ciudad => new
                        {
                            CiudadId = ciudad.Id,
                            CiudadNombre = ciudad.Nombre
                        });
                        

                        break;
                    case RolDeUsuario.Administrador:
                    default:

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
                            (e => (int)e.EstadoId == (int)EstadoMantox.Activo);

                        //Llenar lista de Paises
                        paises = bdMantox.Paises.Select(pais => new
                        {
                            PaisId = pais.Id,
                            PaisNombre = pais.Nombre
                        });

                        //Llenar lista de Paises
                        ciudades = bdMantox.Ciudades.Select(ciudad => new
                        {
                            CiudadId = ciudad.Id,
                            CiudadNombre = ciudad.Nombre
                        });

                        break;
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Devuelve un PartialView que contiene un MultiSelect de las Sedes filtradas por un id de empresa
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarSedes(string idEmpresa = "1")
        {
            var id = int.Parse(idEmpresa);
            List<Sede> sedes = bdMantox.Sedes.Where(s => s.Id_Empresa == id).ToList();

            ViewBag.Sedes = new MultiSelectList(sedes, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarSedes");
        }
    }
}
