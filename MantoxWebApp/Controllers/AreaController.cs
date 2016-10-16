using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MantoxWebApp.Models;

namespace MantoxWebApp.Controllers
{
    public class AreaController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Areas";
        public string NombreObjeto = "Area";

        // GET: Area
        public async Task<ActionResult> Index()
        {
            return VistaAutenticada(View(await bdMantox.Areas.ToListAsync()), RoleDeUsuario.Reportes);
        }

        // GET: Area/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = await bdMantox.Areas.FindAsync(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Area/Create
        public ActionResult Create()
        {
            //Select para Empresas
            var empresas = bdMantox.Empresas.Select(empresa => new
            {
                EmpresaId = empresa.Id,
                EmpresaNombre = empresa.Nombre
            }).ToList();

            ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

            //Select para Edificio
            var edificios = bdMantox.Edificios.Select(edificio => new
            {
                EdificioId = edificio.Id,
                EdificioNombre = edificio.Nombre
            }).ToList();
            ViewBag.Edificios = new MultiSelectList(edificios, "EdificioId", "EdificioNombre");

            ////Select para Estados
            var estados = bdMantox.Estados.Select(estado => new
            {
                EstadoId = estado.Id,
                EstadoNombre = estado.Nombre
            }).ToList();

            //Eliminar los elementos que no se requieren en la vista

            estados.RemoveRange(2, 5);

            ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");


            ViewBag.Titulo = "Crear area";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View();
        }

        // POST: Area/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Piso,Id_Edificio,Id_Estado")] Area area)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Areas.Add(area);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Titulo = "Crear area";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View(area);
        }

        // GET: Area/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = await bdMantox.Areas.FindAsync(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Area/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Piso,Id_Edificio,Id_Estado")] Area area)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(area).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(area);
        }

        // GET: Area/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = await bdMantox.Areas.FindAsync(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Area area = await bdMantox.Areas.FindAsync(id);
            bdMantox.Areas.Remove(area);
            await bdMantox.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bdMantox.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Devuelve un PartialView que contiene un MultiSelect de las Áreas filtradas por un id de edificio o un piso de edificio
        /// </summary>
        /// <param name="idEdificio">Id del edificio</param>
        /// <param name="numPiso">Numero del piso</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarAreas(string idEdificio = "1",  string numPiso = "0")
        {
            var id_edificio = int.Parse(idEdificio);
            var num_piso = int.Parse(numPiso);

            List<Area> areas = (from a in bdMantox.Areas
                               where a.Id_Edificio  == id_edificio
                               where a.Piso == num_piso
                               select a).ToList();

            ViewBag.Areas = new MultiSelectList(areas, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarAreas");

        }
    }
}
