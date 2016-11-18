using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Edificios". Contiene métodos encargados de traer información desde la vista "V_Usuarios" de la base de datos.
    /// </summary>
    public partial class V_Edificios : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarUsuarios()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idEdificio">Id de la edificio</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarEdificios(string searchString, int idEdificio, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorEdificio = false;

            //El filtrado por edificio NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                        //No se añaden restricciones a los edificios que puede ver el desarrollador
                        break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                        filtrarPorEdificio = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Edificios", searchString, idEdificio, sidx, sord, page, rows, searchField, filters, filtrarPorEdificio);
        }

        /// <summary>
        /// Convierte un objeto de clase Edificio en un objeto de clase V_Edificio
        /// </summary>
        /// <param name="e">Edificio</param>
        public static explicit operator V_Edificios(Edificio e)
        {
            V_Edificios ve = new V_Edificios();

            ve.Id = e.Id;
            ve.Nombre = e.Nombre;
            ve.Sede = null;
            ve.Id_Ciudad = 0;
            ve.Estado = null;
            ve.Id_Sede = e.Id_Sede;
            ve.Id_Empresa = 0;
            ve.Id_Estado = e.Id_Estado;

            return ve;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarUsuarioViewModel en un objeto de clase V_Usuarios
        /// </summary>
        /// <param name="edificiovm">CrearEditarUsuarioViewModel</param>
        public static explicit operator V_Edificios(CrearEditarEdificioViewModel edificiovm)
        {
            V_Edificios ve = new V_Edificios();

            ve.Id = edificiovm.Id;
            ve.Nombre = edificiovm.Nombre;
            ve.Sede = null;
            ve.Id_Ciudad = 0;
            ve.Estado = null;
            ve.Id_Sede = edificiovm.Id_Sede;
            ve.Id_Empresa = edificiovm.Id_Empresa;
            ve.Id_Estado = edificiovm.Id_Estado;

            return ve;
        }



    }

}