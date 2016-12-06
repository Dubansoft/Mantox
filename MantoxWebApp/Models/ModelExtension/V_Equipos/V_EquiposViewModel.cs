using System.Collections.Generic;
using System.Web;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Equipos". Contiene métodos encargados de traer información desde la vista "V_Equipos" de la base de datos.
    /// </summary>
    public partial class V_Equipos : MantoxViewModel
    {
        /// <summary>
        /// Diccionario AUXILIAR que devuelve dos índices: TablaResultados y TotalResultados. TablaResultados contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// NOTA: El diccionario principal es devuelto por la función ObtenerTablaVistaDinamica() la cual es llamada por BuscarEquipos()
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
        public Dictionary<string,object> BuscarEquipos(string searchString, int idEmpresa, string sidx, string sord, int page, int rows, string searchField, string filters)
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
            return ObtenerTablaVistaDinamica("V_Equipos", searchString, idEmpresa, sidx, sord, page, rows, searchField, filters, filtrarPorEmpresa);
        }

        /// <summary>
        /// Convierte un objeto de clase Equipo en un objeto de clase V_Equipos
        /// </summary>
        /// <param name="e">Equipo</param>
        public static explicit operator V_Equipos(Equipo e)
        {
            V_Equipos ve = new V_Equipos
            {
                Id = e.Id,
                Activo = e.Activo,
                Serial = e.Serial,
                Nombre_de_Equipo = null,
                Ip = e.Ip,
                Fecha_de_Ingreso = new System.DateTime(1900,01,01),
                Fecha_Fin_de_Garantia = new System.DateTime(1901, 01, 01),
                Comentario = e.Comentario,
                Nombre_de_Responsable = null,
                Apellido_de_Responsable = null,
                Email_de_Responsable = null,
                Edificio_de_Responsable = null,
                Sede_de_Responsable = null,
                Empresa_de_Responsable = null,
                Area = null,
                Ciudad = null,
                Departamento = null,
                Modelo = null,
                Sietema_Operativo = null,
                Propietario = null,
                Version_de_Office = null,
                Estado = null,
                Empresa = null,
                Sede = null,
                Edificio = null,
                Piso = null,
                Area_de_Responsable = null,
                Id_Tipo_Equipo = null,
                Id_Marca = null,
                Id_Modelo = e.Id_Modelo,
                Id_Sistemas_Operativo = e.Id_Sistemas_Operativo,
                Id_Version_Office = e.Id_Version_Office,
                Id_Area = e.Id_Area,
                Id_Responsable = e.Id_Responsable,
                Id_Propietario = e.Id_Propietario,
                Id_Estado = e.Id_Estado
            };

            return ve;

        }

        /// <summary>
        /// Convierte un objeto de clase CrearEditarEquipoViewModel en un objeto de clase V_Equipos
        /// </summary>
        /// <param name="equipovm">CrearEditarEquipoViewModel</param>
        public static explicit operator V_Equipos(CrearEditarEquipoViewModel equipovm)
        {
            V_Equipos ve = new V_Equipos
            {
                Id = equipovm.Id,
                Activo = equipovm.Activo,
                Serial = equipovm.Serial,
                Nombre_de_Equipo = equipovm.Nombre_Equipo,
                Ip = equipovm.Ip,
                Fecha_de_Ingreso = equipovm.Fecha_Ingreso,
                Fecha_Fin_de_Garantia = equipovm.Fecha_Fin_Garantia,
                Comentario = equipovm.Comentario,
                Nombre_de_Responsable = null,
                Apellido_de_Responsable = null,
                Email_de_Responsable = null,
                Edificio_de_Responsable = null,
                Sede_de_Responsable = null,
                Empresa_de_Responsable = null,
                Area = null,
                Ciudad = null,
                Departamento = null,
                Modelo = null,
                Sietema_Operativo = null,
                Propietario = null,
                Version_de_Office = null,
                Estado = null,
                Empresa = null,
                Sede = null,
                Edificio = null,
                Piso = null,
                Area_de_Responsable = null,
                Id_Tipo_Equipo = null,
                Id_Marca = null,
                Id_Modelo = equipovm.Id_Modelo,
                Id_Sistemas_Operativo = equipovm.Id_Sistema_Operativo,
                Id_Version_Office = equipovm.Id_Version_Office,
                Id_Area = equipovm.Id_Area,
                Id_Responsable = equipovm.Id_Responsable,
                Id_Propietario = equipovm.Id_Propietario,
                Id_Estado = equipovm.Id_Estado
            };

            return ve;
        }



    }

}