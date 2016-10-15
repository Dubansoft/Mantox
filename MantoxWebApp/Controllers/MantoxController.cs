//@*
//The MIT License(MIT)
//Copyright(c) 2016
//Jhorman Rodríguez,
//Yeison Mosquera

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// *@

using System;
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
        /// Este método recibe una vista estándar y devuelve la misma vista si el usuario está autenticado, de lo contrario, devuelve la vista de inicio de sesión.
        /// </summary>
        /// <param name="defaultView">La vista que se mostrará si hay una sesión de usuario activa.</param>
        /// <returns>View()</returns>
        public ActionResult VistaAutenticada(ActionResult defaultView)
        {
            try
            {
                if (HaySesion())
                {
                    return defaultView;
                }
                else
                {
                    return RedirectToAction("IniciarSesion", "Usuario");
                }
            }
            catch (Exception)
            {
                EliminarSesion();
                return RedirectToAction("IniciarSesion", "Usuario");
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
