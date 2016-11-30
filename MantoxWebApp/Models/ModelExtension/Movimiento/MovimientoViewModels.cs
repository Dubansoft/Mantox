using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarMovimientoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public int Id_Equipo { get; set; }

        [Required(ErrorMessage = "El campo Sede es obligatorio")]
        public int Id_Area_Origen { get; set; }

        [Required(ErrorMessage = "El campo Edificio es obligatorio")]
        public int Id_Area_Destino { get; set; }

        [Required(ErrorMessage = "El campo Piso es obligatorio")]
        public int Id_Razon_Movimiento { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Usuario { get; set; }
    }

    public partial class Movimiento : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase UsuarViewModel en uno de clase Usuario
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Movimiento(CrearEditarMovimientoViewModel v)
        {
            Movimiento m = new Movimiento();

            m.Id = v.Id;
            m.Fecha = v.Fecha ;
            m.Id_Equipo = v.Id_Equipo;
            m.Id_Area_Origen = v.Id_Area_Origen;
            m.Id_Area_Destino = v.Id_Area_Destino;
            m.Id_Usuario = v.Id_Usuario;
            return m;
        }

    }
}