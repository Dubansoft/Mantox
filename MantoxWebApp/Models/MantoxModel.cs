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
        public bool Duplicado(string tableName, Dictionary<string, string> criteria)
        {
            MantoxSqlServerConnectionHelper sqlHelper = new MantoxSqlServerConnectionHelper();

            StringBuilder query = new StringBuilder();
                query.Append ("SELECT COUNT(*) FROM " + tableName + " WHERE ");

            foreach (KeyValuePair<string, string> entry in criteria)
            {
                query.Append(tableName + "." + entry.Key + "='" + entry.Value + "' AND ");
            }

            //foreach (object _condition in criteria)
            //{
            //    string[] condition = (string[])_condition;
            //    query.Append(tableName + "." + condition[0] + "='" + condition[1] + "' AND ");

            //}

            query.Remove(query.Length - 4, 4);

            int rowCount = sqlHelper.ObtenerConteo(query.ToString(), System.Data.CommandType.Text, null);

            if(rowCount > 0)
            {
                return true;
            }

            return false;
        }
    }
}