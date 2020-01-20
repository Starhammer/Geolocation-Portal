using Geolocation_Portal_Test.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public ActionResult Recordverwaltung()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            return View(db.record.ToList());
        }

        /**
         * Öffnet ein Formular mit welchem ein neuer Datensatz hinzugefügt werden kann.
         */
        public ActionResult Record()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            ViewBag.Message = "Your application description page.";

            ViewBag.category_id = new SelectList(db.category, "Id", "name");
            ViewBag.publisher_id = new SelectList(db.publisher, "Id", "name");
            ViewBag.licence_id = new SelectList(db.licence, "Id", "name");
            ViewBag.role_id = new SelectList(db.role, "Id", "name");
            ViewBag.role_description = new SelectList(db.role, "Id", "description");
            ViewBag.location_id = new SelectList(db.location, "Id", "name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Record([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active,location_id")] record record, IEnumerable<HttpPostedFileBase> files)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            if (ModelState.IsValid)
            {
                var datType = Request["dataTyp"];

                switch (datType)
                {
                    case "geoData":
                        record.geo_data = true;
                        break;
                    case "diaData":
                        record.dia_data = true;
                        break;
                }

                db.record.Add(record);
                db.SaveChanges();

                
                

                if (files != null) saveFiles(files, record.Id);
                
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
            if (!checkSession())
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

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

            // Datensatz Sichtbarkeit auslesen
            int record_visibility_role = (int)db.record.Find(id).role_id;
            int logged_in_user = 4;
            bool logged_in = false;
            if (checkSession())
            {
                logged_in_user = Convert.ToInt32(Session["UserRole"], new CultureInfo("de-DE"));
                logged_in = true;
            }

            // Rollenkonzept:
            // Datensatz Sichtbarkeit = 4 (Öffentlichkeit) oder Benutzer angemeldet und Rolle berechtigt Datensatz zu sehen
            if (record_visibility_role > 3 || logged_in == true && record_visibility_role <= logged_in_user)
            {
                record record = db.record.Find(id);
                record.licence = db.licence.Find(record.licence_id);
                record.publisher = db.publisher.Find(record.publisher_id);

                int downloadcount = 0;
                int file_size_conversion_count = 0; // 0 = Bytes

                foreach (file f in record.file)
                {
                    // Downloadanzahl ermitteln.
                    int file_downloadcount = (int)f.download_count;
                    downloadcount = downloadcount + file_downloadcount;

                    // Dateigröße in bessere Darstellung umwandeln.
                    file_size_conversion_count = 0; // 0 = Bytes
                    while (f.file_size > 1024) {
                        f.file_size = f.file_size / 1024;
                        file_size_conversion_count++;
                    }

                    f.file_size = Math.Round((double)f.file_size, 2);
                }

                // Dateigrößenbezeichnung ermitteln.
                ViewData["downloadcount"] = downloadcount;

                string file_size_extension = "Bytes";
                switch (file_size_conversion_count) {
                    case 0:
                        file_size_extension = "Bytes";
                        break;
                    case 1:
                        file_size_extension = "KB";
                        break;
                    case 2:
                        file_size_extension = "MB";
                        break;
                    case 3:
                        file_size_extension = "GB";
                        break;
                    case 4:
                        file_size_extension = "PB";
                        break;
                    default:
                        file_size_extension = "nicht festgelegt";
                        break;
                }

                ViewData["file_size_extension"] = file_size_extension;

                if (record == null)
                {
                    return HttpNotFound();
                }

                return View(record);
            }
            else
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }
        }

        public ActionResult Recordentfernung(int? id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

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

        // POST: user/Delete/5
        [HttpPost, ActionName("Recordentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult RecordentfernungConfirmed(int id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            record record = db.record.Find(id);
            db.record.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Recordentfernung");
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

        private void saveFiles(IEnumerable<HttpPostedFileBase> files, int recordID)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Server.MapPath("~/App_Data/uploads/" + recordID);
                    var filePath = Path.Combine(path, fileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    file.SaveAs(filePath);
                    

                    db.file.Add(new Models.file
                    {
                        record_id = recordID,
                        name = file.FileName,
                        file_upload_date = System.DateTime.Now,
                        download_count = 0,
                        file_size = file.ContentLength,
                        file_icon = getFileIcon(fileName)                      
                    });

                }
                else
                {
                    return;
                }
            }

            db.SaveChanges();
        }

        private bool checkSession()
        {
            if (Session["UserRole"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult DownLoad(string fileName, int recordId)
        {            
            string path = Server.MapPath("~/App_Data/uploads/"+ recordId + "/"+ fileName);
            string mime = MimeMapping.GetMimeMapping(path);
            

            file file = db.file.Where(f => f.name == fileName && f.record_id == recordId).First();
            if (file != null) { 
                file.download_count++;
                db.SaveChanges();
            }

            return File(path, mime, file.name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordComment([Bind(Include = "")] comment comment)
        {
            return null;
        }
    }
}