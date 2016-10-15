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
    public class EdificioController : MantoxController
    {
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Edificios";

        // GET: Edificio
        public async Task<ActionResult> Index()
        {
            return VistaAutenticada(View(await bdMantox.Edificios.ToListAsync()));
        }

        // GET: Edificio/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Edificio edificio = await bdMantox.Edificios.FindAsync(id);
            if (edificio == null)
            {
                return HttpNotFound();
            }
            return View(edificio);
        }

        // GET: Edificio/Create
        public ActionResult Create()
        {
            //Select para Estados
            var estados = bdMantox.Estados.Select(estado => new
            {
                EstadoId = estado.Id,
                EstadoNombre = estado.Nombre
            }).ToList();

            //Eliminar los elementos que no se requieren en la vista

            estados.RemoveRange(2, 5);

            ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

            //Select para Empresas
            var empresas = bdMantox.Empresas.Select(empresa => new
            {
                EmpresaId = empresa.Id,
                EmpresaNombre = empresa.Nombre
            }).ToList();

            ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

            ViewBag.Titulo = "Crear edificio";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Edificio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Id_Sede,Id_Estado")] Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Edificios.Add(edificio);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(edificio);
        }

        // GET: Edificio/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Edificio edificio = await bdMantox.Edificios.FindAsync(id);
            if (edificio == null)
            {
                return HttpNotFound();
            }
            return View(edificio);
        }

        // POST: Edificio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Sede,Id_Estado")] Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(edificio).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(edificio);
        }

        // GET: Edificio/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Edificio edificio = await bdMantox.Edificios.FindAsync(id);
            if (edificio == null)
            {
                return HttpNotFound();
            }
            return View(edificio);
        }

        // POST: Edificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Edificio edificio = await bdMantox.Edificios.FindAsync(id);
            bdMantox.Edificios.Remove(edificio);
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
        /// Devuelve un PartialView que contiene un MultiSelect de los Edificios filtrados por un id de sede
        /// </summary>
        /// <param name="idSede">Id de la sede</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarEdificios(string idSede = "1")
        {
            var id = int.Parse(idSede);
            List<Edificio> sedes = bdMantox.Edificios.Where(s => s.Id_Sede == id).ToList();

            ViewBag.Edificios = new MultiSelectList(sedes, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarEdificios");
        }
    }
}
