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
                    Server=PC-1803\mantox;
                    Database=inventarioHardware;
                    User Id=sa;
                    Password=CES5767366;")
            : base(connectionString)
        {
            //Vacío
        }
    }
}