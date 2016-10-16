//@*
//The MIT License(MIT)
//Copyright(c) 2016
//Jhorman Rodríguez,
//Yeison Mosquera

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// *@

using FileHelper;
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MantoxWebApp.Controllers
{
    public class MantoxController : Controller
    {
        /// <summary>
        /// La url base de la aplicación
        /// </summary>
        public string BaseUrl{get{return  "http://localhost:50029/";}}

        /// <summary>
        /// Este método recibe una vista estándar y devuelve la misma vista si el usuario está autenticado
        /// y tiene acceso a la vista.
        /// </summary>
        /// <param name="defaultView">La vista que se mostrará si hay una sesión de usuario activa.</param>
        /// <returns>View()</returns>
        public ActionResult VistaAutenticada(ActionResult defaultView, RoleDeUsuario rolPermitido)
        {
            try
            {
                //Si hay sesión se valida si tiene acceso
                //Si no hay sesión,se envía a iniciar sesón
                //Si no tiene acceso se muestra 401: No autorizado


                if (HaySesion())
                {
                    //El siguiente proceso encripta el id del rol para usarlo en JavaScript
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] originalBytes = ASCIIEncoding.Default.GetBytes(System.Web.HttpContext.Current.Session["Id_Rol"].ToString());
                    byte[] encodedBytes = md5.ComputeHash(originalBytes);

                    //Creamos una variable en la sesión con la clave Id_Rol_Encriptado.
                    System.Web.HttpContext.Current.Session["Id_Rol_Encriptado"] = BitConverter.ToString(encodedBytes).Replace("-", "").ToLower();

                    //Eliminamos la clave Contrasena de la sesion
                    System.Web.HttpContext.Current.Session["Contrasena"] = "";

                    return TieneAcceso(defaultView, rolPermitido);

                }else
                {
                    return RedirectToAction("IniciarSesion", "Usuario");
                }


            }
            catch (Exception e)
            {
                EliminarSesion();
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }

        }

        /// <summary>
        /// Este método recibe una vista parcial y devuelve la misma vista si el usuario está autenticado y tiene acceso a la vista.
        /// </summary>
        /// <param name="defaultPartialView">La vista parcial que se mostrará si hay una sesión de usuario activa.</param>
        /// <returns>View()</returns>
        public PartialViewResult VistaAutenticada(PartialViewResult defaultPartialView, RoleDeUsuario rolPermitido)
        {
            try
            {
                //Si hay sesión se valida si tiene acceso
                //Si no hay sesión,se envía a iniciar sesón
                //Si no tiene acceso se muestra 401: No autorizado
                return (
                    HaySesion()) ?
                        TieneAcceso(defaultPartialView, rolPermitido) :
                    PartialView("Error401");

            }
            catch (Exception e)
            {
                EliminarSesion();
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return PartialView("Error500");
            }

        }

        /// <summary>
        /// Devuelve la vista enviada si tiene acceso a la misma, de lo contrario,
        /// devuelve la vida de acceso denegado (HTTP: 401).
        /// </summary>
        /// <param name="defaultView">La vista que se mostrará si tiene acceso</param>
        /// <param name="role">El rol máximo permitido para acceder a esta vista.</param>
        /// <returns></returns>
        public ActionResult TieneAcceso(ActionResult defaultView, RoleDeUsuario role)
        {
            try
            {
                //El rol maximo permitido
                int rolPermitido = (int)role;

                //El rol del usuario actual
                int idRolUsuarioActual = (int)System.Web.HttpContext.Current.Session["Id_Rol"];

                //Validamos si el usuario tiene permiso de acceder
                //Devolvemos la página de desautorización o
                //La vista solicitada
                return (idRolUsuarioActual <= rolPermitido) ? defaultView : View("Error401");
            }
            catch (Exception e)
            {
                EliminarSesion();
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return View("Error500");
            }


        }

        /// <summary>
        /// Devuelve la vista enviada si tiene acceso a la misma, de lo contrario,
        /// devuelve la vida de acceso denegado (HTTP: 401).
        /// </summary>
        /// <param name="defaultView">La vista que se mostrará si tiene acceso</param>
        /// <param name="role">El rol máximo permitido para acceder a esta vista.</param>
        /// <returns></returns>
        public PartialViewResult TieneAcceso(PartialViewResult defaultView, RoleDeUsuario role)
        {
            try
            {
                //El rol máximo permitido
                int rolPermitido = (int)role;

                //El rol del usuario actual
                int idRolUsuarioActual = (int)System.Web.HttpContext.Current.Session["Id_Rol"];

                //Validamos si el usuario tiene permiso de acceder
                //Devolvemos la página de desautorización o
                //La vista solicitada
                return (idRolUsuarioActual <= rolPermitido) ? defaultView : PartialView("Error401");

            }
            catch (Exception e)
            {
                EliminarSesion();
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return PartialView("Error500");
            }

        }

        /// <summary>
        /// Rol de usuario que tiene acceso al método
        /// </summary>
        /// <param name="role"></param>
        public bool TieneAcceso(RoleDeUsuario role)
        {
            try
            {
                //El rol maximo permitido
                int rolPermitido = (int)role;

                //El rol del usuario actual
                int idRolUsuarioActual = (int)System.Web.HttpContext.Current.Session["Id_Rol"];

                //Validamos si el usuario tiene permiso de acceder
                return (idRolUsuarioActual <= rolPermitido);
            }
            catch (Exception e)
            {
                EliminarSesion();
                EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return false;
            }


        }

        /// <summary>
        /// Devuelve True si hay una sesión activa, de lo contrario, devuelve False.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool HaySesion()
        {
            try
            {
                if (Convert.ToBoolean(Session["session"]) == true)
                {
                    return true;
                }
                else
                {
                    EliminarSesion();
                    return false;
                }
            }
            catch (Exception)
            {
                EliminarSesion();
                return false;
            }
        }

        /// <summary>
        /// Elimina la sesión actual y crea una sesión vacía con el parámetro "session" en False (Session["session"] = false;).
        /// </summary>
        public void EliminarSesion()
        {
            Session.RemoveAll();
            Session["session"] = false;
        }
    }
}
