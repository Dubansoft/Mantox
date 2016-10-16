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
using System.Data.SqlClient;
using System.Collections;
using FileHelper;
using System.Reflection;
using System.Linq.Expressions;

namespace MantoxWebApp.Controllers
{
    public class UsuarioController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Usuarios";
        public string NombreObjeto = "Usuario";

        /// <summary>
        /// Lista de roles de usuario
        /// </summary>
        IEnumerable roles; //Almacenará la lista de roles

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
        /// Obtiene una vista con la lista de usuarios registrados y sus detalles
        /// </summary>
        /// <returns>Vista Usuarios</returns>
        public async Task<ActionResult> Ver()
        {
            try
            {
                ViewBag.Accion = "Ver";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UrlBase",this.BaseUrl);

                return VistaAutenticada(View(await bdMantox.V_Usuarios.ToListAsync()), RoleDeUsuario.Reportes);

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Devuelve un PartialView que contiene una lista JSon de los usuarios filtrados por concepto de búsqueda, o por rango
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
        public PartialViewResult BuscarUsuarios(string searchString = "", int rows = 0, int page = 0, int idEmpresa = 0, string sidx = "", string sord = "", string searchField = "", string filters = "")
        {
            //Validar acceso
            if (!TieneAcceso(RoleDeUsuario.Reportes)){return PartialView("Error401");}

            try
            {
                //Creamos nueva instanacia de la clase parcial "vista_usuarios"
                V_Usuarios miVistaUsuarios = new V_Usuarios();

                //Creamos un diccionario para almacener los resultados devueltos por la consulta
                Dictionary<string, object> diccionarioResultados = new Dictionary<string, object>();

                //Ejecutamos la consulta a la base de datos y almacenamos los resultados en  el diccionario
                diccionarioResultados = miVistaUsuarios.BuscarUsuarios(searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);

                //Creamos tabla de datos para almacenar los resultados de la consulta en la base de datos
                DataTable tablaResultadosUsuarios = new DataTable();

                //Asignamos el valor tablaResultadosUsuarios tomando el valor del dicccionario
                tablaResultadosUsuarios = (DataTable)diccionarioResultados["TablaResultados"];

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
                ViewBag.TablaResultadosUsuarios = tablaResultadosUsuarios;
                ViewBag.FilasPorPagina = filasPorPagina;
                ViewBag.TotalFilas = totalFilas;
                ViewBag.PaginaActual = paginaActual;
                ViewBag.TotalPaginas = totalPaginas;

                //Devolvemos la vista
                return VistaAutenticada(PartialView("_VistaParcial_BuscarUsuarios"),RoleDeUsuario.Reportes);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("Error500");
            }

        }

        /// <summary>
        /// Devuelve un formulario de edición del usuario especificado por medio del Id
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <returns></returns>
        public ActionResult Editar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RoleDeUsuario.Administrador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Roles = new MultiSelectList(roles, "RolId", "RolNombre");
                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Plantilla = "formTemplate";
                ViewBag.Accion = "Editar";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                //Objeto de tipo V_Usuarios con los datos del usuario que se está editando
                ViewData.Add("UsuarioActual", bdMantox.V_Usuarios.FirstOrDefault(u => u.Id == id));

                //Se devuelve el formulario de creación de usuario con un objeto de tipo V_Usuarios con los datos del usuario que se está editando
                return VistaAutenticada(View("Crear", (V_Usuarios)bdMantox.Usuarios.Find(id)), RoleDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Muestra un formulario para crear un usuario nuevo
        /// </summary>
        /// <returns></returns>
        public ActionResult Crear()
        {
            //Validar acceso
            if (!TieneAcceso(RoleDeUsuario.Administrador)) { return PartialView("Error401"); }

            llenarListasDesplegables();

            try
            {
                ViewBag.Roles = new MultiSelectList(roles, "RolId", "RolNombre");
                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Plantilla = "formTemplate";

                ViewBag.Accion = "Crear";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                return VistaAutenticada(View("Crear", new V_Usuarios()), RoleDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }

        }

        /// <summary>
        /// Recibe los datos del formulario de creación de usuarios, los valida y los inserta a la base de datos.
        /// Este método también es usado para actualizar un usuario existente.
        /// </summary>
        /// <param name="usuariovm">CrearEditarUsuarioViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear([Bind(Include = "Id,Nombre,Apellido,Email,Contrasena,Id_Rol,Id_Area,Id_Estado,Id_Empresa,Id_Sede,Id_Edificio,Piso,Id_Area")] CrearEditarUsuarioViewModel usuariovm)
        {
            //Validar acceso
            if (!TieneAcceso(RoleDeUsuario.Administrador)) { return PartialView("Error401"); }

            try
            {
                //Creamos una instancia de usuario con los datos que recibimos del formulario
                Usuario nuevoUsuario = new Usuario();
                nuevoUsuario = (Usuario)usuariovm;

                //Creamos un diccionario con el email del usuario nuevo, para validar si ya existe
                Dictionary<string, string> criteria = new Dictionary<string, string>();
                criteria.Add("Email", nuevoUsuario.Email);

                //Si el usuario no es nuevo (el id > 0) entonces validamos primero si ha cambiado el email
                //Para ello consultamos a la bd y vemos el email actual y lo comparamos con el
                //email enviado.
                if (nuevoUsuario.Id > 0)
                {
                    //Seleccionamos el usuario por medio del id
                    V_Usuarios usuarioSeleccionado = bdMantox.V_Usuarios.FirstOrDefault(u => u.Id == nuevoUsuario.Id);

                    //Almacenamos el email en un string
                    string emailUsuarioSeleccionado = usuarioSeleccionado.Email;

                    //Comparamos este emaiil con el que se envió desde el formulario
                    if (emailUsuarioSeleccionado != nuevoUsuario.Email)
                    {
                        //Si es diferente, validamos que el email del usuario no exista en la bd.
                        if(bdMantox.V_Usuarios.FirstOrDefault(u => u.Email == nuevoUsuario.Email) != null)
                        {
                            //Si existe, se añade error al modelo.
                            ModelState.AddModelError("Email", "El email ingresado ya existe.");
                        }
                    }
                }
                else
                {
                    //Si el usuario es nuevo (id  = 0), validamos que el email no exista.
                    if (bdMantox.V_Usuarios.FirstOrDefault(u => u.Email == nuevoUsuario.Email) != null)
                    {
                        //Si existe, se añade error al modelo
                        ModelState.AddModelError("Email", "El email ingresado ya existe.");
                    }
                }

                //Validamos que no haya errores en el modelo
                if (ModelState.IsValid)
                {
                    //Si es usuario nuevo...
                    if (usuariovm.Id <= 0)
                    {
                        //Lo añadirmos a la base de datos
                        bdMantox.Usuarios.Add(nuevoUsuario);
                    }
                    else //Si es usuario existente (id > 0)
                    {
                        //Lo ponemos en estado modificado
                        bdMantox.Entry(nuevoUsuario).State = EntityState.Modified;
                    }

                    //Enviamos los cambios a la base de datos
                    await bdMantox.SaveChangesAsync();

                    //Redirigimos a la página de creación de usuario.
                    return RedirectToAction("Crear");
                }

                llenarListasDesplegables();

                ViewBag.Roles = new MultiSelectList(roles, "RolId", "RolNombre");
                ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");
                ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");
                ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

                ViewBag.Accion = "Editar";
                ViewBag.Plantilla = "formTemplate";

                ViewData.Add("NombreContexto", this.NombreContexto);
                ViewData.Add("NombreObjeto", this.NombreObjeto);
                ViewData.Add("NombreControlador", ControllerContext.RouteData.Values["controller"].ToString());

                ViewData.Add("UsuarioActual", (V_Usuarios)usuariovm);

                return VistaAutenticada(View((V_Usuarios)nuevoUsuario), RoleDeUsuario.Administrador);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }


        /// <summary>
        /// Elimina el usuario especificado por medio de la id
        /// </summary>
        /// <param name="id">Id del usuario que se va a eliminar</param>
        /// <returns></returns>
        public async Task<ActionResult> Eliminar(int? id)
        {
            //Validar acceso
            if (!TieneAcceso(RoleDeUsuario.Administrador)) { return PartialView("Error401"); }

            try
            {
                //Se valida si el id es nulo
                if (id == null)
                {
                    //Si es nulo se envía Error "Bad Request"
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Si no es nulo, se busca el id en la base de datos
                Usuario usuario = await bdMantox.Usuarios.FindAsync(id);

                //Se valida si se encontró o no
                if (usuario == null)
                {
                    //Si no se encuentra, se devuelve error 404
                    return HttpNotFound();
                }

                //Finalmente, si se encuentra el usuario, se elimina
                bdMantox.Usuarios.Remove(usuario);
                //Se guardan los cambios
                await bdMantox.SaveChangesAsync();
                //Se devuelve error 200 (ok)
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("ErrorInterno","Error");
            }
        }


        /// <summary>
        /// Vista inicial de inicio de sesión.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IniciarSesion()
        {
            return View();
        }

        /// <summary>
        /// Crea una sesión y almacena en ella todos los detalles del usuario, tomados de la vista vist_usuarios.
        /// Devuelve la vista de inicio si el usuario inició correctamente la sesión, de lo contrario, devuelve la pantalla de inicio de sesión con los mensajes de error correspondientes.
        /// </summary>
        /// <param name="usuario">Models.Usuario: El usuario que inicia la sesión</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult IniciarSesion(IniciarSesionViewModel usuarioQueSeAutentica)
        {
            try
            {
                //Instancia de conexión por framework a base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                //Validamos si el usuario está activo
                if (!((Usuario)usuarioQueSeAutentica).EsActivo(usuarioQueSeAutentica.Email))
                {
                    //Si no lo está, se envía mensaje de error al view
                    ModelState.AddModelError("Email", "El usuario ha sido desactivado. Consulte con el administrador del sistema.");
                }

                    if (ModelState.IsValid)
                    {
                        if (((Usuario)usuarioQueSeAutentica).Existe(usuarioQueSeAutentica.Email, usuarioQueSeAutentica.Contrasena))
                        {
                            MantoxSqlServerConnectionHelper myMantoxSqlServerConnectionHelper = new MantoxSqlServerConnectionHelper();

                            SqlParameter[] myParams = new SqlParameter[] {
                                new SqlParameter("@email", usuarioQueSeAutentica.Email),
                                new SqlParameter("@contrasena", usuarioQueSeAutentica.Contrasena)
                            };


                            DataSet resDs = myMantoxSqlServerConnectionHelper.ConsultarQuery("paIniciarSesion", CommandType.StoredProcedure, myParams);

                            DataTable resDt = resDs.Tables[0];

                            if (resDt.Rows.Count > 0)
                            {
                                Session["session"] = true;
                                foreach (DataColumn column in resDt.Columns)

                                {
                                    Session[column.ColumnName] = resDt.Rows[0][column.ColumnName];
                                }
                            }
                            return RedirectToAction("Index", "Usuario");
                        }
                        else
                        {
                            EliminarSesion();
                        }
                    }
                return View(usuarioQueSeAutentica);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario. Devuelve la vista de inicio de sesión.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CerrarSesion()
        {
            EliminarSesion();
            return RedirectToAction("IniciarSesion", "Usuario");
        }

        /// <summary>
        /// Método del sistema, no eliminar
        /// </summary>
        /// <param name="disposing"></param>
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
            if (!TieneAcceso(RoleDeUsuario.Administrador)) { return;  }
            try
            {

                //Llenar lista de Areas
                areas = bdMantox.Areas.Select(area => new
                {
                    AreaId = area.Id,
                    AreaNombre = area.Nombre
                }).ToList();

                //Llenar lista de Estados en variable temporal
                estados = bdMantox.Estados.Select(estado => new
                {
                    EstadoId = estado.Id,
                    EstadoNombre = estado.Nombre,
                    EstadoTipo = estado.Tipo
                }).Where(e => e.EstadoTipo == "General").ToList();


                //El filtrado por empresa NO debe estar activado para usuarios no desarrolladores:
                switch ((RoleDeUsuario)System.Web.HttpContext.Current.Session["Id_Rol"])
                {
                    case RoleDeUsuario.Desarrollador:
                        //No se añaden restricciones a las listas que puede ver el desarrollador

                        //Llenar lista de Roles
                        roles = bdMantox.Roles.Select(rol => new
                        {
                            RolId = rol.Id,
                            RolNombre = rol.Nombre
                        }).ToList();

                        //Llenar lista de Empresas
                        empresas = bdMantox.Empresas.Select(empresa => new
                        {
                            EmpresaId = empresa.Id,
                            EmpresaNombre = empresa.Nombre
                        }).ToList();
                        break;
                    case RoleDeUsuario.Administrador:
                    default:
                        //Almacenar id del rol del usuario actual en variable int
                        int idRolDeUsuarioActual = (int)Session["Id_Rol"];

                        //Llenar lista de Roles
                        roles = bdMantox.Roles.Select(rol => new
                        {
                            RolId = rol.Id,
                            RolNombre = rol.Nombre
                        })
                        .Where //Se añade condición de manera que solo pueda crear usuarios con el mismo rol o con rol más restringido
                            (r => (int)r.RolId >= idRolDeUsuarioActual)
                        .ToList();

                        //Almacenar id de la empresa del usuario actual en variable int
                        int idEmpresaDeUsuarioActual = (int)Session["Id_Empresa"];

                        //Llenar lista de Empresas
                        empresas = bdMantox.Empresas.Select(empresa => new
                        {
                            EmpresaId = empresa.Id,
                            EmpresaNombre = empresa.Nombre
                        })
                        .Where //Se añade condición de manera que solo pueda ver la empresa propia
                            (e => (int)e.EmpresaId == idEmpresaDeUsuarioActual ||
                            (int)e.EmpresaId == 1)
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
