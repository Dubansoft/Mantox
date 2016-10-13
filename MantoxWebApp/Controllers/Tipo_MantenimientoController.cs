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
    public class Tipo_MantenimientoController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "TipoMantenimiento";
        // GET: Tipo_Mantenimiento
        public async Task<ActionResult> Index()
        {
            return View(await db.Tipos_Mantenimiento.ToListAsync());
        }

        // GET: Tipo_Mantenimiento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Mantenimiento tipo_Mantenimiento = await db.Tipos_Mantenimiento.FindAsync(id);
            if (tipo_Mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Mantenimiento);
        }

        // GET: Tipo_Mantenimiento/Create
        public ActionResult Create()
        {
            ViewBag.Titulo = "Crear tipo de mantenimiento";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View();
        }

        // POST: Tipo_Mantenimiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Tipo_Mantenimiento tipo_Mantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.Tipos_Mantenimiento.Add(tipo_Mantenimiento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tipo_Mantenimiento);
        }

        // GET: Tipo_Mantenimiento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Mantenimiento tipo_Mantenimiento = await db.Tipos_Mantenimiento.FindAsync(id);
            if (tipo_Mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Mantenimiento);
        }

        // POST: Tipo_Mantenimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Tipo_Mantenimiento tipo_Mantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_Mantenimiento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tipo_Mantenimiento);
        }

        // GET: Tipo_Mantenimiento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Mantenimiento tipo_Mantenimiento = await db.Tipos_Mantenimiento.FindAsync(id);
            if (tipo_Mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Mantenimiento);
        }

        // POST: Tipo_Mantenimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tipo_Mantenimiento tipo_Mantenimiento = await db.Tipos_Mantenimiento.FindAsync(id);
            db.Tipos_Mantenimiento.Remove(tipo_Mantenimiento);
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
