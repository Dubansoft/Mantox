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
    public class ModeloController : Controller
    {
        private MantoxDBEntities db = new MantoxDBEntities();
        public string NombreContexto = "Modelos";

        // GET: Modelo
        public async Task<ActionResult> Index()
        {
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View(await db.Modelos.ToListAsync());
        }

        // GET: Modelo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = await db.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // GET: Modelo/Create
        public ActionResult Create()
        {
            //Select para Id Marca
            var marcas = db.Marcas.Select(marca => new
            {
                MarcaId = marca.id,
                MarcaNombre = marca.nombre
            }).ToList();

            ViewBag.Marcas = new MultiSelectList(marcas, "MarcaId", "Marcanombre");

            //Select para Id Tipo de Equipo
            var equipos = db.Tipos_Equipo.Select(equipo => new
            {
                EquipoId = equipo.Id,
                EquipoNombre = equipo.Nombre
            }).ToList();

            ViewBag.Equipos = new MultiSelectList(equipos, "EquipoId", "EquipoNombre");

            ////Select para Estados
            var estados = db.Estados.Select(estado => new
            {
                EstadoId = estado.Id,
                EstadoNombre = estado.Nombre
            }).ToList();

            //Eliminar los elementos que no se requieren en la vista

            estados.RemoveRange(2, 5);

            ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

            ViewBag.Titulo = "Crear modelos";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View();
        }

        // POST: Modelo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Id_Marca,Id_Tipo_Equipo,Id_Estado")] Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                db.Modelos.Add(modelo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Titulo = "Crear modelos";
            ViewData.Add("NombreContexto", this.NombreContexto);
            return View(Index());
        }

        // GET: Modelo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = await db.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // POST: Modelo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Marca,Id_Tipo_Equipo,Id_Estado")] Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(modelo);
        }

        // GET: Modelo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = await db.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // POST: Modelo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Modelo modelo = await db.Modelos.FindAsync(id);
            db.Modelos.Remove(modelo);
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
