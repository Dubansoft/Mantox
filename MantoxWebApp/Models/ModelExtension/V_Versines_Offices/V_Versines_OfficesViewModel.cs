using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Versiones_Offices". Contiene métodos encargados de traer información desde la vista "V_Versiones_Offices" de la base de datos.
    /// </summary>
    public partial class V_Versiones_Offices : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarVersiones_Offices()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idVersiones_Office">Id de la Versiones_Office</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarVersiones_Offices(string searchString, int idVersiones_Office, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorVersiones_Office = false;

            //El filtrado por Versiones_Office NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                    //No se añaden restricciones a las Versiones_Offices que puede ver el desarrollador
                    break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                    filtrarPorVersiones_Office = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Versiones_Offices", searchString, idVersiones_Office, sidx, sord, page, rows, searchField, filters);
        }

        /// <summary>
        /// Convierte un objeto de clase Versiones_Office en un objeto de clase V_Versiones_Offices
        /// </summary>
        /// <param name="e">Versiones_Office</param>
        public static explicit operator V_Versiones_Offices(Versiones_Office e)
        {
            V_Versiones_Offices vp = new V_Versiones_Offices();

            vp.Id = e.Id;
            vp.Nombre = e.Nombre;
            return vp;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarVersiones_OfficeViewModel en un objeto de clase V_Versiones_Offices
        /// </summary>
        /// <param name="versiones_officevm">CrearEditarVersiones_OfficeViewModel</param>
        public static explicit operator V_Versiones_Offices(CrearEditarVersiones_OfficeViewModel versiones_officevm)
        {
            V_Versiones_Offices ve = new V_Versiones_Offices();

            ve.Id = versiones_officevm.Id;
            ve.Nombre = versiones_officevm.Nombre;

            return ve;
        }



    }

}