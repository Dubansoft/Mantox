using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Licencias". Contiene métodos encargados de traer información desde la vista "V_Licencias" de la base de datos.
    /// </summary>
    public partial class V_Licencias : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarLicencias()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idLicencia">Id de la Licencia</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarLicencias(string searchString, int idLicencia, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorLicencia = false;

            //El filtrado por licencia NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                        //No se añaden restricciones a los licencias que puede ver el desarrollador
                        break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                         filtrarPorLicencia = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Licencias", searchString, idLicencia, sidx, sord, page, rows, searchField, filters, filtrarPorLicencia);
        }

        /// <summary>
        /// Convierte un objeto de clase Licencia en un objeto de clase V_Licencia
        /// </summary>
        /// <param name="e">Licencia</param>
        public static explicit operator V_Licencias(Licencia e)
        {
            V_Licencias vl = new V_Licencias();

            vl.Id = e.Id;
            vl.Serial = e.Serial;
            vl.Serial_Equipo = null;
            vl.Fecha_Compra = e.Fecha_Compra;
            vl.Nombre_de_Responsable = null;
            vl.Apellido_de_Responsable = null;
            vl.Serial_Equipo = null;
            vl.Nombre_de_Equipo = null;
            vl.Empresa =null;
            vl.Sede =  null;
            vl.Edificio = null;
            vl.Area = null;
            vl.Piso = null;
            vl.Ip = null;
            vl.Id_Tipo_Licencia = e.Id_Tipo_Licencia;
            vl.Id_Equipo = e.Id_Equipo;

            return vl;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarLicenciaViewModel en un objeto de clase V_Licencias
        /// </summary>
        /// <param name="licenciavm">CrearEditarLicenciaViewModel</param>
        public static explicit operator V_Licencias(CrearEditarLicenciaViewModel licenciavm)
        {
            V_Licencias vl = new V_Licencias();

            vl.Id = licenciavm.Id;
            vl.Serial = licenciavm.Serial;
            vl.Serial_Equipo = null;
            vl.Fecha_Compra = licenciavm.Fecha_Compra;
            vl.Nombre_de_Responsable = null;
            vl.Apellido_de_Responsable = null;
            vl.Serial_Equipo = null;
            vl.Nombre_de_Equipo = null;
            vl.Empresa = null;
            vl.Sede = null;
            vl.Edificio = null;
            vl.Area = null;
            vl.Piso = null;
            vl.Ip = null;
            vl.Id_Tipo_Licencia = licenciavm.Id_Tipo_Licencia;
            vl.Id_Equipo = licenciavm.Id_Equipo;

            return vl;
        }



    }

}