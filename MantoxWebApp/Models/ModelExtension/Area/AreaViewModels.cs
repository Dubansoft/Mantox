using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarAreaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public int Id_Empresa { get; set; }

        [Required(ErrorMessage = "El campo Sede es obligatorio")]
        public int Id_Sede { get; set; }

        [Required(ErrorMessage = "El campo Edificio es obligatorio")]
        public int Id_Edificio { get; set; }

        [Required(ErrorMessage = "El campo Piso es obligatorio")]
        public int Piso { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Area : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase UsuarViewModel en uno de clase Usuario
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Area(CrearEditarAreaViewModel v)
        {
            Area a = new Area();

            a.Id = v.Id;
            a.Nombre = v.Nombre;
            a.Piso = v.Piso;
            a.Id_Edificio = v.Id_Edificio;
            a.Id_Estado = v.Id_Estado;

            return a;
        }

        /// <summary>
        /// Verifica si existe el area
        /// </summary>
        /// <param name="nombre">Nombre del área</param>
        /// <param name="id_edificio">Id del edificio</param>
        /// <returns>True si el area existe en el edificio enviado</returns>
        public bool Existe(string nombre, int id_edificio)
        {
            try
            {
                //Instancia de conexión por framework a base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Area areaQueSeVerifica = bdMantox.Areas
                    .Where(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
                    .Where(a => a.Id_Edificio == id_edificio)
                    .FirstOrDefault();

                return areaQueSeVerifica != null;

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }
}