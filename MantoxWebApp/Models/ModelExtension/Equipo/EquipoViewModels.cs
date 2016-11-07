using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarEquipoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Activo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Serial { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre_de_Equipo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Ip { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateTime Fecha_de_Ingreso { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Meses_de_Garantia { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Responsable { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Area { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Modelo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Sistema_Operativo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Propietario { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Version_Office { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Equipo : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase EquipoViewModel en uno de clase Equipo
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Equipo(CrearEditarEquipoViewModel v)
        {
            Equipo e = new Equipo
            {
                Id = v.Id,
                Activo = v.Activo,
                Serial = v.Serial,
                Nombre_Equipo = v.Nombre_de_Equipo,
                Ip = v.Ip,
                Comentario = v.Comentario,
                Fecha_Ingreso = v.Fecha_de_Ingreso,
                Meses_Garantia = v.Meses_de_Garantia,
                Id_Responsable = v.Id_Responsable,
                Id_Area = v.Id_Area,
                Id_Modelo = v.Id_Modelo,
                Id_Sistema_Operativo = v.Id_Sistema_Operativo,
                Id_Propietario = v.Id_Propietario,
                Id_Version_Office = v.Id_Version_Office,
                Id_Estado = v.Id_Estado,
            };

            return e;
        }

    }
}