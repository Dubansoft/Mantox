using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarLicenciaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public int Id_Tipo_Licencia { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public string Serial { get; set; }

        [Required(ErrorMessage = "El campo Sede es obligatorio")]
        public DateTime Fecha_Compra { get; set; }

        [Required(ErrorMessage = "El campo Sede es obligatorio")]
        public int Id_Equipo { get; set; }
    }


                   

    public partial class Licencia : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase LicenciaViewModel en uno de clase Licencia
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Licencia(CrearEditarLicenciaViewModel v)
        {
            Licencia l = new Licencia();

            l.Id = v.Id;
            l.Id_Tipo_Licencia = v.Id_Tipo_Licencia;
            l.Serial = v.Serial;
            l.Fecha_Compra = v.Fecha_Compra;
            l.Id_Equipo = v.Id_Equipo;

            return l;
        }

        /// <summary>
        /// Verifica si existe la licencia
        /// </summary>
        /// <param name="nombre">Nombre de la licencia</param>
        /// <param name="id_licencia">Id de la licencia</param>
        /// <returns>True si la licencia existe en el formulario enviado</returns>
        public bool Existe(string nombre, int id_licencia)
        {
            try
            {
                //Instancia de conexión por framework l base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Licencia licenciaQueSeVerifica = bdMantox.Licencias
                    .Where(a => a.Serial.ToLower().Trim() == nombre.ToLower().Trim())
                    .FirstOrDefault();

                return licenciaQueSeVerifica != null;

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }
}