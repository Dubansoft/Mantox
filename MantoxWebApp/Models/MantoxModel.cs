using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MantoxWebApp.Models.Helpers;
using System.Text;

namespace MantoxWebApp.Models
{
    public class MantoxModel
    {
        public bool Duplicado(string tableName, string[,] criteria)
        {
            MantoxSqlServerConnectionHelper sqlHelper = new MantoxSqlServerConnectionHelper();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT COUNT(*) FROM " + tableName + " WHERE ");


            for (int i = 0; i < (criteria.Length/2); i++)
            {
                string columnName = criteria[i,0].ToString();
                string columnValue = criteria[i, 1].ToString();

                query.Append(tableName + "." + columnName + "='" + columnValue + "' AND ");

            }
            
            query.Remove(query.Length - 4, 4);

            int rowCount = sqlHelper.ObtenerConteo(query.ToString(), System.Data.CommandType.Text, null);

            if (rowCount > 0)
            {
                return true;
            }

            return false;
        }
    }
}