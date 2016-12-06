using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarTipos_EquipoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
    }

    public partial class Tipos_Equipo : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase Tipos_EquipoViewModel en uno de clase Tipos_Equipo
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Tipos_Equipo(CrearEditarTipos_EquipoViewModel v)
        {
            Tipos_Equipo e = new Tipos_Equipo();

            e.Id = v.Id;
            e.Nombre = v.Nombre;

            return e;
        }
        
    }
}