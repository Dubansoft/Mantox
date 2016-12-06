using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarPropietarioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public int Id_Empresa { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Propietario : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase PropietarioViewModel en uno de clase Propietario
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Propietario(CrearEditarPropietarioViewModel v)
        {
            Propietario p = new Propietario();

            p.Id = v.Id;
            p.Nombre = v.Nombre;
            p.Id_Empresa = v.Id_Empresa;
            p.Id_Estado = v.Id_Estado;

            return p;
        }

        /// <summary>
        /// Verifica si existe el Propietario
        /// </summary>
        /// <param name="nombre">Nombre del Propietario</param>
        /// <param name="id_propietario">Id del Propietario</param>
        /// <returns>True si el Propietario existe en el Propietario enviado</returns>
        public bool Existe(string nombre, int id_propietario)
        {
            try
            {
                //Instancia de conexión por framework p base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Propietario propietarioQueSeVerifica = bdMantox.Propietarios
                    .Where(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
                    .Where(a => a.Id_Empresa == id_propietario)
                    .FirstOrDefault();

                return propietarioQueSeVerifica != null;

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }
}