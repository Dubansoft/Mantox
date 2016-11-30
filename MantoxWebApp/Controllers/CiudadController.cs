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
using System.Collections;

namespace MantoxWebApp.Controllers
{
    public class CiudadController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Ciudades";
        public string NombreObjeto = "Ciudad";


        /// <summary>
        /// Lista de paises disponibles en la lista desplegable
        /// </summary>
        IEnumerable ciudades; //Almacenará la lista de paises

        /// <summary>
        /// Lista de departamentos disponibles en la lista desplegable
        /// </summary>
        IEnumerable departamentos; //Almacenará la lista de departamentos

        // GET: Ciudad
        public async Task<ActionResult> Index()
        {
            return View(await bdMantox.Ciudades.ToListAsync());
        }

        // GET: Ciudad/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = await bdMantox.Ciudades.FindAsync(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // GET: Ciudad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ciudad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Id_Departamento")] Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Ciudades.Add(ciudad);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ciudad);
        }

        // GET: Ciudad/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = await bdMantox.Ciudades.FindAsync(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: Ciudad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Id_Departamento")] Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(ciudad).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ciudad);
        }

        // GET: Ciudad/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = await bdMantox.Ciudades.FindAsync(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: Ciudad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ciudad ciudad = await bdMantox.Ciudades.FindAsync(id);
            bdMantox.Ciudades.Remove(ciudad);
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
        /// <param name="idDepartamento">Id del pais</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarCiudades(string idDepartamento = "1")
        {
            var id = int.Parse(idDepartamento);
            List<Ciudad> ciudades = bdMantox.Ciudades.Where(s => s.Id_Departamento == id).ToList();

            ViewBag.Ciudades = new MultiSelectList(ciudades, "Id", "Nombre");

            return PartialView("_VistaParcial_FiltrarCiudades");
        }
    }
}
