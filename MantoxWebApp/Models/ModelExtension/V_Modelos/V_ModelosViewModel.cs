using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Modelos". Contiene métodos encargados de traer información desde la vista "V_Modelos" de la base de datos.
    /// </summary>
    public partial class V_Modelos : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarModelos()
        /// </summary>
        /// <param name="searchString">Términos de búsqued</param>
        /// <param name="rows">Numero de filas</param>
        /// <param name="page">Página</param>
        /// <param name="idModelo">Id de la modelo</param>
        /// <param name="sidx">Columna de ordenamiento</param>
        /// <param name="sord">Tipo de ordenamiento, puede ser ASC o DESC</param>
        /// <param name="searchField">Columna de búsqueda</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <returns>Dictionary de string,object</returns>
        public Dictionary<string,object> BuscarModelos(string searchString, int idModelo, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            
            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Modelos", searchString, idModelo, sidx, sord, page, rows, searchField, filters);
        }

        /// <summary>
        /// Convierte un objeto de clase Modelo en un objeto de clase V_Modelos
        /// </summary>
        /// <param name="a">Modelos</param>
        public static explicit operator V_Modelos(Modelo a)
        {
            V_Modelos vm = new V_Modelos();

            vm.Id = a.Id;
            vm.Nombre = a.Nombre;
            vm.Id_Marca = a.Id_Marca;
            vm.Id_Tipo_Equipo = a.Id_Tipo_Equipo;
            vm.Id_Estado = a.Id_Estado;

            return vm;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarModeloViewModel en un objeto de clase V_Modelo
        /// </summary>
        /// <param name="modelovm">CrearEditarModeloViewModel</param>
        public static explicit operator V_Modelos(CrearEditarModeloViewModel modelovm)
        {
            V_Modelos va = new V_Modelos();

            va.Id = modelovm.Id;
            va.Nombre = modelovm.Nombre;
            va.Id_Marca = modelovm.Id_Marca;
            //va.Marca = modelovm.Marca;
            va.Id_Tipo_Equipo = modelovm.Id_Tipo_Equipo;
            va.Id_Estado = modelovm.Id_Estado;

            return va;
        }



    }

}