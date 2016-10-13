using MantoxWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MantoxWebApp.Controllers
{
    public class PisoController : MantoxController
    {
        private MantoxDBEntities bdMantox = new MantoxDBEntities();

        /// <summary>
        /// Devuelve un PartialView que contiene un MultiSelect de los Pisos filtrados por un id de edificio
        /// </summary>
        /// <param name="idEdificio">Id del edificio</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarPisos(string idEdificio = "1")
        {
            var id = int.Parse(idEdificio);

            List<Piso> pisos = bdMantox.Areas.Where(s => s.Id_Edificio == id)
                .Select(t => new Piso
                {
                    NumeroPiso = t.Piso
                }
            ).ToList();

            pisos = pisos.GroupBy(test => test.NumeroPiso).Select(group => group.First()).ToList();

            ViewBag.Pisos = new MultiSelectList(pisos, "NumeroPiso", "NumeroPiso");

            return PartialView("_VistaParcial_FiltrarPisos");
        }
    }

}