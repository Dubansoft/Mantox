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
        public string Nombre_Equipo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Ip { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateTime Fecha_Ingreso { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateTime Fecha_Fin_Garantia { get; set; }

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
                Nombre_Equipo = v.Nombre_Equipo,
                Ip = v.Ip,
                Comentario = v.Comentario,
                Fecha_Ingreso = v.Fecha_Ingreso,
                Fecha_Fin_Garantia = v.Fecha_Fin_Garantia,
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

        ///// <summary>
        ///// Verifica si existe el equipo
        ///// </summary>
        ///// <param name="nombre">Nombre del área</param>
        ///// <param name="id_edificio">Id del edificio</param>
        ///// <returns>True si el usuario existe en el edificio enviado</returns>
        //public bool Existe(string nombre, int id_edificio)
        //{
        //    try
        //    {
        //        //Instancia de conexión por framework a base de datos
        //        MantoxDBEntities bdMantox = new MantoxDBEntities();

        //        Area areaQueSeVerifica = bdMantox.Areas
        //            .Where(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
        //            .Where(a => a.Id_Edificio == id_edificio)
        //            .FirstOrDefault();

        //        return areaQueSeVerifica != null;

        //    }
        //    catch (Exception e)
        //    {
        //        EventLogger.LogEvent(this, e.Message.ToString(), e);
        //        return false;
        //    }

        //}
    }
}