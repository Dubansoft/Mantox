using FileHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MantoxWebApp.Models
{
    /// <summary>
    /// Modelo parcial para la vista "V_Usuarios". Contiene métodos encargados de traer información desde la vista "V_Usuarios" de la base de datos.
    /// </summary>
    public partial class V_Usuarios : MantoxViewModel
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
        public Dictionary<string,object> BuscarUsuarios(string searchString, int idEmpresa, string sidx, string sord, int page, int rows, string searchField, string filters)
        {
            //Devolvemos el resultado de la consulta genérica ObtenerTablaVistaDinamica
            return ObtenerTablaVistaDinamica("V_Usuarios", searchString, idEmpresa, sidx, sord, page, rows, searchField, filters);
        }

        /// <summary>
        /// Convierte un objeto de clase Usuario en un objeto de clase V_Usuarios
        /// </summary>
        /// <param name="u">Usuario</param>
        public static explicit operator V_Usuarios(Usuario u)
        {
            V_Usuarios vu = new V_Usuarios();

            vu.Id = u.Id;
            vu.Nombre = u.Nombre;
            vu.Apellido = u.Apellido;
            vu.Email = u.Email;
            vu.Contrasena = u.Contrasena;
            vu.Estado = null;
            vu.Rol = null;
            vu.Area = null;
            vu.Edificio = null;
            vu.Sede = null;
            vu.Empresa = null;
            vu.Ciudad = null;
            vu.Departamento = null;
            vu.Id_Empresa = 0;
            vu.Id_Sede = 0;
            vu.Id_Edificio = 0;
            vu.Id_Estado = u.Id_Estado;
            vu.Id_Area = u.Id_Area;
            vu.Id_Rol = u.Id_Rol;
            vu.Piso = null;

            return vu;

        }

        /// <summary>
        /// Convierte un objeto de clase UsuarioViewModel en un objeto de clase V_Usuarios
        /// </summary>
        /// <param name="usuariovm">UsuarioViewModel</param>
        public static explicit operator V_Usuarios(UsuarioViewModel usuariovm)
        {
            V_Usuarios vu = new V_Usuarios();

            vu.Id = usuariovm.Id;
            vu.Nombre = usuariovm.Nombre;
            vu.Apellido = usuariovm.Apellido;
            vu.Email = usuariovm.Email;
            vu.Contrasena = usuariovm.Contrasena;
            vu.Estado = null;
            vu.Rol = null;
            vu.Area = null;
            vu.Edificio = null;
            vu.Sede = null;
            vu.Empresa = null;
            vu.Ciudad = null;
            vu.Departamento = null;
            vu.Id_Empresa = usuariovm.Id_Empresa;
            vu.Id_Sede = usuariovm.Id_Sede;
            vu.Id_Edificio = usuariovm.Id_Edificio;
            vu.Id_Estado = usuariovm.Id_Estado;
            vu.Id_Area = usuariovm.Id_Area;
            vu.Id_Rol = usuariovm.Id_Rol;
            vu.Piso = usuariovm.Piso;

            return vu;
        }


        
    }

}