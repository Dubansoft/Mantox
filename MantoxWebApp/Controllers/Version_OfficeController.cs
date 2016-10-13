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
    public class Version_OfficeController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "VersionOffice";
        // GET: Version_Office
        public async Task<ActionResult> Index()
        {
            return View(await db.Versiones_Office.ToListAsync());
        }

        // GET: Version_Office/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Version_Office version_Office = await db.Versiones_Office.FindAsync(id);
            if (version_Office == null)
            {
                return HttpNotFound();
            }
            return View(version_Office);
        }

        // GET: Version_Office/Create
        public ActionResult Create()
        {
            ViewBag.Titulo = "Crear Office";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Version_Office/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Version_Office version_Office)
        {
            if (ModelState.IsValid)
            {
                db.Versiones_Office.Add(version_Office);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(version_Office);
        }

        // GET: Version_Office/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Version_Office version_Office = await db.Versiones_Office.FindAsync(id);
            if (version_Office == null)
            {
                return HttpNotFound();
            }
            return View(version_Office);
        }

        // POST: Version_Office/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Version_Office version_Office)
        {
            if (ModelState.IsValid)
            {
                db.Entry(version_Office).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(version_Office);
        }

        // GET: Version_Office/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Version_Office version_Office = await db.Versiones_Office.FindAsync(id);
            if (version_Office == null)
            {
                return HttpNotFound();
            }
            return View(version_Office);
        }

        // POST: Version_Office/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Version_Office version_Office = await db.Versiones_Office.FindAsync(id);
            db.Versiones_Office.Remove(version_Office);
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
