using FileHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace MantoxWebApp.Models
{
    public partial class UsuarioViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "La contraseña es muy corta")]
        public string Contrasena { get; set; }
        [Required]
        public int Id_Rol { get; set; }

        public int Id_Empresa { get; set; }
        public int Id_Sede { get; set; }
        public int Id_Edificio { get; set; }
        public int Piso { get; set; }

        [Required]
        public int Id_Area { get; set; }
        [Required]
        public int Id_Estado { get; set; }
    }

    public class IniciarSesionViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
    }


    public partial class Usuario : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase UsuarViewModel en uno de clase Usuario
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Usuario(UsuarioViewModel v)
        {
            Usuario u = new Usuario();

            u.Id = v.Id;
            u.Nombre = v.Nombre;
            u.Apellido = v.Apellido;
            u.Email = v.Email;
            u.Contrasena = v.Contrasena;
            u.Id_Rol = v.Id_Rol;
            u.Id_Estado = v.Id_Estado;
            u.Id_Area = v.Id_Area;

            return u;
        }

        /// <summary>
        /// Opeerador específico que convierte un objeto de clase UsuarViewModel en uno de clase Usuario
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Usuario(IniciarSesionViewModel v)
        {
            Usuario u = new Usuario();

            u.Id = 0;
            u.Nombre = string.Empty;
            u.Apellido = string.Empty;
            u.Email = v.Email;
            u.Contrasena = v.Contrasena;
            u.Id_Rol = 0;
            u.Id_Estado = 0;
            u.Id_Area = 0;

            return u;
        }

        /// <summary>
        /// Verifica si existe el usuario y la contraseña especificados
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns>True si el usuario existe y la contraseña es correcta</returns>
        public bool Existe(string email, string contrasena)
        {
            try
            {
                //Instancia de conexión por framework a base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Usuario usuarioQueSeAutentica = bdMantox.Usuarios
                    .Where(u => u.Email == email)
                    .Where(u => u.Contrasena == contrasena)
                    .FirstOrDefault();

                return usuarioQueSeAutentica != null;

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }

        ///// <summary>
        ///// Selecciona un usuario por medio del email y lo devuelve
        ///// </summary>
        ///// <param name="email">Email del usuario</param>
        ///// <returns></returns>
        //public V_Usuarios ObtenerVistaUsuario(string email)
        //{
        //    try
        //    {
        //        //Instancia de conexión por framework a base de datos
        //        MantoxDBEntities bdMantox = new MantoxDBEntities();

        //        //Selecciona la vista de usuario usando el email
        //        V_Usuarios vu = bdMantox.V_Usuarios
        //            .Where(u => u.Email == email)
        //            .FirstOrDefault();

        //        //Devuelve la vista de usuario
        //        return vu;

        //    }
        //    catch (Exception e)
        //    {
        //        EventLogger.LogEvent(this, e.Message.ToString(), e);
        //        return null;
        //    }
        //}

    }


}