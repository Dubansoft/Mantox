using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Marcas". Contiene métodos encargados de traer información desde la vista "V_Marcas" de la base de datos.
    /// </summary>
    public partial class V_Marcas : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarMarcas()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idmarca">Id de la marca</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarMarcas(string searchString, int idMarca, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Definimos variable para almacener el True o el False que activará o no el filtrado
            bool filtrarPorMarca = false;

            //El filtrado por marca NO debe estar activado para usuarios no desarrolladores:
            switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
                {
                    case RolDeUsuario.Desarrollador:
                        //No se añaden restricciones a las marcas que puede ver el desarrollador
                        break;
                    case RolDeUsuario.Administrador:
                    case RolDeUsuario.Reportes:
                    default:
                        filtrarPorMarca = true;
                        break;
                }

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Marcas", searchString, idMarca, sidx, sord, page, rows, searchField, filters, filtrarPorMarca);
        }

        /// <summary>
        /// Convierte un objeto de clase Marca en un objeto de clase V_Marcas
        /// </summary>
        /// <param name="m">Marca</param>
        public static explicit operator V_Marcas(Marca m)
        {
            V_Marcas vm = new V_Marcas();

            vm.Id = m.Id;
            vm.Nombre = m.Nombre;
            vm.Id_Estado = m.Id_Estado;

            return vm;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarMarcaViewModel en un objeto de clase V_Marcas
        /// </summary>
        /// <param name="marcavm">CrearEditarMarcaViewModel</param>
        public static explicit operator V_Marcas(CrearEditarMarcaViewModel marcavm)
        {
            V_Marcas vm = new V_Marcas();

            vm.Id = marcavm.Id;
            vm.Nombre = marcavm.Nombre;
            vm.Id_Estado = marcavm.Id_Estado;

            return vm;
        }



    }

}