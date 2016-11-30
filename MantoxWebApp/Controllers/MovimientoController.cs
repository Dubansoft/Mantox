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
    public class MovimientoController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Movimientos";
        public string NombreObjeto = "Movimiento";


        // <summary>
        /// Lista de áreas.
        /// </summary>
        IEnumerable areas; //Almacenará la lista de áreas

        // <summary>
        /// Lista de usuarios.
        /// </summary>
        IEnumerable usuarios; //Almacenará la lista de usuarios

        // <summary>
        /// Lista de equipos.
        /// </summary>
        IEnumerable equipos; //Almacenará la lista de equipos

        // <summary>
        /// Lista de movimiento.
        /// </summary>
        IEnumerable movimientos; //Almacenará la lista de los movimiento

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
        /// <returns>Vista Movimientos</returns>
        public async Task<ActionResult> Ver()
        {
            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase", this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Movimientos.ToListAsync()), RolDeUsuario.Desarrollador);

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
        /// <param name="idMovimiento">Id del movimiento</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>PartialView</returns>
        public PartialViewResult BuscarMovimientos(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            try
            {
                //Creamos nueva instancia de la clase parcial "v_movimientos"
                V_Movimientos miVistaMovimientos = new V_Movimientos();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaMovimientos.BuscarMovimientos(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosMovimientos = new DataTable();

                //Asignamos el valor tablaResultadosMovimientos tomando el valor del dicccionario
                tablaResultadosMovimientos = (DataTable)diccionarioResultados["TablaResultados"];

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
                ViewBag.TablaResultadosMovimientos = tablaResultadosMovimientos;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarMovimientos"), RolDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }


        /// <summary>
        /// Muestra un formulario para crear un movimiento nuevo
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
                ViewBag.Usuarios = new MultiSelectList(usuarios, "UsuarioId", "UsuarioNombre");
                ViewBag.Equipos = new MultiSelectList(equipos, "EquipoId", "EquipoNombre");
                ViewBag.Movimientos = new MultiSelectList(movimientos, "MovimientoId", "MovimientoNombre");

                ViewBag.Plantilla = "formTemplate";

                ViewBag.Accion = "Crear";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                return VistaAutenticada(View("Crear", new V_Movimientos()), RolDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Recibe los datos del formulario de creación de movimientos, los valida y los inserta a la base de datos.
        /// Este método también es usado para actualizar un movimiento existente.
        /// </summary>
        /// <param name="movimientovm">CrearEditarMovimientoViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear([Bind(Include = "Id,Fecha,Id_Equipo,Id_Area_Origen,Id_Area_Destino,Id_Razon_Movimiento,Id_Usuario")] CrearEditarMovimientoViewModel movimientovm)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Administrador)) { return PartialView("Error401"); }

            try
            {
                //Creamos una instancia de movimiento con los datos que recibimos del formulario
                Movimiento movimientoRecibido = new Movimiento();
                movimientoRecibido = (Movimiento)movimientovm;

               
                //Validamos que no haya errores en el modelo
                if (ModelState.IsValid)
                {
                    //Si el movimiento es nuevo...
                    if (movimientovm.Id <= 0)
                    {
                        //Lo añadirmos a la base de datos
                        bdMantox.Movimientos.Add(movimientoRecibido);
                    }
                    else //Si es un movimiento existente (id > 0)
                    {
                        //Lo ponemos en estado modificado
                        bdMantox.Entry(movimientoRecibido).State = EntityState.Modified;
                    }

                    //Enviamos los cambios a la base de datos
                    await bdMantox.SaveChangesAsync();

                    //Redirigimos a la página de creación de movimiento.
                    return RedirectToAction("Crear");
                }

                //Si el modelo tiene errores de validación, se crea nuevamente el
                //formulario y se muestra con los errores

                llenarListasDesplegables();

                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Usuarios = new MultiSelectList(usuarios, "UsuarioId", "UsuarioNombre");
                ViewBag.Equipos = new MultiSelectList(equipos, "EquipoId", "EquipoNombre");
                ViewBag.Movimientos = new MultiSelectList(movimientos, "MovimientoId", "MovimientoNombre");

                ViewBag.Accion = "Editar";
                ViewBag.Plantilla = "formTemplate";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("MovimientoActual", (V_Movimientos)movimientovm);

                return VistaAutenticada(View((V_Movimientos)movimientoRecibido), RolDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        public ActionResult Editar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RolDeUsuario.Desarrollador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Usuarios = new MultiSelectList(usuarios, "UsuarioId", "UsuarioNombre");
                ViewBag.Equipos = new MultiSelectList(equipos, "EquipoId", "EquipoNombre");
                ViewBag.Movimientos = new MultiSelectList(movimientos, "MovimientoId", "MovimientoNombre");

                ViewBag.Plantilla = "formTemplate";
                ViewBag.Accion = "Editar";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                //Objeto de tipo V_Movimientos con los datos del movimiento que se está editando
                ViewData.Add("MovimientoActual", bdMantox.V_Movimientos.FirstOrDefault(u => u.Id == id));

                //Se devuelve el formulario de creación del movimiento con un objeto de tipo V_Movimientos con los datos del movimiento que se está editando
                return VistaAutenticada(View("Crear", (V_Movimientos)bdMantox.Movimientos.Find(id)), RolDeUsuario.Desarrollador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }


        // POST: Movimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Fecha,Id_Equipo,Id_Area_Origen,Id_Area_Destino,Id_Razon_Movimiento,Id_Usuario")] Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(movimiento).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movimiento);
        }

        /// <summary>
        /// Elimina el movimiento especificado por medio de la id
        /// </summary>
        /// <param name="id">Id del movimiento que se va a eliminar</param>
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
                Movimiento movimiento = await bdMantox.Movimientos.FindAsync(id);

                //Se valida si se encontró o no
                if (movimiento == null)
                {
                    //Si no se encuentra, se devuelve error 404
                    return HttpNotFound();
                }

                //Finalmente, si se encuentra el movimiento, se elimina
                bdMantox.Movimientos.Remove(movimiento);
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
                    AreaNombre = area.Nombre
                });

                //Llenar lista de Usuarios
                usuarios = bdMantox.Usuarios.Select(usuario => new
                {
                    UsuarioId = usuario.Id,
                    UsuarioNombre = usuario.Nombre
                });

                //Llenar lista de Equipos
                equipos = bdMantox.Equipos.Select(equipo => new
                {
                    EquipoId = equipo.Id,
                    EquipoNombre = equipo.Activo
                });

                //Llenar lista de Movimientos
                movimientos = bdMantox.Razones_Movimiento.Select(movimiento => new
                {
                    MovimientoId = movimiento.Id,
                    MovimientoNombre = movimiento.Nombre
                });




                //El filtrado por empresa NO debe estar activado para usuarios no desarrolladores:
                switch ((RolDeUsuario)System.Web.HttpContext.Current.Session["Id_Rol"])
                {
                   
                    case RolDeUsuario.Administrador:
                    default:
                        //Almacenar id del rol del usuario actual en variable int
                        int idRolDeUsuarioActual = (int)Session["Id_Rol"];
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
