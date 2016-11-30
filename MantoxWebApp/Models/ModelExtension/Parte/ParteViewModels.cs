using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarParteViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public int Id_Tipo_Equipo { get; set; }
    }

    public partial class Parte : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase ParteViewModel en uno de clase Parte
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Parte(CrearEditarParteViewModel v)
        {
            Parte p = new Parte();

            p.Id = v.Id;
            p.Nombre = v.Nombre;
            p.Id_Tipo_Equipo = v.Id_Tipo_Equipo;

            return p;
        }
        
    }
}