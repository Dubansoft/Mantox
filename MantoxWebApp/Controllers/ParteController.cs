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
    public class ParteController : MantoxController
    {
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Partes";

        // GET: Parte
        public async Task<ActionResult> Index()
        {
            return VistaAutenticada(View(await bdMantox.Partes.ToListAsync()));
        }

        // GET: Parte/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parte parte = await bdMantox.Partes.FindAsync(id);
            if (parte == null)
            {
                return HttpNotFound();
            }
            return View(parte);
        }

        // GET: Parte/Create
        public ActionResult Create()
        {
            //Select para Partes de equipo
            var partes = bdMantox.Partes.Select(parte => new
            {
                ParteId = parte.Id,
                ParteNombre = parte.Nombre
            }).ToList();

            ViewBag.Partes = new MultiSelectList(partes, "ParteId", "ParteNombre");

            ViewBag.Titulo = "Crear parte";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Parte/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Id_Tipo_Equipo")] Parte parte)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Partes.Add(parte);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(parte);
        }

        // GET: Parte/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parte parte = await bdMantox.Partes.FindAsync(id);
            if (parte == null)
            {
                return HttpNotFound();
            }
            return View(parte);
        }

        // POST: Parte/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Tipo_Equipo")] Parte parte)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(parte).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(parte);
        }

        // GET: Parte/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parte parte = await bdMantox.Partes.FindAsync(id);
            if (parte == null)
            {
                return HttpNotFound();
            }
            return View(parte);
        }

        // POST: Parte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Parte parte = await bdMantox.Partes.FindAsync(id);
            bdMantox.Partes.Remove(parte);
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
    }
}
