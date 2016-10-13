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
    public class SedeController : MantoxController
    {
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Sedes";

        // GET: Sede
        public async Task<ActionResult> Index()
        {
            return View(await bdMantox.Sedes.ToListAsync());
        }

        // GET: Sede/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = await bdMantox.Sedes.FindAsync(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }

        // GET: Sede/Create
        public ActionResult Create()
        {
            //Select para Empresa
            var empresas = bdMantox.Empresas.Select(empresa => new
            {
                EmpresaId = empresa.Id,
                EmpresaNombre = empresa.Nombre
            }).ToList();

            ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

            ////Select para Estados
            var estados = bdMantox.Estados.Select(estado => new
            {
                EstadoId = estado.Id,
                EstadoNombre = estado.Nombre
            }).ToList();

            //Eliminar los elementos que no se requieren en la vista

            estados.RemoveRange(2, 5);

            ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

            ViewBag.Titulo = "Crear sede";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View();
        }

        // POST: Sede/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Ciudad,Departamento,Id_Empresa,Id_Estado")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Sedes.Add(sede);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sede);
        }

        // GET: Sede/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = await bdMantox.Sedes.FindAsync(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }

        // POST: Sede/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Ciudad,Departamento,Id_Empresa,Id_Estado")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(sede).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sede);
        }

        // GET: Sede/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = await bdMantox.Sedes.FindAsync(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }

        // POST: Sede/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sede sede = await bdMantox.Sedes.FindAsync(id);
            bdMantox.Sedes.Remove(sede);
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

        /// <summary>
        /// Devuelve un PartialView que contiene un MultiSelect de las Sedes filtradas por un id de empresa
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarSedes(string idEmpresa = "1")
        {
            var id = int.Parse(idEmpresa);
            List<Sede> sedes = bdMantox.Sedes.Where(s => s.Id_Empresa == id).ToList();

            ViewBag.Sedes = new MultiSelectList(sedes, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarSedes");
        }
    }
}
