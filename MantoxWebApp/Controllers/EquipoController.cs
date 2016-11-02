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
    public class EquipoController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Equipos";
        public string NombreObjeto = "Equipo";

        /// <summary>
        /// Lista de usuarios responsables de equipos
        /// </summary>
        IEnumerable responsables; //Almacenará la lista de responsables de equipos

        /// <summary>
        /// Lista de modelos de equipos
        /// </summary>
        IEnumerable modelos; //Almacenará la lista de modelos de equipos

        /// <summary>
        /// Lista de sistemas operativos de equipos
        /// </summary>
        IEnumerable sistemasoperativos; //Almacenará la lista de sistemas operativos de equipos

        /// <summary>
        /// Lista de versiones office de equipos
        /// </summary>
        IEnumerable versionesoffice; //Almacenará la lista de versiones office

        /// <summary>
        /// Lista de empresas registradas en Mantox
        /// </summary>
        IEnumerable empresas; //Almacenará la lista de empresas

        /// <summary>
        /// Lista de propietarios de equipos
        /// </summary>
        IEnumerable propietarios; //Almacenará la lista de propietarios

        /// <summary>
        /// Lista de estados de objetos. Los estados disponibles varían según el tipo de objeto al cual están asociados. Ejemplos: Activo, inactivo, pendiente, suspendido, etc.
        /// </summary>
        IEnumerable estados; //Almacenará la lista de estados

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
        /// <returns>Vista Equipos</returns>
        public async Task<ActionResult> Ver()
        {
            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase", this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Equipos.ToListAsync()), RolDeUsuario.Reportes);

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
        public PartialViewResult BuscarEquipos(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Reportes)) { return PartialView("Error401"); }

            try
            {
                //Creamos nueva instancia de la clase parcial "v_equipos"
                V_Equipos miVistaEquipos = new V_Equipos();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaEquipos.BuscarEquipos(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosEquipos = new DataTable();

                //Asignamos el valor tablaResultadosEquipos tomando el valor del dicccionario
                tablaResultadosEquipos = (DataTable)diccionarioResultados["TablaResultados"];

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
                ViewBag.TablaResultadosEquipos = tablaResultadosEquipos;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarEquipos"), RolDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }

        /// <summary>
        /// Devuelve una tabla con los detalles del equipo en cuestión.
        /// </summary>
        /// <param name="id">El id del equipo</param>
        /// <returns></returns>
        public PartialViewResult Detalles(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Reportes)) { return PartialView("Error401"); }

            try
            {
                //Validamos si el id pasado es nulo
                if (id == null)
                {
                    //Devolvemos error 400 si es nulo el id
                    return PartialView("Error400");
                }

                //Buscamos el id en la base de datos
                Equipo equipo = bdMantox.Equipos.Find(id);

                //Validamos si se encontró el id en la base de datos
                if (equipo == null)
                {
                    //Retornamos error 404 si no se encuentra
                    return PartialView("Error404");
                }

                //Creamos el conector a la base de datos
                MantoxSqlServerConnectionHelper conector = new MantoxSqlServerConnectionHelper();

                //Consultamos la base de datos y almacenamos los resultados en un dataset
                DataSet resDs = conector.ConsultarQuery("SELECT * FROM V_Equipos WHERE Id=" + id + "");

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                //Añadimos la tabla de resultados al ViewBag de la vista
                ViewBag.TablaDetallesEquipo = resDs.Tables[0];

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_DetallesEquipo"), RolDeUsuario.Reportes);

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return PartialView("Error500");
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
                ViewBag.Modelos = new MultiSelectList(modelos, "ModeloId", "ModeloNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.SistemasOperativos = new MultiSelectList(sistemasoperativos, "SistemaOperativoId", "SistemaOperativoNombre");
                ViewBag.VersionesOffice = new MultiSelectList(versionesoffice, "VersionOfficeId", "VersionOfficeNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");
                ViewBag.Propietarios = new MultiSelectList(propietarios, "propietarioId", "propietarioNombre");

                ViewBag.Plantilla = "formTemplate";

                ViewBag.Accion = "Crear";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                return VistaAutenticada(View("Crear", new V_Equipos()), RolDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }


        }

        // POST: Equipo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Activo,Serial,Nombre_Equipo,Ip,Comentario,Fecha_Ingreso,Fecha_Fin_Garantia,Id_Responsable,Id_Area,Id_Modelo,Id_Sistema_Operativo,Id_Propietario,Id_Version_Office,Id_Estado")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Equipos.Add(equipo);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(equipo);
        }

        // GET: Equipo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = await bdMantox.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Titulo = "Editar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View(equipo);
        }

        // POST: Equipo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Activo,Serial,Nombre_Equipo,Ip,Comentario,Fecha_Ingreso,Fecha_Fin_Garantia,Id_Responsable,Id_Area,Id_Modelo,Id_Sistema_Operativo,Id_Propietario,Id_Version_Office,Id_Estado")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(equipo).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Titulo = "Editar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View(equipo);
        }

        // GET: Equipo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = await bdMantox.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Titulo = "Eliminar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);


            return View(equipo);
        }

        // POST: Equipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Equipo equipo = await bdMantox.Equipos.FindAsync(id);
            bdMantox.Equipos.Remove(equipo);
            await bdMantox.SaveChangesAsync();

            ViewBag.Titulo = "Eliminar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return RedirectToAction("Index");
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
                //Select para Modelos
                modelos = bdMantox.Modelos.Select(modelo => new
                {
                    ModeloId = modelo.Id,
                    ModeloNombre = modelo.Nombre,
                    EstadoId = modelo.Id_Estado
                })
                .Where //Se añade condición para mostrar sólo modelos activos
                    (e => (int)e.EstadoId == (int)EstadoMantox.Activo)
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

                //OJO CARGA ASINCRONICA
                //Select para Sistemas operativos
                sistemasoperativos = bdMantox.Sistemas_Operativos.Select(sistemaoperativo => new
                {
                    SistemaOperativoId = sistemaoperativo.Id,
                    SistemaOperativoNombre = sistemaoperativo.Nombre
                }).ToList();

                ////Select para Versiones Office
                versionesoffice = bdMantox.Versiones_Office.Select(versionoffice => new
                {
                    VersionOfficeId = versionoffice.Id,
                    VersionOfficeNombre = versionoffice.Nombre
                }).ToList();

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

                        //Select para Propietarios
                        propietarios = bdMantox.V_Usuarios.Select(propietario => new
                        {
                            PropietarioId = propietario.Id,
                            PropietarioNombre = propietario.Nombre,
                            EstadoId = propietario.Id_Estado
                        })
                        .Where //Se añade condición para mostrar sólo usuarios activos
                            (p => (int)p.EstadoId == (int)EstadoMantox.Activo)
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

                        //Select para Propietarios
                        propietarios = bdMantox.V_Usuarios.Select(propietario => new
                        {
                            PropietarioId = propietario.Id,
                            PropietarioNombre = propietario.Nombre,
                            EstadoId = propietario.Id_Estado,
                            EmpresaId = propietario.Id_Empresa
                        })
                        .Where //Se añade condición para mostrar sólo usuarios activos
                            (p => (int)p.EstadoId == (int)EstadoMantox.Activo)
                        .Where //Se añade condición de manera que solo pueda ver la empresa propia
                            (p => (int)p.EmpresaId == idEmpresaDeUsuarioActual || (int)p.EmpresaId == 1)
                        .ToList();
                        break;
                }
            }

            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
            }

        }
    }
}
