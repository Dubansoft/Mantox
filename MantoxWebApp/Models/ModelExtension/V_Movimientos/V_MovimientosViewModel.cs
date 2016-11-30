using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static MantoxWebApp.Controllers.MantoxController;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Movimientos". Contiene métodos encargados de traer información desde la vista "V_Movimientos" de la base de datos.
    /// </summary>
    public partial class V_Movimientos : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarMovimientos()
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
        public Dictionary<string,object> BuscarMovimientos(string searchString, int idEmpresa, string sidx, string sord, int page, int rows, string searchField, string filters)
        {

            ////El filtrado por empresa NO debe estar activado para usuarios no desarrolladores:
            //switch ((RolDeUsuario)HttpContext.Current.Session["Id_Rol"])
            //{
            //    case RolDeUsuario.Desarrollador:
            //        //No se añaden restricciones a las empresas que puede ver el desarrollador
            //        break;
            //    case RolDeUsuario.Administrador:
            //    case RolDeUsuario.Reportes:
            //    default:
            //        filtrarPorEmpresa = true;
            //        break;
            //}

            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Movimientos", searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);
        }

        /// <summary>
        /// Convierte un objeto de clase Movimiento en un objeto de clase V_Movimientos
        /// </summary>
        /// <param name="a">Movimiento</param>
        public static explicit operator V_Movimientos(Movimiento a)
        {
            V_Movimientos vm = new V_Movimientos();

            vm.Id = a.Id;
            vm.Usuario = null;
            vm.RazonMovimiento = null;
            vm.Fecha = a.Fecha;
            vm.Nombre_Equipo = null;
            vm.Area = null;
            vm.Id_Usuario = a.Id_Usuario;
            vm.Id_Equipo = a.Id_Equipo;
            vm.Id_Razon_Movimiento = a.Id_Razon_Movimiento;
            vm.Id_Area_Origen = a.Id_Area_Origen;
            vm.Id_Area_Destino = a.Id_Area_Destino;

            return vm;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarMovimientoViewModel en un objeto de clase V_Movimientos
        /// </summary>
        /// <param name="movimientovm">CrearEditarMovimientoViewModel</param>
        public static explicit operator V_Movimientos(CrearEditarMovimientoViewModel movimientovm)
        {
            V_Movimientos vm = new V_Movimientos();

            vm.Id = movimientovm.Id;
            vm.Usuario = null;
            vm.RazonMovimiento = null;
            vm.Fecha = movimientovm.Fecha;
            vm.Nombre_Equipo = null;
            vm.Area = null;
            vm.Id_Usuario = movimientovm.Id_Usuario;
            vm.Id_Equipo = movimientovm.Id_Equipo;
            vm.Id_Razon_Movimiento = movimientovm.Id_Razon_Movimiento;
            vm.Id_Area_Origen = movimientovm.Id_Area_Origen;
            vm.Id_Area_Destino = movimientovm.Id_Area_Destino;

            return vm;
        }



    }

}