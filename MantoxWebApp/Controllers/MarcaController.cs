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
    public class MarcaController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "Marcas";

        // GET: Marca
        public async Task<ActionResult> Index()
        {
            return View(await db.Marcas.ToListAsync());
        }

        // GET: Marca/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marcas = await db.Marcas.FindAsync(id);
            if (marcas == null)
            {
                return HttpNotFound();
            }
            return View(marcas);
        }

        // GET: Marca/Create
        public ActionResult Create()
        {
            ////Select para Estados
            var estados = db.Estados.Select(estado => new
            {
                EstadoId = estado.Id,
                EstadoNombre = estado.Nombre
            }).ToList();

            //Eliminar los elementos que no se requieren en la vista

            estados.RemoveRange(2, 5);

            ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

            ViewBag.Titulo = "Crear marca";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View();
        }

        // POST: Marca/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,nombre,id_estado")] Marca marcas)
        {
            if (ModelState.IsValid)
            {
                db.Marcas.Add(marcas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(marcas);
        }

        // GET: Marca/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marcas = await db.Marcas.FindAsync(id);
            if (marcas == null)
            {
                return HttpNotFound();
            }
            return View(marcas);
        }

        // POST: Marca/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombre,id_estado")] Marca marcas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marcas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(marcas);
        }

        // GET: Marca/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marcas = await db.Marcas.FindAsync(id);
            if (marcas == null)
            {
                return HttpNotFound();
            }
            return View(marcas);
        }

        // POST: Marca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Marca marcas = await db.Marcas.FindAsync(id);
            db.Marcas.Remove(marcas);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
