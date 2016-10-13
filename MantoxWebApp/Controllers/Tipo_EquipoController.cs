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
    public class Tipo_EquipoController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "Tipo de equipo";
        // GET: Tipo_Equipo
        public async Task<ActionResult> Index()
        {
            return View(await db.Tipos_Equipo.ToListAsync());
        }

        // GET: Tipo_Equipo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Equipo tipo_Equipo = await db.Tipos_Equipo.FindAsync(id);
            if (tipo_Equipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Equipo);
        }

        // GET: Tipo_Equipo/Create
        public ActionResult Create()
        {

            ViewBag.Titulo = "Crear tipo de equipo";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Tipo_Equipo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Tipo_Equipo tipo_Equipo)
        {
            if (ModelState.IsValid)
            {
                db.Tipos_Equipo.Add(tipo_Equipo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tipo_Equipo);
        }

        // GET: Tipo_Equipo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Equipo tipo_Equipo = await db.Tipos_Equipo.FindAsync(id);
            if (tipo_Equipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Equipo);
        }

        // POST: Tipo_Equipo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Tipo_Equipo tipo_Equipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_Equipo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tipo_Equipo);
        }

        // GET: Tipo_Equipo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Equipo tipo_Equipo = await db.Tipos_Equipo.FindAsync(id);
            if (tipo_Equipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Equipo);
        }

        // POST: Tipo_Equipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tipo_Equipo tipo_Equipo = await db.Tipos_Equipo.FindAsync(id);
            db.Tipos_Equipo.Remove(tipo_Equipo);
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
