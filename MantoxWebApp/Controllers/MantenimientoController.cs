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
    public class MantenimientoController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "Mantenimientos";

        // GET: Mantenimiento
        public async Task<ActionResult> Index()
        {
            return View(await db.Mantenimientos.ToListAsync());
        }

        // GET: Mantenimiento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimiento mantenimiento = await db.Mantenimientos.FindAsync(id);
            if (mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(mantenimiento);
        }

        // GET: Mantenimiento/Create
        public ActionResult Create()
        {

            //Select para Id Equipo
            var equipos = db.V_Equipos.Select(equipo => new
            {
                EquipoId = equipo.Id,
                EquipoActivo = equipo.Activo
            }).ToList();

            ViewBag.Equipos = new MultiSelectList(equipos, "EquipoId", "EquipoActivo");

            //Select para Id Usuario
            var usuarios = db.Usuarios.Select(usuario => new
            {
                UsuarioId = usuario.Id,
                UsuarioNombre = usuario.Nombre
            }).ToList();

            ViewBag.Usuarios = new MultiSelectList(usuarios, "UsuarioId", "UsuarioNombre");

            //Select para Id Id Tipo Mantenimiento
            var matenimientos = db.Mantenimientos.Select(mantenimiento => new
            {
                MantenimientoId = mantenimiento.Id,
                MantenimientoNombre = mantenimiento.Id_Tipo_Mantenimiento
            }).ToList();

            ViewBag.MAntenimientos = new MultiSelectList(matenimientos, "matenimientosId", "UsuarioNombre");


            ViewBag.Titulo = "Crear mantenimiento";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Mantenimiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Id_Equipo,Id_Usuario,Id_Tipo_Mantenimiento,Piezas_Instaladas,Piezas_Retiradas,Observaciones")] Mantenimiento mantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.Mantenimientos.Add(mantenimiento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mantenimiento);
        }

        // GET: Mantenimiento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimiento mantenimiento = await db.Mantenimientos.FindAsync(id);
            if (mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(mantenimiento);
        }

        // POST: Mantenimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Id_Equipo,Id_Usuario,Id_Tipo_Mantenimiento,Piezas_Instaladas,Piezas_Retiradas,Observaciones")] Mantenimiento mantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mantenimiento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mantenimiento);
        }

        // GET: Mantenimiento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimiento mantenimiento = await db.Mantenimientos.FindAsync(id);
            if (mantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(mantenimiento);
        }

        // POST: Mantenimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Mantenimiento mantenimiento = await db.Mantenimientos.FindAsync(id);
            db.Mantenimientos.Remove(mantenimiento);
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
