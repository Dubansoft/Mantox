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
    public class EquipoController : MantoxController
    {
        private MantoxDBEntities bdMantox = new MantoxDBEntities();

        public string NombreContexto = "Equipo";
        // GET: Equipo
        public async Task<ActionResult> Index()
        {
            ViewBag.Titulo = "Ver equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View(await bdMantox.Equipos.ToListAsync());
        }

        // GET: Equipo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = await bdMantox.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Titulo = "Detalle Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

        
            return View(equipo);
        }

        // GET: Equipo/Create
        public ActionResult Create()
        {

            ////Select para Responsable
            var responsables = bdMantox.Usuarios.Select(responsable => new
            {
                ResponsableId = responsable.Id,
                ResponsableNombre = responsable.Nombre
            }).ToList();
            ViewBag.Responsables = new MultiSelectList(responsables, "ResponsableId", "ResponsableNombre");

            //Select para Areas
            var areas = bdMantox.Areas.Select(area => new
            {
                AreaId = area.Id,
                AreaNombre = area.Nombre
            }).ToList();

            ViewBag.Areas = new MultiSelectList(areas, "AreaId", "AreaNombre");

            //Select para Modelos
            var modelos = bdMantox.Modelos.Select(modelo => new
            {
                ModeloId = modelo.Id,
                ModeloNombre = modelo.Nombre
            }).ToList();

            ViewBag.Modelos = new MultiSelectList(modelos, "ModeloId", "ModeloNombre");

            //Select para Sistemas operativos
            var sistemaoperativos = bdMantox.Sistemas_Operativos.Select(sistemaoperativo => new
            {
                SistemaOperativoId = sistemaoperativo.Id,
                SistemaOperativoNombre = sistemaoperativo.Nombre
            }).ToList();

            ViewBag.SistemasOperativos = new MultiSelectList(sistemaoperativos, "SistemaOperativoId", "SistemaOperativoNombre");


            ////Select para Office
            var versionoffices = bdMantox.Versiones_Office.Select(versionoffice => new
            {
                VersionOfficeId = versionoffice.Id,
                VersionOfficeNombre = versionoffice.Nombre
            }).ToList();

            ViewBag.Office = new MultiSelectList(versionoffices, "VersionOfficeId", "VersionOfficeNombre");

            ////Select para Empresas
            var empresas = bdMantox.Empresas.Select(empresa => new
            {
                EmpresaId = empresa.Id,
                EmpresaNombre = empresa.Nombre
            }).ToList();

            ViewBag.Empresas = new MultiSelectList(empresas, "EmpresaId", "EmpresaNombre");

            ////Select para Propietario
            var propietarios = bdMantox.Usuarios.Select(propietario => new
            {
                PropietarioId = propietario.Id,
                PropietarioNombre = propietario.Nombre
            }).ToList();

            ViewBag.Propietarios = new MultiSelectList(propietarios, "propietarioId", "propietarioNombre");




            ////Select para Estados
            var estados = bdMantox.Estados.Select(estado => new
            {
                EstadoId = estado.Id,
                EstadoNombre = estado.Nombre
            }).ToList();

            //Eliminar los elementos que no se requieren en la vista

            estados.RemoveRange(2, 5);

            ViewBag.Estados = new MultiSelectList(estados, "EstadoId", "EstadoNombre");

            ViewBag.Titulo = "Crear Equipo";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View();
       


        
        }

        // POST: Equipo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Activo,Serial,Nombre_Equipo,Ip,Comentario,Fecha_Ingreso,Fecha_Fin_Garantia,Id_Responsable,Id_Area,Id_Modelo,Id_Sistema_Operativo,Id_Propietario,Id_Version_Office,Id_Estado")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Equipos.Add(equipo);
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
           

            return View(equipo);
        }

        // GET: Equipo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = await bdMantox.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Titulo = "Editar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View(equipo);
        }

        // POST: Equipo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Activo,Serial,Nombre_Equipo,Ip,Comentario,Fecha_Ingreso,Fecha_Fin_Garantia,Id_Responsable,Id_Area,Id_Modelo,Id_Sistema_Operativo,Id_Propietario,Id_Version_Office,Id_Estado")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                bdMantox.Entry(equipo).State = EntityState.Modified;
                await bdMantox.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Titulo = "Editar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

            return View(equipo);
        }

        // GET: Equipo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipo = await bdMantox.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Titulo = "Eliminar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);


            return View(equipo);
        }

        // POST: Equipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Equipo equipo = await bdMantox.Equipos.FindAsync(id);
            bdMantox.Equipos.Remove(equipo);
            await bdMantox.SaveChangesAsync();

            ViewBag.Titulo = "Eliminar Equipos";
            ViewData.Add("NombreContexto", this.NombreContexto);

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
