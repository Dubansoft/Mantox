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
    public class LicenciaController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "Licencias";

        // GET: Licencia
        public async Task<ActionResult> Index()
        {
            return View(await db.Licencias.ToListAsync());
        }

        // GET: Licencia/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Licencia licencia = await db.Licencias.FindAsync(id);
            if (licencia == null)
            {
                return HttpNotFound();
            }
            return View(licencia);
        }

        // GET: Licencia/Create
        public ActionResult Create()
        {
            //Select para Licencias
            var licencias = db.Licencias.Select(licencia => new
            {
                LicenciaId = licencia.Id,
                LicenciaNombre = licencia.Id_Tipo_Licencia
            }).ToList();

            ViewBag.Licencias = new MultiSelectList(licencias, "LicenciaId", "LicenciaId_Tipo_Licencia");

             //Select para  equipo
            var equipos = db.Equipos.Select(equipo => new
            {
                EquipoId = equipo.Id,
                EquipoNombre = equipo.Nombre_Equipo
            }).ToList();

            ViewBag.Equipos = new MultiSelectList(equipos, "EquipoId", "EquipoNombre");

            ViewBag.Titulo = "Crear Licencia";
            ViewData.Add("NombreContexto", this.NombreContexto);
        

            return View();
        }

        // POST: Licencia/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Id_Tipo_Licencia,Serial,Fecha_Vencimiento,Fecha_Compra,Id_Equipo")] Licencia licencia)
        {
            if (ModelState.IsValid)
            {
                db.Licencias.Add(licencia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(licencia);
        }

        // GET: Licencia/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Licencia licencia = await db.Licencias.FindAsync(id);
            if (licencia == null)
            {
                return HttpNotFound();
            }
            return View(licencia);
        }

        // POST: Licencia/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Id_Tipo_Licencia,Serial,Fecha_Vencimiento,Fecha_Compra,Id_Equipo")] Licencia licencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(licencia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(licencia);
        }

        // GET: Licencia/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Licencia licencia = await db.Licencias.FindAsync(id);
            if (licencia == null)
            {
                return HttpNotFound();
            }
            return View(licencia);
        }

        // POST: Licencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Licencia licencia = await db.Licencias.FindAsync(id);
            db.Licencias.Remove(licencia);
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
