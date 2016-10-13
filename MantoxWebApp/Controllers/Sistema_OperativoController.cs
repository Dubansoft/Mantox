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
    public class Sistema_OperativoController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "Sistemas operativo";

        // GET: Sistema_Operativo
        public async Task<ActionResult> Index()
        {
            return View(await db.Sistemas_Operativos.ToListAsync());
        }

        // GET: Sistema_Operativo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sistema_Operativo sistema_Operativo = await db.Sistemas_Operativos.FindAsync(id);
            if (sistema_Operativo == null)
            {
                return HttpNotFound();
            }
            return View(sistema_Operativo);
        }

        // GET: Sistema_Operativo/Create
        public ActionResult Create()
        {
            ViewBag.Titulo = "Crear Sistemas operativo";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Sistema_Operativo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Sistema_Operativo sistema_Operativo)
        {
            if (ModelState.IsValid)
            {
                db.Sistemas_Operativos.Add(sistema_Operativo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sistema_Operativo);
        }

        // GET: Sistema_Operativo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sistema_Operativo sistema_Operativo = await db.Sistemas_Operativos.FindAsync(id);
            if (sistema_Operativo == null)
            {
                return HttpNotFound();
            }
            return View(sistema_Operativo);
        }

        // POST: Sistema_Operativo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Sistema_Operativo sistema_Operativo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sistema_Operativo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sistema_Operativo);
        }

        // GET: Sistema_Operativo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sistema_Operativo sistema_Operativo = await db.Sistemas_Operativos.FindAsync(id);
            if (sistema_Operativo == null)
            {
                return HttpNotFound();
            }
            return View(sistema_Operativo);
        }

        // POST: Sistema_Operativo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sistema_Operativo sistema_Operativo = await db.Sistemas_Operativos.FindAsync(id);
            db.Sistemas_Operativos.Remove(sistema_Operativo);
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
