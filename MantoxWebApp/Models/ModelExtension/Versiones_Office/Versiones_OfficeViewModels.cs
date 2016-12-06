using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarVersiones_OfficeViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
    }

    public partial class Versiones_Office : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase ParteViewModel en uno de clase Versiones_Office
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Versiones_Office(CrearEditarVersiones_OfficeViewModel v)
        {
            Versiones_Office o = new Versiones_Office();

            o.Id = v.Id;
            o.Nombre = v.Nombre;

            return o;
        }
        
    }
}