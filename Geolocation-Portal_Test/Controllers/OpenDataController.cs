using Geolocation_Portal_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Geolocation_Portal_Test.Controllers
{
    public class OpenDataController : Controller
    {
        private Entities db = new Entities();

        // GET: OpenData
        public ActionResult Index()
        {  
            var data = from f in db.record
                            where f.record_active == true
                            select f;

            return View(data.ToList());
            
        }

        public ActionResult Record()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //GET: OpenDate/Category/5
        public ActionResult Category(int? id)
        {
            IQueryable<record> data;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            data = from f in db.record
                   where f.record_active == true
                   && f.category_id == id
                   select f;

            return View("index", data.ToList());
                

        }

        //Nicht als eigentliche View gedacht
        //GET: OpenDate/Files/5
        public ActionResult Files(int id)
        {
            var data = from f in db.file
                       where f.record_id == id
                       select f;

            return PartialView("_files", data.ToList());
        }
    }
}