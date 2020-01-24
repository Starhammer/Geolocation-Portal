using Geolocation_Portal_Test.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Geolocation_Portal_Test.Controllers
{
    public class OpenDataController : Controller
    {
        /// <summary>
        /// Object of the database tables. Each database table is defined as a model below the entities.
        /// The entity object allows you to select tupels from the database and return a model of each tupel.
        /// </summary>
        private Entities myDatabaseEntities = new Entities();

        /// <summary>
        /// Determines which records should be returned.
        /// </summary>
        [Flags]
        public enum restriction_enum
        {
            all_records,
            all_active_records,
            geo_records,
            dia_records,
            sort_alpha_asc,
            sort_alpha_desc,
            sort_date_asc,
            sort_date_desc
        }

        /// <summary>
        /// This action result returns the index view. On the index page there is an overview of the 
        /// created data records.
        /// </summary>
        /// <param name="restriction">Determines which records should be returned.</param>
        /// <param name="categoryID">Determines which category records should be returned.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Index(string restriction_string = "all_active_records", int categoryID = 0, string searchTerm = null)
        {
            // ToDo: In future "string restriction_string = "all_records"" should be "restriction_enum restriction = restriction_enum.all_records"
            // ToDo: Current problem is the use of the enum in the view. As solution the string is converted into an enum.
            restriction_enum restriction = (restriction_enum)Enum.Parse(typeof(restriction_enum), restriction_string, true);

            // parameter verification cop to avoid errors.
            if (categoryID == 0)
            {
                // 0 is required to avoid filtering by category.
            }

            if (searchTerm == null)
            {
                // null is required to avoid searching.
                searchTerm = Request["search"];
            }

            // Check all required database tables for their contents to avoid errors.
            if (checkDatabaseContent(myDatabaseEntities.record) &&
                checkDatabaseContent(myDatabaseEntities.category))
            {
                // Load the categories to display the filter function. The category filter is implemented using icons defined in the database.
                ViewBag.category = (List<category>)myDatabaseEntities.category.ToList();
                // Allows the user to reset the record restriction.
                ViewBag.resetRestriction = true;

                string userInfoMessage = null;

                // Load the records to return them to the view.
                // The variable is null at this position because the restriction parameter still needs to be checked.
                IEnumerable<record> records = null;

                // Check record display authorization using the role ID.
                int loggedInUserRole = 4;
                if (Session["UserRole"] != null)
                {
                    // Is needed for the Where condition.
                    loggedInUserRole = Convert.ToInt32(Session["UserRole"], new CultureInfo("de-DE"));
                }

                if (categoryID == 0)
                {
                    if (searchTerm == null)
                    {
                        switch (restriction)
                        {
                            case (restriction_enum.all_records):
                                // Read out all data records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.all_active_records):
                                // Read out all "active" data records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          select f;
                                ViewBag.resetRestriction = false;   // If "all" data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.geo_records):
                                // Read out all "geo" data records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true &&
                                          f.geo_data == true
                                          select f;
                                break;
                            case (restriction_enum.dia_records):
                                // Read out all "diagram" data records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true &&
                                          f.dia_data == true
                                          select f;
                                break;
                            case (restriction_enum.sort_alpha_asc):
                                // Sort all records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          orderby f.title
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.sort_alpha_desc):
                                // Read out all "diagram" data records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          orderby f.title descending
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.sort_date_asc):
                                // Read out all "diagram" data records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          orderby f.dataset_modified_date
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.sort_date_desc):
                                // Read out all "diagram" data records from the database.
                                records = from f in myDatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          orderby f.dataset_modified_date descending
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            default:
                                // do nothing to avoid errors if case is not available.
                                break;
                        }
                    }
                    else
                    {
                        // Read out all data records from the database with a certain search term Compliance.
                        records = from f in myDatabaseEntities.record
                                  where f.role_id >= loggedInUserRole &&
                                  f.record_active == true &&
                                  (f.title.Contains(searchTerm) || f.description.Contains(searchTerm))
                                  select f;

                        if (records != null && records.Count() == 0)
                        {
                            userInfoMessage = "Die Suche ergab keinen Treffer.";
                        }
                        else if (records != null && records.Count() == 1)
                        {
                            userInfoMessage = "Die Suche nach '" + searchTerm + "' ergab einen Treffer.";
                        }
                        else 
                        {
                            userInfoMessage = "Die Suche nach '" + searchTerm + "' zeigt " + records.Count() + " Datensätze an.";
                        }
                    }
                }
                else
                {
                    // Read out all data records from the database with a certain category affiliation.
                    category category = myDatabaseEntities.category.Find(categoryID);
                    if (category == null)
                    {
                        return HttpNotFound();
                    }

                    records = from f in myDatabaseEntities.record
                              where f.role_id >= loggedInUserRole &&
                              f.record_active == true &&
                              f.category_id == categoryID
                              select f;
                }

                // If user was already filled, it should not be overwritten here by default message.
                if (userInfoMessage == null)
                {
                    if (records.Count() == 0)
                    {
                        ViewBag.userInfoMessage = "Es wird kein Datensatz angezeigt. Ändern Sie Ihre Such- oder Filtereinstellungen.";
                    }
                    else if (records.Count() > 1)
                    {
                        userInfoMessage = "Es werden " + records.Count() + " Datensätze angezeigt.";
                    }
                    else
                    {
                        userInfoMessage = "Es wird " + records.Count() + " Datensatz angezeigt.";
                    }
                }

                ViewBag.userInfoMessage = userInfoMessage;
                return View(records.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
        }

        /// <summary>
        /// Checks the contents of the database table to avoid errors.
        /// </summary>
        /// <param name="entity">The database table to be checked.</param>
        /// <returns>Returns false if the content is null.</returns>
        private bool checkDatabaseContent(object entity)
        {
            if (entity == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This action result returns the recordverwaltung view. On the recordverwaltung page the 
        /// database content is showing in a table.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Recordverwaltung()
        {
            // Login verification. Function is only accessible for logged in users.
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            // Read out all data records from the database.
            IEnumerable<record> records = from f in myDatabaseEntities.record
                                   select f;

            return View(records.ToList());
        }

        /// <summary>
        /// This action result returns the recorderstellung view. On the recorderstellung page shows 
        /// a form to create a new record on database.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Recorderstellung()
        {
            // Login verification. Function is only accessible for logged in users.
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            ViewBag.category_id = new SelectList(myDatabaseEntities.category, "Id", "name");
            ViewBag.publisher_id = new SelectList(myDatabaseEntities.publisher, "Id", "name");
            ViewBag.licence_id = new SelectList(myDatabaseEntities.licence, "Id", "name");
            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name");
            ViewBag.location_id = new SelectList(myDatabaseEntities.location, "Id", "name");

            //var licenses = myDatabaseEntities.licence.ToList();
            var licenses = new string[3] { "Manual", "Semi", "Auto" };
            ViewBag.licence_descriptions = licenses;     // Send this list to the view

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recorderstellung([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active,location_id")] record record, IEnumerable<HttpPostedFileBase> files)
        {
            // Login verification. Function is only accessible for logged in users.
            if (!checkSession(2))
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

                record.dataset_upload = DateTime.Now;
                record.dataset_modified_date = DateTime.Now;

                myDatabaseEntities.record.Add(record);
                myDatabaseEntities.SaveChanges();

                if (files != null) saveFiles(files, record.Id);
                
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(myDatabaseEntities.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(myDatabaseEntities.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(myDatabaseEntities.licence, "Id", "name", record.licence_id);
            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name");
            ViewBag.location_id = new SelectList(myDatabaseEntities.location, "Id", "name");

            return View(record);
        }

        public ActionResult Recordbearbeitung(int? id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            record record = myDatabaseEntities.record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }

            ViewBag.category_id = new SelectList(myDatabaseEntities.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(myDatabaseEntities.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(myDatabaseEntities.licence, "Id", "name", record.licence_id);
            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name", record.role_id);
            ViewBag.location_id = new SelectList(myDatabaseEntities.location, "Id", "name", record.location_id);
            
            ViewBag.licence_descriptions = myDatabaseEntities.licence.ToList();    // Send this list to the view

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recordbearbeitung([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active,location_id")] record record, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                myDatabaseEntities.Entry(record).State = EntityState.Modified;

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
                
                record.dataset_modified_date = DateTime.Now;                

                if (files != null) saveFiles(files, record.Id);

                myDatabaseEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            

                ViewBag.category_id = new SelectList(myDatabaseEntities.category, "Id", "name");
            ViewBag.publisher_id = new SelectList(myDatabaseEntities.publisher, "Id", "name");
            ViewBag.licence_id = new SelectList(myDatabaseEntities.licence, "Id", "name");
            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name");
            ViewBag.location_id = new SelectList(myDatabaseEntities.location, "Id", "name");

            var licenses = myDatabaseEntities.licence.ToList();
            ViewBag.licence_descriptions = licenses;    // Send this list to the view

            return View();
        }
        
        public ActionResult Recorddetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Datensatz Sichtbarkeit auslesen
            int record_visibility_role = (int)myDatabaseEntities.record.Find(id).role_id;
            int logged_in_user = 4;
            bool logged_in = false;
            if (checkSession(3))
            {
                logged_in_user = Convert.ToInt32(Session["UserRole"], new CultureInfo("de-DE"));
                logged_in = true;
            }

            // Rollenkonzept:
            // Datensatz Sichtbarkeit = 4 (Öffentlichkeit) oder Benutzer angemeldet und Rolle berechtigt Datensatz zu sehen
            if (record_visibility_role > 3 || logged_in == true && record_visibility_role <= logged_in_user)
            {
                record record = myDatabaseEntities.record.Find(id);
                record.licence = myDatabaseEntities.licence.Find(record.licence_id);
                record.publisher = myDatabaseEntities.publisher.Find(record.publisher_id);

                int downloadcount = 0;
                int file_size_conversion_count = 0; // 0 = Bytes

                foreach (file f in record.file)
                {
                    // Downloadanzahl ermitteln.
                    int file_downloadcount = (int)f.download_count;
                    downloadcount = downloadcount + file_downloadcount;

                    // Dateigröße in bessere Darstellung umwandeln.
                    file_size_conversion_count = 0; // 0 = Bytes
                    while (f.file_size > 1024)
                    {
                        f.file_size = f.file_size / 1024;
                        file_size_conversion_count++;
                    }

                    f.file_size = Math.Round((double)f.file_size, 2);
                }

                // Dateigrößenbezeichnung ermitteln.
                ViewData["downloadcount"] = downloadcount;

                string file_size_extension = "Bytes";
                switch (file_size_conversion_count)
                {
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
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            record record = myDatabaseEntities.record.Find(id);

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
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            record record = myDatabaseEntities.record.Find(id);
            myDatabaseEntities.record.Remove(record);
            myDatabaseEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordComment([Bind(Include = "title,text,person_name,bewertung,record_id")] comment comment)
        {
            myDatabaseEntities.comment.Add(comment);

            double avgRating = myDatabaseEntities.comment.Where(c => c.record_id == comment.record_id).Average(c => c.bewertung);

            myDatabaseEntities.record.Find(comment.record_id).rating = (int)Math.Round(avgRating);

            myDatabaseEntities.SaveChangesAsync();
            return RedirectToAction("Recorddetails", new { id = comment.record_id });
        }

        public ActionResult Kategorie()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kategorie([Bind(Include = "parent_id,name,description,icon")] category category)
        {
            if (ModelState.IsValid)
            {
                myDatabaseEntities.category.Add(category);
                myDatabaseEntities.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //GET: OpenDate/Category/5
        public ActionResult Kategoriedetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            category category = myDatabaseEntities.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }          

            return View(category);
        }

        public ActionResult Kategoriebearbeitung(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            category category = myDatabaseEntities.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kategoriebearbeitung([Bind(Include = "parent_id,name,description,icon")] category category)
        {
            if (ModelState.IsValid)
            {
                myDatabaseEntities.Entry(category).State = EntityState.Modified;
                myDatabaseEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Kategorieentfernung(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = myDatabaseEntities.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Kategorieentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult Kategorieentfernungconfirmed(int id)
        {
            category category = myDatabaseEntities.category.Find(id);
            myDatabaseEntities.category.Remove(category);
            myDatabaseEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        /***
         * 
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
                    

                    myDatabaseEntities.file.Add(new Models.file
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

            myDatabaseEntities.SaveChanges();
        }

        public ActionResult DownLoad(string fileName, int recordId)
        {            
            string path = Server.MapPath("~/App_Data/uploads/"+ recordId + "/"+ fileName);
            string mime = MimeMapping.GetMimeMapping(path);
            

            file file = myDatabaseEntities.file.Where(f => f.name == fileName && f.record_id == recordId).First();
            if (file != null) { 
                file.download_count++;
                myDatabaseEntities.SaveChanges();
            }

            return File(path, mime, file.name);
        }

        public ActionResult Dateientfernung(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            file file = myDatabaseEntities.file.Find(id);

            if (file == null)
            {
                return HttpNotFound();
            }

            string path = Server.MapPath("~/App_Data/uploads/" + file.record_id + "/" + file.name);
            System.IO.File.Delete(path);

            int record_id = file.record_id;

            myDatabaseEntities.file.Remove(file);
            myDatabaseEntities.SaveChanges();

            //return RedirectToAction("Recordbearbeitung", new { id = record_id } );
            return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// Check the session. A session exists when a user is logged in.
        /// The required user role is also checked.
        /// </summary>
        /// <param name="requiredUserRole">The required user role.</param>
        /// <returns>Returns true if the user role matches or is lower.</returns>
        private bool checkSession(int requiredUserRole)
        {
            if (Session["UserRole"] != null)
            {
                int loggedInUserRole = Convert.ToInt32(Session["UserRole"], new CultureInfo("de-DE"));

                if (loggedInUserRole <= requiredUserRole)
                {
                    return true;
                }
            }

            return false;
        }
    }
}