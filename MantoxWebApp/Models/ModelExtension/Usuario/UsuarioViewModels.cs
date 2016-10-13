﻿using FileHelper;
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

    public class LoginViewModel
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
        public static explicit operator Usuario(LoginViewModel v)
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
                MantoxSqlServerConnectionHelper myMantoxSqlServerConnectionHelper = new MantoxSqlServerConnectionHelper();
                
                SqlParameter[] misParametros = new SqlParameter[] {
                    new SqlParameter("@email", email),
                    new SqlParameter("@contrasena", contrasena)
                };

                int cuentaUsuarios = myMantoxSqlServerConnectionHelper.ObtenerConteo("paUsuarioExiste", CommandType.StoredProcedure, misParametros);

                if (cuentaUsuarios == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }


}