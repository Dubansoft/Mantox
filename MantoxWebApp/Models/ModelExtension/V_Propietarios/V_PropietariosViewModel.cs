using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Propietarios". Contiene métodos encargados de traer información desde la vista "V_Propietarios" de la base de datos.
    /// </summary>
    public partial class V_Propietarios : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarPropietarios()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idPropietario">Id del Propietario</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarPropietarios(string searchString, int idPropietario, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorPropietario = false;

            //El filtrado por Propietario NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                    //No se añaden restricciones a las Propietarios que puede ver el desarrollador
                    break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                    filtrarPorPropietario = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Propietarios", searchString, idPropietario, sidx, sord, page, rows, searchField, filters, filtrarPorPropietario);
        }

        /// <summary>
        /// Convierte un objeto de clase Propietarios en un objeto de clase V_Propietarios
        /// </summary>
        /// <param name="e">Propietarios</param>
        public static explicit operator V_Propietarios(Propietario e)
        {
            V_Propietarios vp = new V_Propietarios();

            vp.Id = e.Id;
            vp.Nombre = e.Nombre;
            vp.Id_Empresa = e.Id_Empresa;
            vp.Id_Estado = e.Id_Estado;
            return vp;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarPropietarioViewModel en un objeto de clase V_Propietarios
        /// </summary>
        /// <param name="propietariovm">CrearEditarPropietarioViewModel</param>
        public static explicit operator V_Propietarios(CrearEditarPropietarioViewModel propietariovm)
        {
            V_Propietarios vp = new V_Propietarios();

            vp.Id = propietariovm.Id;
            vp.Nombre = propietariovm.Nombre;
            vp.Id_Empresa = propietariovm.Id_Empresa;
            vp.Id_Estado = propietariovm.Id_Estado;

            return vp;
        }



    }

}