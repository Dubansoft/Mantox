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
    public class EmpresaController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Empresas";
        public string NombreObjeto = "Empresa";

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
        /// <returns>Vista Empresas</returns>
        public async Task<ActionResult> Ver()
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase", this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Empresas.ToListAsync()), RolDeUsuario.Reportes);

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
        public PartialViewResult BuscarEmpresas(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            try
            {
                //Creamos nueva instancia de la clase parcial "v_empresas"
                V_Empresas miVistaEmpresas = new V_Empresas();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaEmpresas.BuscarEmpresas(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosEmpresas = new DataTable();

                //Asignamos el valor tablaResultadosEmpresas tomando el valor del dicccionario
                tablaResultadosEmpresas = (DataTable)diccionarioResultados["TablaResultados"];

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
                ViewBag.TablaResultadosEmpresas = tablaResultadosEmpresas;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarEmpresas"), RolDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }

        /// <summary>
        /// Devuelve un formulario de edición de la empresa especificado por medio del Id
        /// </summary>
        /// <param name="id">Id de la empresa</param>
        /// <returns></returns>
        public ActionResult Editar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

                ViewBag.Plantilla = "formTemplate";
                ViewBag.Accion = "Editar";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                //Objeto de tipo V_Empresas con los datos de la empresa que se está editando
                ViewData.Add("EmpresaActual", bdMantox.V_Empresas.FirstOrDefault(e => e.Id == id));

                V_Empresas miempresa = (V_Empresas)bdMantox.Empresas.Find(id);

                //Se devuelve el formulario de creación de empresa con un objeto de tipo V_Empresas con los datos de la empresa que se está editando
                return VistaAutenticada(View("Crear", miempresa), RolDeUsuario.Desarrollador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Muestra un formulario para crear un empresa nueva
        /// </summary>
        /// <returns></returns>
        public ActionResult Crear()
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

                ViewBag.Plantilla = "formTemplate";

                ViewBag.Accion = "Crear";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                return VistaAutenticada(View("Crear", new V_Empresas()), RolDeUsuario.Desarrollador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Recibe los datos del formulario de creación de empresas, los valida y los inserta a la base de datos.
        /// Este método también es usado para actualizar una empesa existente.
        /// </summary>
        /// <param name="empresavm">CrearEditarEmpresaViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear([Bind(Include = "Id,Nombre,Id_Estado")] CrearEditarEmpresaViewModel empresavm)
        {

            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            try
            {
                //Creamos una instancia de empresa con los datos que recibimos del formulario
                Empresa empresaRecibida = new Empresa();
                empresaRecibida = (Empresa)empresavm;

                //Si la empresa no es nueva (el id > 0) entonces validamos primero si ya existe una empresa
                //con un id diferente pero con los mismos datos, esto identificaría un duplicado.
                if (empresaRecibida.Id > 0)
                {
                    //Buscamos un duplicado de la siguiente manera
                    if (bdMantox.V_Empresas.FirstOrDefault(
                        ve =>
                            ve.Nombre.Trim().ToLower() == empresaRecibida.Nombre.Trim().ToLower() &&
                            (int)ve.Id != (int)empresaRecibida.Id
                        ) != null)
                    {
                        //Si existe, se añade error al modelo.
                        ModelState.AddModelError("Nombre", "La empresa ingresada ya existe en el sistema.");
                    }
                }
                else
                {
                    //Si la empresa es nueva (id = 0), validamos que el no exista una con los mismos datos.
                    if (bdMantox.V_Empresas.FirstOrDefault(
                        ve =>
                            ve.Nombre.Trim().ToLower() == empresaRecibida.Nombre.Trim().ToLower()
                        ) != null)
                    {
                        //Si existe, se añade error al modelo
                        ModelState.AddModelError("Nombre", "La empresa ingresada ya existe.");
                    }
                }

                //Validamos que no haya errores en el modelo
                if (ModelState.IsValid)
                {
                    //Si es empresa nueva...
                    if (empresavm.Id <= 0)
                    {
                        //La añadirmos a la base de datos
                        bdMantox.Empresas.Add(empresaRecibida);
                    }
                    else //Si la empresa existente (id > 0)
                    {
                        //Lo ponemos en estado modificado
                        bdMantox.Entry(empresaRecibida).State = EntityState.Modified;
                    }

                    //Enviamos los cambios a la base de datos
                    await bdMantox.SaveChangesAsync();

                    //Redirigimos a la página de creación de empresa.
                    return RedirectToAction("Crear");
                }

                //Si el modelo tiene errores de validación, se crea nuevamente el
                //formulario y se muestra con los errores

                llenarListasDesplegables();

                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

                ViewBag.Accion = "Editar";
                ViewBag.Plantilla = "formTemplate";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("EmpresaActual", (V_Empresas)empresavm);

                return VistaAutenticada(View((V_Empresas)empresaRecibida), RolDeUsuario.Desarrollador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }



        // POST: Empresa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Estado")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(empresa).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        /// <summary>
        /// Elimina la empresa especificada por medio de la id
        /// </summary>
        /// <param name="id">Id de la empresa que se va a eliminar</param>
        /// <returns></returns>
        public async Task<ActionResult> Eliminar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            try
            {
                //Se valida si el id es nulo
                if (id == null)
                {
                    //Si es nulo se envía Error "Bad Request"
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Si no es nulo, se busca el id en la base de datos
                Empresa empresa = await bdMantox.Empresas.FindAsync(id);

                //Se valida si se encontró o no
                if (empresa == null)
                {
                    //Si no se encuentra, se devuelve error 404
                    return HttpNotFound();
                }

                //Finalmente, si se encuentra la empresa, se elimina
                bdMantox.Empresas.Remove(empresa);
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

                //Llenar lista de Estados
                estados = bdMantox.Estados.Select(estado => new
                {
                    EstadoId = estado.Id,
                    EstadoNombre = estado.Nombre,
                    EstadoTipo = estado.Tipo
                }).Where(e => e.EstadoTipo == "General").ToList();

            }

            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
            }

        }
    }
}
