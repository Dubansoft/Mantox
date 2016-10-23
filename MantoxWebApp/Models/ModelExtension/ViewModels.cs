using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;

namespace MantoxWebApp.Models
{

    public class MantoxViewModel
    {
        /// <summary>
        /// Diccionario que devuelve estos índices: TablaResultados, TotalFilas, FilasPorPagina, PaginaActual y TotalPaginas. TotalFilas contiene los datos que se mostrarán en la página y TotalResultados es un entero que representa la cantidad total de registros encontrados en la base de datos para los criterios de búsqueda enviados. Este entero se usa para mostrar la cantidad de páginas y el total de resultados en el paginador de la tabla dinámica de los indexes de cada controlador.
        /// </summary>
        /// <param name="tabla">Tabla o vista que se usará para la consulta.</param>
        /// <param name="searchString">Texto buscado</param>
        /// <param name="idEmpresa">Id de la empresa actual, por defecto es la empresa del usuario actual.</param>
        /// <param name="sidx">Nombre de la columma de ordenamiento, por defecto ninguna.</param>
        /// <param name="sord">Orden de la búsqueda, por defecto ASC</param>
        /// <param name="page">Página actual del paginador</param>
        /// <param name="rows">Número de filas que se deben mostrar en la tabla</param>
        /// <param name="searchField">Nombre de la columma de búsqueda, por defecto ninguna.</param>
        /// <param name="filters">Cadena JSON con los filtros que se usarán para busquedas generales que involucrarán todas las columnas de la tabla.</param>
        /// <param name="filtrarPorEmpresa">Activa el filtrado de datos por  medio de la comuna Id_Empresa. Solo se debe activar si la tabla consultada tiene tal columna, de lo contrario, se generará un error de lectura en la base de datos.</param>
        /// <returns></returns>
        public Dictionary<string, object> ObtenerTablaVistaDinamica(string tabla, string searchString, int idEmpresa, string sidx, string sord, int page, int rows, string searchField, string filters, bool filtrarPorEmpresa = false)
        {
            //Creamos nueva instancia del ayudante de base de datos
            MantoxSqlServerConnectionHelper myMantoxSqlServerConnectionHelper = new MantoxSqlServerConnectionHelper();

            //Definimos las variables a usar en la consulta
            string nombreTabla = tabla;
            string conceptoBusqueda = (searchString == null) ? "" : searchString;
            string columnaBusqueda = (searchField == null) ? "" : searchField;
            int id_empresa = idEmpresa;
            string columnaOrdenamiento = (sidx == null) ? "" : sidx;
            string tipoOrdenamiento = (sord == null) ? "" : sord;
            string filtrosBusquedaGeneral = (filters == null) ? "" : filters;

            //Definimos entero para almacenar el conteo general de los resultados
            int totalFilas = 0;

            //Definimos entero para almacenar el numero de la página actual solicitada.
            int paginaActual = 0;

            //Creamos enteros para almacenar los diferentes valores requeridos por el paginador
            int filasPorPagina = 0;
            int totalPaginas = 0;

            //Definimos las variables a usar en el limitador de la consulta
            int desdeFila = 0;
            int hastaFila = 0;

            #region Conteo de Registros y Paginación

            //Segmento de query que permite retornar el conteo general de los resultados
            //con los criterios de busqueda enviados, se contruye con un StringBuilder
            StringBuilder queryConteoGeneral = new StringBuilder();

            //Añadimos el selector de tabla
            queryConteoGeneral.Append(
                @", * FROM " + nombreTabla + " "
            );

            //Si hay concepto de búsqueda Y columna específica de busqueda,
            //añadir cláusula WHERE con términos de busqueda
            if (conceptoBusqueda.Length > 0 && columnaBusqueda.Length > 0)
            {
                queryConteoGeneral.Append(
                    @" WHERE " + nombreTabla + "." + columnaBusqueda + @" LIKE '%" + conceptoBusqueda + @"%' "
                );
            }else if(
                //Si hay concepto de busqueda Y no hay columna especifica,
                //y hay filtros de busqueda general (en todas las columnas),
                //añadismos la clausua WHERE con los terminos de busqueda.
                conceptoBusqueda.Length == 0 && columnaBusqueda.Length == 0
                && filtrosBusquedaGeneral.Length > 0
                )
            {

                //Añadimos la cláusula WHERE a la query "queryConteoGeneral"
                queryConteoGeneral.Append(
                    @" WHERE "
                );

                //Creamos variable para formatear como array Json la matriz de busqueda enviada desde
                //el formulario
                string jsonString = "[" + filtrosBusquedaGeneral +"]";

                //Creamos nueva instancia del serializador
                JavaScriptSerializer serializadorBusquedaGeneral = new JavaScriptSerializer();

                //Creamos una lista de queries JSONFiltroBusquedaMulticolumna para almacenar los parámetros de búsqueda multicolumna
                List<JSONFiltroBusquedaMulticolumna>  listJsonQuery = (List<JSONFiltroBusquedaMulticolumna>)serializadorBusquedaGeneral.Deserialize(jsonString, typeof(List<JSONFiltroBusquedaMulticolumna>));

                //Por cada ítem de la lista anterior, iterar para construir la query "queryConteoGeneral"
                foreach (JSONFiltroBusquedaMulticolumna jsonFiltro in listJsonQuery)
                {
                    string st = jsonFiltro.searchString; //Concepto buscado
                    string go = jsonFiltro.groupOp; //Operador por defecto "OR"

                    //Creamos variable de tipo lista para almacenar las reglas de búsqueda.
                    List<JSONReglaBusquedaMulticolumna> rules = jsonFiltro.rules; //Lista de reglas

                    //Por cada regla de búsqueda, iterar para añadir condiciones a la query "queryConteoGeneral"
                    foreach (JSONReglaBusquedaMulticolumna reglaBusqueda in rules)
                    {
                        conceptoBusqueda = reglaBusqueda.data;
                        columnaBusqueda = reglaBusqueda.field;
                        string operador = reglaBusqueda.op;

                        //Añadimos las condiciones al query "queryConteoGeneral". Nótese como se añade LIKE y OR
                        queryConteoGeneral.Append(
                            @"" + nombreTabla + "." + columnaBusqueda + @" LIKE '%" + conceptoBusqueda + @"%' OR "
                        );
                    }
                    //Eliminar el último OR
                    queryConteoGeneral = queryConteoGeneral.Remove(queryConteoGeneral.ToString().Length - 3, 3);
                }
            }

            //Si está activado el filtrado de empresas, se añade la cláusula al query
            if (filtrarPorEmpresa)
            {
                //Almacenar id de la empresa del usuario actual en variable int
                int idEmpresaDeUsuarioActual = (int)System.Web.HttpContext.Current.Session["Id_Empresa"];

                //Validamos que query no tenga no tenga una búsqueda activa que condicione
                //los resultados. Si la hay, la query ya tiene un "WHERE",
                //por tanto, solo se añade una clausula adicional empezando con AND
                if (queryConteoGeneral.ToString().Contains("WHERE"))
                {
                    //Añadir las condiciones al query
                    queryConteoGeneral.Append(
                                @" AND Id_Empresa = " + idEmpresaDeUsuarioActual.ToString() + ""
                            );
                }else
                {
                    //Si lo anterior es falso, se añade la condicion completa, empezando con "WHERE"
                    queryConteoGeneral.Append(
                                @" WHERE Id_Empresa = " + idEmpresaDeUsuarioActual.ToString() + ""
                            );
                }



            }

            //Contamos las filas y añadimos el valor a totalFilas por medio de la consulta a la bd.
            totalFilas = myMantoxSqlServerConnectionHelper.ObtenerConteo(queryConteoGeneral.ToString().Replace(", *", "SELECT COUNT(*) "), CommandType.Text, null);

            //Asignamos el valor a las variables haciendo los cálculos adecuados
            filasPorPagina = (rows <= 0) ? 10 : rows;
            totalPaginas = ((int)Math.Ceiling((double)totalFilas / filasPorPagina)) > 0 ? (int)Math.Ceiling((double)totalFilas / filasPorPagina) : 1;

            //Añadimos lógica para validar si es posible
            //mostrar la página solicitada de acuerdo al total de filas encontradas
            if (page <= 0) {
                //Si la pagina solicitada no fue definida, se asigna 1 a paginaActual
                paginaActual = 1;
            }else if (page > totalPaginas)
            {
                //Si la página solicitada es mayor a la cantidad total de ppáginas se asigna el total de paginas a
                //página actual
                paginaActual = totalPaginas;
            }else
            {
                //Si no se cumplen estas condiciones, se asigna el valor por defecto
                paginaActual = page;
            }

            //Asignamos el valor de las variables a usar en el limitador de la consulta
            desdeFila = ((filasPorPagina * paginaActual) - filasPorPagina) + 1;
            hastaFila = filasPorPagina * paginaActual;

            //Segmento de query que añade una nueva columna con los números de fila a la tabla de resultados
            string columnaDeNumerosDeFilaQuery = " SELECT ROW_NUMBER() OVER(ORDER BY(SELECT NULL AS NOORDER)) AS NumeroFila ";

            #endregion

            #region Consulta de datos para mostrar en la tabla

            //Segmento de query que contiene la consulta reducida a la cantidad de filas solicitada por páginanada, es decir, los resultados en la cantidad requerida
            //Se construye un StringBuilder
            StringBuilder paginaActualQuery = new StringBuilder();

            //Añadimos el SELECT compuesto
            paginaActualQuery.Append(
                @"SELECT * 
                FROM(" + columnaDeNumerosDeFilaQuery + queryConteoGeneral.ToString() + @") as TablaResultados
                WHERE NumeroFila BETWEEN " + desdeFila + @" AND " + hastaFila
                );


            //Creamos nuevo dataset para los resultados de la consulta
            DataSet resultadosDataset = new DataSet();

            //Asignamos el valor a resultadosDataset por medio de la consulta a la bd.
            resultadosDataset = myMantoxSqlServerConnectionHelper.ConsultarQuery(paginaActualQuery.ToString(), CommandType.Text, null);

            //Creamos datatable para almacenar la tabla de resultados
            DataTable UsuariosDt = new DataTable();

            //Asignamos el valor a UsuariosDt tomando la primera tabla del Dataset
            UsuariosDt = resultadosDataset.Tables[0];

            #endregion

            #region Creación del diccionario de resultados

            //Creamos nuevo diccionario para devolver como resultado
            Dictionary<string, object> diccionarioTablaVistaDinamica = new Dictionary<string, object>();

            //Asignamos las entradas al diccionario.
            diccionarioTablaVistaDinamica.Add("TablaResultados", UsuariosDt);
            diccionarioTablaVistaDinamica.Add("TotalFilas", totalFilas);
            diccionarioTablaVistaDinamica.Add("FilasPorPagina", filasPorPagina);
            diccionarioTablaVistaDinamica.Add("PaginaActual", paginaActual);
            diccionarioTablaVistaDinamica.Add("TotalPaginas", totalPaginas);

            #endregion

            //Devolvemos el diccionario
            return diccionarioTablaVistaDinamica;
        }

    }

}
