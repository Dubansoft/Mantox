using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarEdificioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public int Id_Empresa { get; set; }

        [Required(ErrorMessage = "El campo Sede es obligatorio")]
        public int Id_Sede { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Edificio : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase EdificioViewModel en uno de clase Edificio
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Edificio(CrearEditarEdificioViewModel v)
        {
            Edificio e = new Edificio();

            e.Id = v.Id;
            e.Nombre = v.Nombre;
            e.Id_Sede = v.Id_Sede;
            e.Id_Estado = v.Id_Estado;

            return e;
        }

        /// <summary>
        /// Verifica si existe el edificio
        /// </summary>
        /// <param name="nombre">Nombre del edificio</param>
        /// <param name="id_edificio">Id del edificio</param>
        /// <returns>True si el edificio existe en el edificio enviado</returns>
        public bool Existe(string nombre, int id_edificio)
        {
            try
            {
                //Instancia de conexión por framework e base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Edificio edificioQueSeVerifica = bdMantox.Edificios
                    .Where(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
                    .Where(a => a.Id_Sede == id_edificio)
                    .FirstOrDefault();

                return edificioQueSeVerifica != null;

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }
}