using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MantoxWebApp.Models
{
    public class JSONFiltroBusquedaMulticolumna
    {
        public string searchString { get; set; }
        public string groupOp { get; set; }
        public List<JSONReglaBusquedaMulticolumna> rules { get; set; }
    }

    public class JSONReglaBusquedaMulticolumna
    {
        public string field { get; set; }
        public string op { get; set; }
        public string data { get; set; }
    }
}