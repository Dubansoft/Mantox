using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MovieController : Controller
    {
        private MoviesDBEntities _db = new MoviesDBEntities();
        // GET: Movie
        public ActionResult Index()
        {
            
            return View(_db.Movies.ToList());
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            var movieToEdit = (from m in _db.Movies

                               where m.Id == id

                               select m).First();

            return View(movieToEdit);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult Create([Bind(Exclude = "Id")] Movie movieToCreate)

        {

            if (!ModelState.IsValid)

                return View();

            _db.Movies.Add(movieToCreate);

            _db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: /Home/Edit/5 

        public ActionResult Edit(int id)

        {

            var movieToEdit = (from m in _db.Movies

                               where m.Id == id

                               select m).First();

            return View(movieToEdit);

        }

        //


        // POST: /Home/Edit/5 

        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult Edit(Movie movieToEdit)

        {

            var originalMovie = (from m in _db.Movies

                                 where m.Id == movieToEdit.Id

                                 select m).First();

            if (!ModelState.IsValid)

                return View(originalMovie);

            _db.Entry(originalMovie).CurrentValues.SetValues(movieToEdit);
            _db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
