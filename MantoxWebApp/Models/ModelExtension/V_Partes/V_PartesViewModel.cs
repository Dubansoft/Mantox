using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Partes". Contiene métodos encargados de traer información desde la vista "V_Partes" de la base de datos.
    /// </summary>
    public partial class V_Partes : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarPartes()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idParte">Id de la parte</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarPartes(string searchString, int idParte, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorParte = false;

            //El filtrado por parte NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                        //No se añaden restricciones a las partes que puede ver el desarrollador
                        break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                        filtrarPorParte = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Partes", searchString, idParte, sidx, sord, page, rows, searchField, filters, filtrarPorParte);
        }

        /// <summary>
        /// Convierte un objeto de clase Parte en un objeto de clase V_Parte
        /// </summary>
        /// <param name="e">Parte</param>
        public static explicit operator V_Partes(Parte e)
        {
            V_Partes vp = new V_Partes();

            vp.Id = e.Id;
            vp.Nombre = e.Nombre;
            vp.Id_Tipo_Equipo = e.Id_Tipo_Equipo;
            return vp;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarParteViewModel en un objeto de clase V_Partes
        /// </summary>
        /// <param name="partevm">CrearEditarParteViewModel</param>
        public static explicit operator V_Partes(CrearEditarParteViewModel partevm)
        {
            V_Partes ve = new V_Partes();

            ve.Id = partevm.Id;
            ve.Nombre = partevm.Nombre;
            ve.Id_Tipo_Equipo = partevm.Id_Tipo_Equipo;

            return ve;
        }



    }

}