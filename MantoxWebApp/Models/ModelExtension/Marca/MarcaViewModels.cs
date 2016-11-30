using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarMarcaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Marca : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase UsuarViewModel en uno de clase Marca
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Marca(CrearEditarMarcaViewModel v)
        {
            Marca m = new Marca();

            m.Id = v.Id;
            m.Nombre = v.Nombre;
            m.Id_Estado = v.Id_Estado;

            return m;
        }

        /// <summary>
        /// Verifica si existe la marca
        /// </summary>
        /// <param name="nombre">Nombre de la marca</param>
        /// <param name="id_marca">Id de la marca</param>
        /// <returns>True si la marca existe</returns>
        public bool Existe(string nombre, int id_marca)
        {
            try
            {
                //Instancia de conexión por framework a base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Marca marcaQueSeVerifica = bdMantox.Marcas
                    .Where(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
                    .FirstOrDefault();

                return marcaQueSeVerifica != null;

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }
}