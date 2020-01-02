using Geolocation_Portal_Test.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class OpenDataController : Controller
    {
        private Entities db = new Entities();

        // GET: OpenData
        public ActionResult Index()
        {  
            if(db.record == null) {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            var data = from f in db.record
                            where f.record_active == true
                            select f;

            return View(data.ToList());
            
        }

        public ActionResult Datensatzverwaltung()
        {
            return View(db.record.ToList());
        }

        /**
         * Öffnet ein Formular mit welchem ein neuer Datensatz hinzugefügt werden kann.
         */
        public ActionResult Record()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Recordbearbeitung()
        {
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

        /**
         * Zeigt einen Datensatz als Diagramm an.
         */
        public ActionResult Diagramm()
        {
            return View();
        }

        /**
         * Erstellt aus einer Datei ein Diagramm.
         */
        public ActionResult createDiagram()
        {
            return View();
        }
    }
}