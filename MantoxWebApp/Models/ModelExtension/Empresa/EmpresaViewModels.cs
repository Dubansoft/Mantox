using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarEmpresaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Empresa : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase UsuarViewModel en uno de clase Usuario
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Empresa(CrearEditarEmpresaViewModel v)
        {
            Empresa a = new Empresa();

            a.Id = v.Id;
            a.Nombre = v.Nombre;
            a.Id_Estado = v.Id_Estado;

            return a;
        }

        /// <summary>
        /// Verifica si existe la empresa
        /// </summary>
        /// <param name="nombre">Nombre del área</param>
        /// <param name="id_edificio">Id del edificio</param>
        /// <returns>True si el empresa existe en el edificio enviado</returns>
        public bool Existe(string nombre, int id_edificio)
        {
            try
            {
                //Instancia de conexión por framework a base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Empresa empresaQueSeVerifica = bdMantox.Empresas
                    .Where(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
                    .FirstOrDefault();

                return empresaQueSeVerifica != null;

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }
}