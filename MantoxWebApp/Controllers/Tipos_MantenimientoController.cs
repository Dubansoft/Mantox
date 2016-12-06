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
using FileHelper;
using System.Reflection;

namespace MantoxWebApp.Controllers
{
    public class Tipos_MantenimientoController : MantoxController
    {

        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Tipo Mantenimientos";
        public string NombreObjeto = "Tipo Mantenimiento";


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
        /// <returns>Vista Tipos_Mantenimientos</returns>
        public async Task<ActionResult> Ver()
        {
            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase", this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Tipos_Mantenimientos.ToListAsync()), RolDeUsuario.Desarrollador);

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Devuelve un PartialView que contiene una lista JSon de los Tipos_Mantenimientos filtrados por concepto de búsqueda, o por rango
        /// </summary>
        /// <param name="searchString">Términos de búsqueda</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idTipos_Mantenimientos">Id de la empresa</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>PartialView</returns>
        public PartialViewResult BuscarTipos_Mantenimientos(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            try
            {
                //Creamos nueva instancia de la clase parcial "v_tipos_mantenimientos"
                V_Tipos_Mantenimientos miVistaTipos_Mantenimientos = new V_Tipos_Mantenimientos();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaTipos_Mantenimientos.BuscarTipos_Mantenimientos(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosTipos_Mantenimientos = new DataTable();

                //Asignamos el valor tablaResultadosTipos_Mantenimientos tomando el valor del dicccionario
                tablaResultadosTipos_Mantenimientos = (DataTable)diccionarioResultados["TablaResultados"];

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
                ViewBag.TablaResultadosTipos_Mantenimientos = tablaResultadosTipos_Mantenimientos;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarTipos_Mantenimientos"), RolDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }


        // GET: Tipo_Mantenimiento/Create
        public ActionResult Create()
        {
            ViewBag.Titulo = "Crear tipo de mantenimiento";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View();
        }

        // POST: Tipo_Mantenimiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Tipos_Mantenimiento tipo_Mantenimiento)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Tipos_Mantenimiento.Add(tipo_Mantenimiento);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tipo_Mantenimiento);
        }

        // GET: Tipo_Mantenimiento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipos_Mantenimiento tipo_Mantenimiento = await bdMantox.Tipos_Mantenimiento.FindAsync(id);
            if (tipo_Mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Mantenimiento);
        }

        // POST: Tipo_Mantenimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Tipos_Mantenimiento tipo_Mantenimiento)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(tipo_Mantenimiento).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tipo_Mantenimiento);
        }

        // GET: Tipo_Mantenimiento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipos_Mantenimiento tipo_Mantenimiento = await bdMantox.Tipos_Mantenimiento.FindAsync(id);
            if (tipo_Mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Mantenimiento);
        }

        // POST: Tipo_Mantenimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tipos_Mantenimiento tipo_Mantenimiento = await bdMantox.Tipos_Mantenimiento.FindAsync(id);
            bdMantox.Tipos_Mantenimiento.Remove(tipo_Mantenimiento);
            await bdMantox.SaveChangesAsync();
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
    }
}
