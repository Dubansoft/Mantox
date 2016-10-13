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
    public class MovimientoController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "Movientos";

        // GET: Movimiento
        public async Task<ActionResult> Index()
        {
            return View(await db.Movimientos.ToListAsync());
        }

        // GET: Movimiento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimiento movimiento = await db.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return HttpNotFound();
            }
            return View(movimiento);
        }

        // GET: Movimiento/Create
        public ActionResult Create()
        {

            //Select para Area destinos
            var destinos = db.Movimientos.Select(destino => new
            {
                DestinoId = destino.Id,
                DestinoId_Area_Destino = destino.Id_Area_Destino
            }).ToList();

            ViewBag.Destinos = new MultiSelectList(destinos, "DestinoId", " DestinoId_Area_Destino");

            //Select para Sedes
            var sedes = db.Sedes.Select(sede => new
            {
                SedeId = sede.Id,
                SedeNombre = sede.Nombre
            }).ToList();

            ViewBag.Sedes = new MultiSelectList(sedes, "SedeId", "SedeNombre");

            ViewBag.Titulo = "Crear movimiento";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Movimiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Fecha,Id_Equipo,Id_Area_Origen,Id_Area_Destino,Razon_Movimiento,Id_Usuario")] Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                db.Movimientos.Add(movimiento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(movimiento);
        }

        // GET: Movimiento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimiento movimiento = await db.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return HttpNotFound();
            }
            return View(movimiento);
        }

        // POST: Movimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Fecha,Id_Equipo,Id_Area_Origen,Id_Area_Destino,Razon_Movimiento,Id_Usuario")] Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimiento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movimiento);
        }

        // GET: Movimiento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimiento movimiento = await db.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return HttpNotFound();
            }
            return View(movimiento);
        }

        // POST: Movimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Movimiento movimiento = await db.Movimientos.FindAsync(id);
            db.Movimientos.Remove(movimiento);
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
