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
using FileHelper;
using System.Reflection;

namespace MantoxWebApp.Controllers
{
    public class PropietarioController : MantoxController
    {
        ///Instancia de conexión por framework a la base de datos
        private MantoxDBEntities bdMantox = new MantoxDBEntities();
        public string NombreContexto = "Propietarios";
        public string NombreObjeto = "Propietario";

        /// <summary>
        /// Lista de propietarios
        /// </summary>
        IEnumerable propietarios; //Almacenará la lista de usuarios que se asignarán como responsables de los equipos


        // GET: Propietario
        public async Task<ActionResult> Index()
        {
            return View(await bdMantox.Propietarios.ToListAsync());
        }

        // GET: Propietario/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Propietario propietario = await bdMantox.Propietarios.FindAsync(id);
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
                bdMantox.Propietarios.Add(propietario);
                await bdMantox.SaveChangesAsync();
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
            Propietario propietario = await bdMantox.Propietarios.FindAsync(id);
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
                bdMantox.Entry(propietario).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
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
            Propietario propietario = await bdMantox.Propietarios.FindAsync(id);
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
            Propietario propietario = await bdMantox.Propietarios.FindAsync(id);
            bdMantox.Propietarios.Remove(propietario);
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
        /// Devuelve un PartialView que contiene un MultiSelect de los propietarios
        /// filtrados por un id de empresa
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <returns>PartialView</returns>
        public PartialViewResult FiltrarPropietarios(string idEmpresa = "1")
        {
            try
            {
                var id_empresa = int.Parse(idEmpresa);

                //Select para Responsables
                propietarios = bdMantox.Propietarios.Select(propietario => new
                {
                    PropietarioId = propietario.Id,
                    PropietarioNombre = propietario.Nombre,
                    EstadoId = propietario.Id_Estado,
                    EmpresaId = propietario.Id_Empresa
                })
                .Where //Se añade condición para mostrar sólo propietarios activos
                    (r => (int)r.EstadoId == (int)EstadoMantox.Activo)
                .Where //Se añade condición para mostrar sólo usuarios de la empresa seleccionada
                    (r => (int)r.EmpresaId == id_empresa)
                .ToList();

                ViewBag.Propietarios = new MultiSelectList(propietarios, "PropietarioId", "PropietarioNombre");

                return VistaAutenticada(PartialView("_VistaParcial_FiltrarPropietarios"), RolDeUsuario.Administrador);

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = EventLogger.LogEvent(this, e.Message.ToString(), e, MethodBase.GetCurrentMethod().Name);
                return PartialView("ErrorInterno", "Error");
            }
        }
    }
}
