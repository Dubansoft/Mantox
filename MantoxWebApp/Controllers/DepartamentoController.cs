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
using FileHelper;
using System.Reflection;
using System.Collections;

namespace MantoxWebApp.Controllers
{
    public class DepartamentoController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Departamentos";
        public string NombreObjeto = "Departamento";


        /// <summary>
        /// Lista de paises disponibles en la lista desplegable
        /// </summary>
        IEnumerable paises; //Almacenará la lista de paises

        /// <summary>
        /// Lista de departamentos disponibles en la lista desplegable
        /// </summary>
        IEnumerable departamentos; //Almacenará la lista de departamentos

        // GET: Departamento
        public async Task<ActionResult> Index()
        {
            return View(await bdMantox.Departamentos.ToListAsync());
        }

        // GET: Departamento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = await bdMantox.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // GET: Departamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Id_Pais")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Departamentos.Add(departamento);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(departamento);
        }

        // GET: Departamento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = await bdMantox.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // POST: Departamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Pais")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(departamento).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(departamento);
        }

        // GET: Departamento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamento departamento = await bdMantox.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // POST: Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Departamento departamento = await bdMantox.Departamentos.FindAsync(id);
            bdMantox.Departamentos.Remove(departamento);
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
        /// <param name="idPais">Id del pais</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarDepartamentos(string idPais = "1")
        {
            var id = int.Parse(idPais);
            List<Departamento> departamentos = bdMantox.Departamentos.Where(s => s.Id_Pais == id).ToList();

            ViewBag.Departamentos = new MultiSelectList(departamentos, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarDepartamentos");
        }
    }
}
    

