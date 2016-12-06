using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarRazones_MovimientoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
    }

    public partial class Razones_Movimiento : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase Razones_MovimientoViewModel en uno de clase Razones_Movimiento
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Razones_Movimiento(CrearEditarRazones_MovimientoViewModel v)
        {
            Razones_Movimiento r = new Razones_Movimiento();

            r.Id = v.Id;
            r.Nombre = v.Nombre;

            return r;
        }
        
    }
}