using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MantoxWebApp.Models.Helpers
{

    class torta
    {
        private string alino1;
        public string Alino1
        {
            get
            {
                return alino1;
            }

            set
            {
                alino1 = value;
            }
        }

        public string Alino2
        {
            get
            {
                return alino2;
            }

            set
            {
                alino2 = value;
            }
        }
        private string alino2;

        public torta crearTorta()
        {
            torta miTorta = new torta();

            miTorta.Alino1 = "cebolla";
            miTorta.Alino2 = "tomate";



            return miTorta;

        }
    }

    class torta20dejulio : torta
    {
        private string color;

        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public torta20dejulio crearTorta()
        {
            torta20dejulio miTortade20julio = new torta20dejulio();

            miTortade20julio.Alino1 = "cebolla";
            miTortade20julio.Alino2 = "tomate";
            miTortade20julio.Color = "amarilla";




            return miTortade20julio;

        }
    }
}