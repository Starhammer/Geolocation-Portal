using Geolocation_Portal_Test.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
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

            ViewBag.category = (List<category>)db.category.ToList();

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

            ViewBag.category_id = new SelectList(db.category, "Id", "name");
            ViewBag.publisher_id = new SelectList(db.publisher, "Id", "name");
            ViewBag.licence_id = new SelectList(db.licence, "Id", "name");
            ViewBag.role_id = new SelectList(db.role, "Id", "name");
            ViewBag.location_id = new SelectList(db.location, "Id", "name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Record([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active,location_id")] record record, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                db.record.Add(record);
                db.SaveChanges();

                
                

                if (files != null) saveFiles(files, record.Id, Request["dataTyp"]);
                
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(db.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(db.licence, "Id", "name", record.licence_id);
            ViewBag.role_id = new SelectList(db.role, "Id", "name");
            ViewBag.location_id = new SelectList(db.location, "Id", "name");

            return View(record);
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

            ViewBag.category = (List<category>)db.category.ToList();
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

        public ActionResult Recorddetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            record record = db.record.Find(id);

            if (record == null)
            {
                return HttpNotFound();
            }

            return View(record);
        }

        [HttpPost]
        public ActionResult Search()
        {
            var searchtext = Request["search"];

            IQueryable<record> data;

            data = from f in db.record
                   where f.record_active == true && 
                   ( f.description.Contains(searchtext) || f.title.Contains(searchtext) )
                   select f;

            if(data != null && data.Count() > 0)
            {
                ViewBag.message = "Die Suche nach '"+ searchtext + "' ergab "+data.Count()+" treffer";
            }
            else
            {
                ViewBag.message = "Die Suche ergab keine Treffer";
            }
            
            ViewBag.category = (List<category>)db.category.ToList();
            return View("index", data.ToList());
        }

        /***
         * asdasdad
         */
        private string getFileIcon(string name)
        {
            var extension = Path.GetExtension(name);
            return extension.Remove(0, 1);
        }

        private void saveFiles(IEnumerable<HttpPostedFileBase> files, int recordID, string datType)
        {
            foreach (var file in files)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Server.MapPath("~/App_Data/uploads/" + recordID);
                    var filePath = Path.Combine(path, fileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    file.SaveAs(filePath);

                    var geoData = false;
                    var diaData = false;

                    switch (datType)
                    {
                        case "geoData":
                            geoData = true;
                            break;
                        case "diaData":
                            diaData = true;
                            break;
                    }

                    db.file.Add(new Models.file
                    {
                        record_id = recordID,
                        name = file.FileName,
                        file_upload_date = System.DateTime.Now,
                        download_count = 0,
                        file_size = file.ContentLength,
                        file_icon = getFileIcon(fileName),
                        map_data = geoData,
                        diagram_data = diaData
                      
                    });

                }
            }

            db.SaveChanges();
        }
    }
}