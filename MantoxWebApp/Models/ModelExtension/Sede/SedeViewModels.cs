using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarSedeViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public int Id_Empresa { get; set; }

        [Required(ErrorMessage = "El campo Sede es obligatorio")]
        public int Id_Pais { get; set; }

        [Required(ErrorMessage = "El campo Edificio es obligatorio")]
        public int Id_Departamento { get; set; }

        [Required(ErrorMessage = "El campo Piso es obligatorio")]
        public int Id_Ciudad { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Sede : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase UsuarViewModel en uno de clase Usuario
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Sede(CrearEditarSedeViewModel v)
        {
            Sede s = new Sede();
            
            s.Id = v.Id;
            s.Nombre = v.Nombre;
            s.Id_Empresa = v.Id_Empresa;
            s.Id_Ciudad = v.Id_Ciudad;
            s.Id_Estado = v.Id_Estado;

            return s;
        }

        /// <summary>
        /// Verifica si existe el area
        /// </summary>
        /// <param name="nombre">Nombre del área</param>
        /// <param name="id_edificio">Id del edificio</param>
        /// <returns>True si el area existe en el edificio enviado</returns>
        //public bool Existe(string nombre, int id_edificio)
        //{
        //    try
        //    {
        //        Instancia de conexión por framework s base de datos
        //        MantoxDBEntities bdMantox = new MantoxDBEntities();

        //        Sede sedeQueSeVerifica = bdMantox.Sedes
        //            .Where(s => s.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
        //            .Where(s => s.Id_Sede == id_edificio)
        //            .FirstOrDefault();

        //        return sedeQueSeVerifica != null;

        //    }
        //    catch (Exception e)
        //    {
        //        EventLogger.LogEvent(this, e.Message.ToString(), e);
        //        return false;
        //    }

        //}
    }
}