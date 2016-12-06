using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Razones_Movimientos". Contiene métodos encargados de traer información desde la vista "V_Razones_Movimientos" de la base de datos.
    /// </summary>
    public partial class V_Razones_Movimientos : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarRazones_Movimientos()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idRazones_Movimientos">Id de la Razones_Movimientos</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarRazones_Movimientos(string searchString, int idRazones_Movimientos, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorRazones_Movimientos = false;

            //El filtrado por Razones_Movimientos NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                    //No se añaden restricciones a las Razones_Movimientos que puede ver el desarrollador
                    break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                         filtrarPorRazones_Movimientos = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Razones_Movimientos", searchString, idRazones_Movimientos, sidx, sord, page, rows, searchField, filters, filtrarPorRazones_Movimientos);
        }

        /// <summary>
        /// Convierte un objeto de clase Razones_Movimientos en un objeto de clase V_Razones_Movimientos
        /// </summary>
        /// <param name="e">Razones_Movimientos</param>
        public static explicit operator V_Razones_Movimientos(Razones_Movimiento e)
        {
            V_Razones_Movimientos vp = new V_Razones_Movimientos();

            vp.Id = e.Id;
            vp.Nombre = e.Nombre;
            return vp;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarRazones_MovimientosViewModel en un objeto de clase V_Razones_Movimientos
        /// </summary>
        /// <param name="Razones_Movimientovm">CrearEditarRazones_MovimientosViewModel</param>
        public static explicit operator V_Razones_Movimientos(CrearEditarRazones_MovimientoViewModel Razones_Movimientovm)
        {
            V_Razones_Movimientos vr = new V_Razones_Movimientos();

            vr.Id = Razones_Movimientovm.Id;
            vr.Nombre = Razones_Movimientovm.Nombre;
            return vr;
        }



    }

}