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
    public class Razon_MovimientoController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "RazonMovimiento";

        // GET: Razon_Movimiento
        public async Task<ActionResult> Index()
        {
            return View(await db.Razones_Movimiento.ToListAsync());
        }

        // GET: Razon_Movimiento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Razon_Movimiento razon_Movimiento = await db.Razones_Movimiento.FindAsync(id);
            if (razon_Movimiento == null)
            {
                return HttpNotFound();
            }
            return View(razon_Movimiento);
        }

        // GET: Razon_Movimiento/Create
        public ActionResult Create()
        {
            ViewBag.Titulo = "Crear Razon de movimiento";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Razon_Movimiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Razon_Movimiento razon_Movimiento)
        {
            if (ModelState.IsValid)
            {
                db.Razones_Movimiento.Add(razon_Movimiento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(razon_Movimiento);
        }

        // GET: Razon_Movimiento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Razon_Movimiento razon_Movimiento = await db.Razones_Movimiento.FindAsync(id);
            if (razon_Movimiento == null)
            {
                return HttpNotFound();
            }
            return View(razon_Movimiento);
        }

        // POST: Razon_Movimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Razon_Movimiento razon_Movimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(razon_Movimiento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(razon_Movimiento);
        }

        // GET: Razon_Movimiento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Razon_Movimiento razon_Movimiento = await db.Razones_Movimiento.FindAsync(id);
            if (razon_Movimiento == null)
            {
                return HttpNotFound();
            }
            return View(razon_Movimiento);
        }

        // POST: Razon_Movimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Razon_Movimiento razon_Movimiento = await db.Razones_Movimiento.FindAsync(id);
            db.Razones_Movimiento.Remove(razon_Movimiento);
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
