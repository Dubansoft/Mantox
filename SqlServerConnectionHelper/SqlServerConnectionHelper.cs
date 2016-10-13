//@*
//The MIT License(MIT)
//Copyright(c) 2016
//Jhorman Rodríguez,
//Yeison Mosquera

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// *@

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using FileHelper;

/// <summary>
/// Espacio de nombres del ayudante de conexión a base de datos MS SQL Server
/// </summary>
namespace SqlServerConnectionHelper
{

    /// <summary>
    /// AdministradorDeConexiones se encarga de crear conexiones a la base de datos y entregarlas abiertas
    /// a los procedimientos o rutinas que las han solicitado. Los procedimientos de lectura deberán
    /// cerrar estas conexiones luego de finalizar la consulta o luego de haber manejado los errores.
    /// </summary>
    public class AdministradorDeConexiones
    {
        /// <summary>
        /// Cadena de conexión
        /// </summary>
        private static string cmConnectionString = string.Empty;
        public static string CmConnectionString
        {
            get
            {
                return cmConnectionString;
            }

            set
            {
                cmConnectionString = value;
            }
        }

        public static SqlConnection ObtenerConexionSql(string connectionString)
        {
            try
            {
                CmConnectionString = connectionString;
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                string[] eventSender = new string[] { "ObtenerConexionSql" };

                EventLogger.LogEvent(eventSender, e.Message.ToString(), e);

                return null;
            }
        }
    }
    /// <summary>
    /// Esta clase contiene métodos de apoyo para conectividad a base de datos de MS SQL SERVER
    /// Se debe inicializar con una extensión de clase como la siguiente:
    /// <example>
    /// <code>
    /// public class MantoxSqlServerConnectionHelper : global::SqlServerConnectionHelper.SqlServerConnectionHelper
    /// {
    ///
    ///    public MantoxSqlServerConnectionHelper(string connectionString = 
    ///         @"Server=SERVIDOR\INSTANCIA;
    ///          Database=NOMBRE_BASE_DATOS;
    ///         User Id=NOMBRE_USUARIO;
    ///         Password=CONTRASEÑA;"
    ///         ) 
    ///         : base(connectionString)
    ///    {}
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class SqlServerConnectionHelper
    {
        /// <summary>
        /// La cadena de conexión, por defecto vacía.
        /// </summary>
        private string connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }

            set
            {
                connectionString = value;
            }
        }

        /// <summary>
        /// Inicializador principal, es obligatorio pasar la cadena completa de conexión.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión. Ejemplo: @"Server=SERVIDOR\INSTANCIA;Database=NOMBRE_BASE_DATOS;User Id=NOMBRE_USUARIO;Password=CONTRASEÑA;";</param>
        public SqlServerConnectionHelper(string connectionString)
        {
            try
            {
                ConnectionString = connectionString;
            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
            }
        }

        /// <summary>
        /// EjecutarNonQuery() ejecuta una consulta de tipo INSERT, DELETE o UPDATE en la base de datos
        /// pero no devuelve datos. Devuelve el número de filas afectadas, sea el número de filas insertadas,
        /// eliminadas o actualizadas.
        /// </summary>
        /// <param name="commandText">Texto del comando, puede ser el nombre de un procedimiento almacenado o una cadena tipo SELECT, UPDATE, DELETE, etc.</param>
        /// <param name="commandType">Tipo de comando. Opciones validas son CommandType.Text y CommandType.StoredProcedure</param>
        /// <param name="commandParameters">Si commandType es CommandType.StoredProcedure, debe contener los parámetros del procedimiento en una matriz así: SqlParameter[] myParams = new SqlParameter[] { new SqlParameter("@nombreParametro", "valor") }</param>
        /// <returns></returns>
        public int EjecutarNonQuery(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] commandParameters)
        {
            using (var connection = AdministradorDeConexiones.ObtenerConexionSql(connectionString))
            {
                try
                {
                    int filasAfectadas = 0;
                    {
                        using (var command = new SqlCommand(commandText, connection))
                        {
                            command.CommandType = commandType;
                            command.Parameters.AddRange(commandParameters);
                            filasAfectadas = command.ExecuteNonQuery();
                        }
                    }
                    return filasAfectadas;
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(this, e.Message.ToString(), e);
                    return 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            
        }

        /// <summary>
        /// ObtenerConteo() ejecuta una consulta tipo SELECT en la base de datos
        /// pero no devuelve datos. Devuelve un entero (INT) con el número de filas encontradas.
        /// </summary>
        /// <param name="commandText">Texto del comando, puede ser el nombre de un procedimiento almacenado o una cadena tipo SELECT.</param>
        /// <param name="commandType">Tipo de comando. Opciones validas son CommandType.Text y CommandType.StoredProcedure</param>
        /// <param name="commandParameters">Si commandType es CommandType.StoredProcedure, debe contener los parámetros del procedimiento en una matriz así: SqlParameter[] myParams = new SqlParameter[] { new SqlParameter("@nombreParametro", "valor") }</param>
        /// <returns></returns>
        public int ObtenerConteo(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] commandParameters)
        {
            using (var connection = AdministradorDeConexiones.ObtenerConexionSql(connectionString))
                try
                {
                    int filasContadas = 0;
                    {
                        using (var command = new SqlCommand(commandText, connection))
                        {
                            command.CommandType = commandType;

                            if(command.CommandType == CommandType.StoredProcedure)
                            {
                                command.Parameters.AddRange(commandParameters);
                            }
                            command.CommandText = commandText;

                            string result = command.ExecuteScalar().ToString();

                            filasContadas = Convert.ToInt32(result);

                        }
                    }
                    return filasContadas;
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(this, e.Message.ToString(), e);
                    return 0;
                }
                finally
                {
                    try{
                        connection.Close();
                    }
                    catch (Exception){
                    }
                }
        }

        /// <summary>
        /// ConsultarQuery() ejecuta una consulta tipo SELECT en la base de datos
        /// y devuelve los resultados en forma de DataTable.
        /// </summary>
        /// <param name="commandText">Texto del comando, puede ser el nombre de un procedimiento almacenado o una cadena tipo SELECT.</param>
        /// <param name="commandType">Tipo de comando. Opciones validas son CommandType.Text y CommandType.StoredProcedure</param>
        /// <param name="commandParameters">Si commandType es CommandType.StoredProcedure, debe contener los parámetros del procedimiento en una matriz así: SqlParameter[] myParams = new SqlParameter[] { new SqlParameter("@nombreParametro", "valor") }</param>
        /// <returns></returns>
        public DataSet ConsultarQuery(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
        {
            using (var connection = AdministradorDeConexiones.ObtenerConexionSql(connectionString))
                try
                {
                 using (var command = new SqlCommand(commandText, connection))
                    {
                        DataSet ds = new DataSet();
                        command.CommandType = commandType;

                        if(command.CommandType == CommandType.StoredProcedure)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                    
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        connection.Close();
                        return ds;
                    }
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(this, e.Message.ToString(), e);
                    return new DataSet("ERROR");
                }
                finally
                {
                    connection.Close();
                }
        }
    }


}