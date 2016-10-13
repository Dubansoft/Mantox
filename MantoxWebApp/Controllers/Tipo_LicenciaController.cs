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
    public class Tipo_LicenciaController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "TipoLicencia";

        // GET: Tipo_Licencia
        public async Task<ActionResult> Index()
        {
            return View(await db.Tipos_Licencia.ToListAsync());
        }

        // GET: Tipo_Licencia/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Licencia tipo_Licencia = await db.Tipos_Licencia.FindAsync(id);
            if (tipo_Licencia == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Licencia);
        }

        // GET: Tipo_Licencia/Create
        public ActionResult Create()
        {
            ViewBag.Titulo = "Crear tipo de licencia";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Tipo_Licencia/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Tipo_Licencia tipo_Licencia)
        {
            if (ModelState.IsValid)
            {
                db.Tipos_Licencia.Add(tipo_Licencia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tipo_Licencia);
        }

        // GET: Tipo_Licencia/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Licencia tipo_Licencia = await db.Tipos_Licencia.FindAsync(id);
            if (tipo_Licencia == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Licencia);
        }

        // POST: Tipo_Licencia/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Tipo_Licencia tipo_Licencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_Licencia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tipo_Licencia);
        }

        // GET: Tipo_Licencia/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Licencia tipo_Licencia = await db.Tipos_Licencia.FindAsync(id);
            if (tipo_Licencia == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Licencia);
        }

        // POST: Tipo_Licencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tipo_Licencia tipo_Licencia = await db.Tipos_Licencia.FindAsync(id);
            db.Tipos_Licencia.Remove(tipo_Licencia);
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
