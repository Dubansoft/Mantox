using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Sistemas_Operativos". Contiene métodos encargados de traer información desde la vista "V_Sistemas_Operativos" de la base de datos.
    /// </summary>
    public partial class V_Sistemas_Operativos : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarSistemas_Operativos()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idSistemas_Operativo">Id de la Sistemas_Operativo</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarSistemas_Operativos(string searchString, int idSistemas_Operativo, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorSistemas_Operativo = false;

            //El filtrado por Sistemas_Operativo NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                    //No se añaden restricciones a las Sistemas_Operativos que puede ver el desarrollador
                    break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                    filtrarPorSistemas_Operativo = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Sistemas_Operativos", searchString, idSistemas_Operativo, sidx, sord, page, rows, searchField, filters, filtrarPorSistemas_Operativo);
        }

        /// <summary>
        /// Convierte un objeto de clase Sistemas_Operativos en un objeto de clase V_Sistemas_Operativos
        /// </summary>
        /// <param name="e">Sistemas_Operativo</param>
        public static explicit operator V_Sistemas_Operativos(Sistemas_Operativo e)
        {
            V_Sistemas_Operativos vo = new V_Sistemas_Operativos();

            vo.Id = e.Id;
            vo.Nombre = e.Nombre;
            vo.Nombre_de_Equipo = null;
            return vo;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarSistemas_OperativoViewModel en un objeto de clase V_Sistemas_Operativos
        /// </summary>
        /// <param name="sistemas_operativovm">CrearEditarSistemas_OperativoViewModel</param>
        public static explicit operator V_Sistemas_Operativos(CrearEditarSistemas_OperativoViewModel sistemas_operativovm)
        {
            V_Sistemas_Operativos vo = new V_Sistemas_Operativos();

            vo.Id = sistemas_operativovm.Id;
            vo.Nombre = sistemas_operativovm.Nombre;
            vo.Nombre_de_Equipo = null;

            return vo;
        }



    }

}