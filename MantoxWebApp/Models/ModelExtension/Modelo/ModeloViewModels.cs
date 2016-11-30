using FileHelper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace MantoxWebApp.Models
{
    public partial class CrearEditarModeloViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Sede es obligatorio")]
        public int Id_Marca { get; set; }

        [Required(ErrorMessage = "El campo Edificio es obligatorio")]
        public int Id_Tipo_Equipo { get; set; }
        
        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public int Id_Estado { get; set; }
    }

    public partial class Modelo : MantoxModel
    {
        /// <summary>
        /// Opeerador específico que convierte un objeto de clase ModeloViewModel en uno de clase Modelo
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator Modelo(CrearEditarModeloViewModel v)
        {
            Modelo m = new Modelo();

            m.Id = v.Id;
            m.Nombre = v.Nombre;
            m.Id_Marca = v.Id_Marca;
            m.Id_Tipo_Equipo = v.Id_Tipo_Equipo;
            m.Id_Estado = v.Id_Estado;

            return m;
        }

        /// <summary>
        /// Verifica si existe el modelo
        /// </summary>
        /// <param name="nombre">Nombre del modelo</param>
        /// <param name="id_modelo">Id del modelo</param>
        /// <returns>True si el modelo existe en el enviado</returns>
        public bool Existe(string nombre, int id_modelo)
        {
            try
            {
                //Instancia de conexión por framework m base de datos
                MantoxDBEntities bdMantox = new MantoxDBEntities();

                Modelo modeloQueSeVerifica = bdMantox.Modelos
                    .Where(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim())
                    .FirstOrDefault();

                return modeloQueSeVerifica != null;


            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }

        }
    }
}