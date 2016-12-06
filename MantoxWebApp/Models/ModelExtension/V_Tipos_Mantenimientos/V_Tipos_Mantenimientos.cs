using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Tipos_Mantenimientos". Contiene métodos encargados de traer información desde la vista "V_Tipos_Mantenimientos" de la base de datos.
    /// </summary>
    public partial class V_Tipos_Mantenimientos : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarTipos_Mantenimientos()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idTipos_Mantenimiento">Id de la Tipos_Mantenimiento</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarTipos_Mantenimientos(string searchString, int idTipos_Mantenimiento, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorTipos_Mantenimiento = false;

            //El filtrado por Tipos_Mantenimiento NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                    //No se añaden restricciones a las Tipos_Mantenimientos que puede ver el desarrollador
                    break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                    filtrarPorTipos_Mantenimiento = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Tipos_Mantenimientos", searchString, idTipos_Mantenimiento, sidx, sord, page, rows, searchField, filters, filtrarPorTipos_Mantenimiento);
        }

        /// <summary>
        /// Convierte un objeto de clase Tipos_Mantenimiento en un objeto de clase V_Tipos_Mantenimiento
        /// </summary>
        /// <param name="e">Tipos_Mantenimiento</param>
        public static explicit operator V_Tipos_Mantenimientos(Tipos_Mantenimiento e)
        {
            V_Tipos_Mantenimientos vm = new V_Tipos_Mantenimientos();

            vm.Id = e.Id;
            vm.Nombre = e.Nombre;
            return vm;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarTipos_MantenimientoViewModel en un objeto de clase V_Tipos_Mantenimientos
        /// </summary>
        /// <param name="tipos_mantenimientosvm">CrearEditarTipos_MantenimientoViewModel</param>
        public static explicit operator V_Tipos_Mantenimientos(CrearEditarTipos_MantenimientoViewModel tipos_mantenimientosvm)
        {
            V_Tipos_Mantenimientos ve = new V_Tipos_Mantenimientos();

            ve.Id = tipos_mantenimientosvm.Id;
            ve.Nombre = tipos_mantenimientosvm.Nombre;

            return ve;
        }



    }

}