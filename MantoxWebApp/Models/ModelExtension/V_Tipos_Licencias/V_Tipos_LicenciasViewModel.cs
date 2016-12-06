using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Tipos_Licencias". Contiene métodos encargados de traer información desde la vista "V_Tipos_Licencias" de la base de datos.
    /// </summary>
    public partial class V_Tipos_Licencias : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarTipos_Licencias()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idTipos_Licencia">Id de la Tipos_Licencia</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarTipos_Licencias(string searchString, int idTipos_Licencia, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorTipos_Licencia = false;

            //El filtrado por Tipos_Licencia NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                    //No se añaden restricciones a las Tipos_Licencias que puede ver el desarrollador
                    break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                    filtrarPorTipos_Licencia = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Tipos_Licencias", searchString, idTipos_Licencia, sidx, sord, page, rows, searchField, filters);
        }

        /// <summary>
        /// Convierte un objeto de clase Tipos_Licencias en un objeto de clase V_Tipos_Licencias
        /// </summary>
        /// <param name="e">Tipos_Licencia</param>
        public static explicit operator V_Tipos_Licencias(Tipos_Licencia e)
        {
            V_Tipos_Licencias vp = new V_Tipos_Licencias();

            vp.Id = e.Id;
            vp.Nombre = e.Nombre;
            return vp;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarTipos_LicenciaViewModel en un objeto de clase V_Tipos_Licencias
        /// </summary>
        /// <param name="tipos_licenciasvm">CrearEditarTipos_LicenciaViewModel</param>
        public static explicit operator V_Tipos_Licencias(CrearEditarTipos_LicenciaViewModel tipos_licenciasvm)
        {
            V_Tipos_Licencias ve = new V_Tipos_Licencias();

            ve.Id = tipos_licenciasvm.Id;
            ve.Nombre = tipos_licenciasvm.Nombre;

            return ve;
        }



    }

}