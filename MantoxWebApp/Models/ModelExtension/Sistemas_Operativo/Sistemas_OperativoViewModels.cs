using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarSistemas_OperativoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public string Id_Tipo_Equipo { get; set; }
    }

    public partial class Sistemas_Operativo : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase Sistemas_OperativoViewModel en uno de clase Parte
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Sistemas_Operativo(CrearEditarSistemas_OperativoViewModel v)
        {
            Sistemas_Operativo s = new Sistemas_Operativo();

            s.Id = v.Id;
            s.Nombre = v.Nombre;
            s.Id_Tipo_Equipo = null;

            return s;
        }
        
    }
}