using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarTipos_MantenimientoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
    }

    public partial class Tipos_Mantenimiento : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase Tipos_MantenimientoViewModel en uno de clase Tipos_Mantenimiento
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Tipos_Mantenimiento(CrearEditarTipos_MantenimientoViewModel v)
        {
            Tipos_Mantenimiento m = new Tipos_Mantenimiento();

            m.Id = v.Id;
            m.Nombre = v.Nombre;

            return m;
        }
        
    }
}