using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Tipos_Equipos". Contiene métodos encargados de traer información desde la vista "V_Partes" de la base de datos.
    /// </summary>
    public partial class V_Tipos_Equipos : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarPartes()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idTipos_Equipo">Id de la Tipos_Equipo</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarTipos_Equipos(string searchString, int idTipos_Equipo, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorTipos_Equipo = false;

            //El filtrado por Tipos_Equipo NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                    //No se añaden restricciones a las Tipos_Equipos que puede ver el desarrollador
                    break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                    filtrarPorTipos_Equipo = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Tipos_Equipos", searchString, idTipos_Equipo, sidx, sord, page, rows, searchField, filters, filtrarPorTipos_Equipo);
        }

        /// <summary>
        /// Convierte un objeto de clase Tipos_Equipo en un objeto de clase V_Tipos_Equipo
        /// </summary>
        /// <param name="e">Tipos_Equipo</param>
        public static explicit operator V_Tipos_Equipos(Tipos_Equipo e)
        {
            V_Tipos_Equipos vp = new V_Tipos_Equipos();

            vp.Id = e.Id;
            vp.Nombre = e.Nombre;
            return vp;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarTipos_EquipoViewModel en un objeto de clase V_Tipos_Equipos
        /// </summary>
        /// <param name="tipos_equiposvm">CrearEditartipos_equiposViewModel</param>
        public static explicit operator V_Tipos_Equipos(CrearEditarTipos_EquipoViewModel tipos_equiposvm)
        {
            V_Tipos_Equipos ve = new V_Tipos_Equipos();

            ve.Id = tipos_equiposvm.Id;
            ve.Nombre = tipos_equiposvm.Nombre;

            return ve;
        }



    }

}