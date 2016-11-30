using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Sedes". Contiene métodos encargados de traer información desde la vista "V_Sedes" de la base de datos.
    /// </summary>
    public partial class V_Sedes : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarUsuarios()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarSedes(string searchString, int idEmpresa, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorEmpresa = false;

            //El filtrado por empresa NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                        //No se añaden restricciones a las empresas que puede ver el desarrollador
                        break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                        filtrarPorEmpresa = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Sedes", searchString, idEmpresa, sidx, sord, page, rows, searchField, filters, filtrarPorEmpresa);
        }

        /// <summary>
        /// Convierte un objeto de clase Usuario en un objeto de clase V_Sedes
        /// </summary>
        /// <param name="s">Sede</param>
        public static explicit operator V_Sedes(Sede s)
        {
            V_Sedes vs = new V_Sedes();
            
            vs.Id = s.Id;
            vs.Nombre = s.Nombre;
            vs.Empresa = null;
            vs.Pais = null;
            vs.Departamento = null;
            vs.Ciudad = null;
            vs.Estado = null;
            vs.Id_Empresa = 0;
            vs.Id_Pais = 0;
            vs.Id_Departamento =0;
            vs.Id_Ciudad = 0;
            vs.Id_Estado = 0;

            return vs;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarUsuarioViewModel en un objeto de clase V_Usuarios
        /// </summary>
        /// <param name="areavm">CrearEditarUsuarioViewModel</param>
        public static explicit operator V_Sedes(CrearEditarSedeViewModel areavm)
        {
            V_Sedes vs = new V_Sedes();
           

            vs.Id = areavm.Id;
            vs.Nombre = areavm.Nombre;
            vs.Empresa = null;
            vs.Pais = null;
            vs.Departamento = null;
            vs.Ciudad = null;
            vs.Estado = null;
            vs.Id_Empresa = 0;
            vs.Id_Pais = 0;
            vs.Id_Departamento = 0;
            vs.Id_Ciudad = 0;
            vs.Id_Estado = 0;

            return vs;
        }



    }

}