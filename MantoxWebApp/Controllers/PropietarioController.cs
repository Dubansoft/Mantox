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
    public class PropietarioController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "propietarios";

        // GET: Propietario
        public async Task<ActionResult> Index()
        {
            return View(await db.Propietarios.ToListAsync());
        }

        // GET: Propietario/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Propietario propietario = await db.Propietarios.FindAsync(id);
            if (propietario == null)
            {
                return HttpNotFound();
            }
            return View(propietario);
        }

        // GET: Propietario/Create
        public ActionResult Create()
        {
            //Select para Empresa
            var empresas = db.Empresas.Select(empresa => new
            {
                EmpresaId = empresa.Id,
                EmpresaNombre = empresa.Nombre
            }).ToList();

            ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

            ////Select para Estados
            var estados = db.Estados.Select(estado => new
            {
                EstadoId = estado.Id,
                EstadoNombre = estado.Nombre
            }).ToList();

            //Eliminar los elementos que no se requieren en la vista

            estados.RemoveRange(2, 5);

            ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

            ViewBag.Titulo = "Crear propietario";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Propietario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Id_Empresa,Id_Estado")] Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                db.Propietarios.Add(propietario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(propietario);
        }

        // GET: Propietario/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Propietario propietario = await db.Propietarios.FindAsync(id);
            if (propietario == null)
            {
                return HttpNotFound();
            }
            return View(propietario);
        }

        // POST: Propietario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Empresa,Id_Estado")] Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propietario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(propietario);
        }

        // GET: Propietario/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Propietario propietario = await db.Propietarios.FindAsync(id);
            if (propietario == null)
            {
                return HttpNotFound();
            }
            return View(propietario);
        }

        // POST: Propietario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Propietario propietario = await db.Propietarios.FindAsync(id);
            db.Propietarios.Remove(propietario);
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
