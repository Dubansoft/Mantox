using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarTipos_LicenciaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
    }

    public partial class Tipos_Licencia : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase Tipos_LicenciaViewModel en uno de clase Tipos_Licencia
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Tipos_Licencia(CrearEditarTipos_LicenciaViewModel v)
        {
            Tipos_Licencia l = new Tipos_Licencia();

            l.Id = v.Id;
            l.Nombre = v.Nombre;
            return l;
        }
        
    }
}