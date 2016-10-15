using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using SqlServerConnectionHelper;

namespace MantoxWebApp.Models
{
    public class MantoxSqlServerConnectionHelper : SqlServerConnectionHelper.SqlServerConnectionHelper
    {

        public MantoxSqlServerConnectionHelper(
            string connectionString = @"
                    Server=(local);
                    Database=inventarioHardware;
                    Integrated Security=SSPI;")
            : base(connectionString)
        {
            //Vacío
        }
    }
}