﻿using Geolocation_Portal_Test.Models;
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
        private Entities DatabaseEntities = new Entities();

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

            // parameter verification cop to avoid errors.
            if (searchTerm == null)
            {
                // null is required to avoid searching.
                searchTerm = Request["search"];
            }

            // Check all required database tables for their contents to avoid errors.
            if (checkDatabaseContent(DatabaseEntities.record) &&
                checkDatabaseContent(DatabaseEntities.category))
            {
                // Load the categories to display the filter function. The category filter is implemented using icons defined in the database.
                ViewBag.category = (List<category>)DatabaseEntities.category.ToList();
                // Allows the user to reset the record restriction.
                ViewBag.resetRestriction = true;

                string userInfoMessage = null;

                // Load the records to return them to the view.
                // The variable is null at this position because the restriction parameter still needs to be checked.
                IEnumerable<record> records = null;

                // Check record display authorization using the role ID. Role concept 4 = extern user.
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
                                records = from f in DatabaseEntities.record
                                          where f.role_id >= loggedInUserRole
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.all_active_records):
                                // Read out all "active" data records from the database.
                                records = from f in DatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          select f;
                                ViewBag.resetRestriction = false;   // If "all" data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.geo_records):
                                // Read out all "geo" data records from the database.
                                records = from f in DatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true &&
                                          f.geo_data == true
                                          select f;
                                break;
                            case (restriction_enum.dia_records):
                                // Read out all "diagram" data records from the database.
                                records = from f in DatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true &&
                                          f.dia_data == true
                                          select f;
                                break;
                            case (restriction_enum.sort_alpha_asc):
                                // Sort all records from the database.
                                records = from f in DatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          orderby f.title
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.sort_alpha_desc):
                                // Read out all "diagram" data records from the database.
                                records = from f in DatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          orderby f.title descending
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.sort_date_asc):
                                // Read out all "diagram" data records from the database.
                                records = from f in DatabaseEntities.record
                                          where f.role_id >= loggedInUserRole &&
                                          f.record_active == true
                                          orderby f.dataset_modified_date
                                          select f;
                                ViewBag.resetRestriction = false;   // If all data records are already displayed, the reset function should not be displayed.
                                break;
                            case (restriction_enum.sort_date_desc):
                                // Read out all "diagram" data records from the database.
                                records = from f in DatabaseEntities.record
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
                        records = from f in DatabaseEntities.record
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
                    category category = DatabaseEntities.category.Find(categoryID);
                    if (category == null)
                    {
                        return HttpNotFound();
                    }

                    records = from f in DatabaseEntities.record
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
        /// <param name="entity">A database table object to check the content.</param>
        /// <returns>Returns false if the content is null.</returns>
        private bool checkDatabaseContent(object entity)
        {
            // parameter verification cop to avoid errors.
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
            IEnumerable<record> records = from f in DatabaseEntities.record
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

            ViewBag.category_id = new SelectList(DatabaseEntities.category, "Id", "name");
            ViewBag.publisher_id = new SelectList(DatabaseEntities.publisher, "Id", "name");
            ViewBag.licence_id = new SelectList(DatabaseEntities.licence, "Id", "name");
            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name", 4);
            ViewBag.location_id = new SelectList(DatabaseEntities.location, "Id", "name");

            var licenceItems = new SelectList(DatabaseEntities.licence, "Id", "description");

            List<string> licence_descriptions = new List<string>();
            
            foreach (var licenceItem in licenceItems.ToList())
            {
                licence_descriptions.Add(licenceItem.Text);
            }

            ViewData["licence_description_list"] = licence_descriptions.ToArray();

            var roleItems = new SelectList(DatabaseEntities.role, "Id", "description");

            List<string> role_descriptions = new List<string>();

            foreach (var roleItem in roleItems.ToList())
            {
                role_descriptions.Add(roleItem.Text);
            }

            ViewData["role_description_list"] = role_descriptions.ToArray();

            return View();
        }

        /// <summary>
        /// This action result returns the recorderstellung view. This action result is called after the user submits the Add Record form.
        /// </summary>
        /// <param name="record">The record object that is to be stored in the database.</param>
        /// <param name="files">A list with information about the files that the user has attached to the record.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recorderstellung([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active,location_id")] record record, IEnumerable<HttpPostedFileBase> files)
        {
            // parameter verification cop to avoid errors.
            if (record == null || files == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

                DatabaseEntities.record.Add(record);
                DatabaseEntities.SaveChanges();

                if (files != null) saveFiles(files, record.Id);
                
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(DatabaseEntities.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(DatabaseEntities.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(DatabaseEntities.licence, "Id", "name", record.licence_id);
            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name");
            ViewBag.location_id = new SelectList(DatabaseEntities.location, "Id", "name");

            var licenceItems = new SelectList(DatabaseEntities.licence, "Id", "description");

            List<string> licence_descriptions = new List<string>();

            foreach (var licenceItem in licenceItems.ToList())
            {
                licence_descriptions.Add(licenceItem.Text);
            }

            ViewData["licence_description_list"] = licence_descriptions.ToArray();

            var roleItems = new SelectList(DatabaseEntities.role, "Id", "description");

            List<string> role_descriptions = new List<string>();

            foreach (var roleItem in roleItems.ToList())
            {
                role_descriptions.Add(roleItem.Text);
            }

            ViewData["role_description_list"] = role_descriptions.ToArray();

            return View(record);
        }

        /// <summary>
        /// This action result returns the recordbearbeitung view. This method allows the subsequent editing of a record.
        /// </summary>
        /// <param name="id">An ID is required to load an existing record from the database for editing.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Recordbearbeitung(int? id)
        {
            // parameter verification cop to avoid errors.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            record record = DatabaseEntities.record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }

            ViewBag.category_id = new SelectList(DatabaseEntities.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(DatabaseEntities.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(DatabaseEntities.licence, "Id", "name", record.licence_id);
            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name", record.role_id);
            ViewBag.location_id = new SelectList(DatabaseEntities.location, "Id", "name", record.location_id);

            var licenceItems = new SelectList(DatabaseEntities.licence, "Id", "description");

            List<string> licence_descriptions = new List<string>();

            foreach (var licenceItem in licenceItems.ToList())
            {
                licence_descriptions.Add(licenceItem.Text);
            }

            ViewData["licence_description_list"] = licence_descriptions.ToArray();

            var roleItems = new SelectList(DatabaseEntities.role, "Id", "description");

            List<string> role_descriptions = new List<string>();

            foreach (var roleItem in roleItems.ToList())
            {
                role_descriptions.Add(roleItem.Text);
            }

            ViewData["role_description_list"] = role_descriptions.ToArray();

            return View(record);
        }

        /// <summary>
        /// This action result returns the recordbearbeitung view. This method saves the changes to the record in the database.
        /// </summary>
        /// <param name="record">The record to be saved.</param>
        /// <param name="files">The files that are attached to the record.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recordbearbeitung([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active,location_id")] record record, IEnumerable<HttpPostedFileBase> files)
        {
            // parameter verification cop to avoid errors.
            if (record == null || files == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                DatabaseEntities.Entry(record).State = EntityState.Modified;

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

                DatabaseEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            

            ViewBag.category_id = new SelectList(DatabaseEntities.category, "Id", "name");
            ViewBag.publisher_id = new SelectList(DatabaseEntities.publisher, "Id", "name");
            ViewBag.licence_id = new SelectList(DatabaseEntities.licence, "Id", "name");
            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name");
            ViewBag.location_id = new SelectList(DatabaseEntities.location, "Id", "name");

            var licenceItems = new SelectList(DatabaseEntities.licence, "Id", "description");

            List<string> licence_descriptions = new List<string>();

            foreach (var licenceItem in licenceItems.ToList())
            {
                licence_descriptions.Add(licenceItem.Text);
            }

            ViewData["licence_description_list"] = licence_descriptions.ToArray();

            var roleItems = new SelectList(DatabaseEntities.role, "Id", "description");

            List<string> role_descriptions = new List<string>();

            foreach (var roleItem in roleItems.ToList())
            {
                role_descriptions.Add(roleItem.Text);
            }

            ViewData["role_description_list"] = role_descriptions.ToArray();

            return View();
        }

        /// <summary>
        /// This action result returns the recorddetails view. Displays the record without allowing 
        /// the user to accidentally make changes.
        /// </summary>
        /// <param name="id">An ID is required to view the details.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Recorddetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Datensatz Sichtbarkeit auslesen
            int record_visibility_role = (int)DatabaseEntities.record.Find(id).role_id;
            int logged_in_user = 4;
            bool logged_in = false;
            if (checkSession(record_visibility_role))
            {
                logged_in_user = Convert.ToInt32(Session["UserRole"], new CultureInfo("de-DE"));
                logged_in = true;
            }

            // Rollenkonzept:
            // Datensatz Sichtbarkeit = 4 (Öffentlichkeit) oder Benutzer angemeldet und Rolle berechtigt Datensatz zu sehen
            if (record_visibility_role > 3 || logged_in == true && record_visibility_role >= logged_in_user)
            {
                record record = DatabaseEntities.record.Find(id);
                record.licence = DatabaseEntities.licence.Find(record.licence_id);
                record.publisher = DatabaseEntities.publisher.Find(record.publisher_id);

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

        /// <summary>
        /// This action result returns the recordentfernung view. This method displays a record that the 
        /// user wants to delete. However, the user must confirm the deletion again.
        /// </summary>
        /// <param name="id">The ID of the data file to be deleted</param>
        /// <returns>Returns a view to the browser.</returns>
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

            record record = DatabaseEntities.record.Find(id);

            if (record == null)
            {
                return HttpNotFound();
            }

            return View(record);
        }

        /// <summary>
        /// This action result returns the recordentfernung view. This method deletes a record from the database.
        /// </summary>
        /// <param name="id">The ID of the data file to be deleted</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost, ActionName("Recordentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult RecordentfernungConfirmed(int id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }

            record record = DatabaseEntities.record.Find(id);

            DatabaseEntities.comment.RemoveRange(record.comment);

            Directory.Delete(Server.MapPath("~/App_Data/uploads/" +id),true);

            DatabaseEntities.file.RemoveRange(record.file);
            DatabaseEntities.record.Remove(record);

            DatabaseEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This action result returns the record comment view. This method allows you to save a comment to a record.
        /// </summary>
        /// <param name="comment">The comment to be stored in the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordComment([Bind(Include = "title,text,person_name,bewertung,record_id")] comment comment)
        {
            // parameter verification cop to avoid errors.
            if (comment == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Add the new comment to the database.
            DatabaseEntities.comment.Add(comment);

            // Save comment to calculate the average.
            DatabaseEntities.SaveChanges();

            // Average calculation if comments already exist for the data set.
            double avgRating = DatabaseEntities.comment.Where(c => c.record_id == comment.record_id).Average(c => c.bewertung);
            avgRating = Math.Round(avgRating);

            // Add a new average value to the data set so that it does not always have to be calculated (performance).
            DatabaseEntities.record.Find(comment.record_id).rating = (int)avgRating;

            DatabaseEntities.SaveChangesAsync();

            return RedirectToAction("Recorddetails", new { id = comment.record_id });
        }

        /// <summary>
        /// This action result returns the kategorieverwaltung view. The administrator and key users have the possibility 
        /// to manage the categories from the database.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Kategorieverwaltung()
        {
            return View(DatabaseEntities.category.ToList());
        }

        /// <summary>
        /// This action result returns the kategorieerstellung view. The administrator and key users have the possibility 
        /// to save a new category in the database via a form.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Kategorieerstellung()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the kategorieerstellung view. The administrator and key users have the possibility 
        /// to save a new category in the database via a form.
        /// This page saves the changes in the database after clicking Save.
        /// </summary>
        /// <param name="category">A category object is required to make its values available for editing.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kategorieerstellung([Bind(Include = "parent_id,name,description,icon")] category category)
        {
            if (ModelState.IsValid)
            {
                DatabaseEntities.category.Add(category);
                DatabaseEntities.SaveChanges();
                return RedirectToAction("Kategorieverwaltung");
            }

            return View(category);
        }

        /// <summary>
        /// This action result returns the kategoriedetails view. Displays the category without allowing 
        /// the user to accidentally make changes.
        /// </summary>
        /// <param name="id">An ID is required to view the details.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Kategoriedetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            category category = DatabaseEntities.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }          

            return View(category);
        }

        /// <summary>
        /// This action result returns the kategoriedetails view. Allows you to change a category via a form.
        /// </summary>
        /// <param name="id">An ID is required to view the details.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Kategoriebearbeitung(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            category category = DatabaseEntities.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        /// <summary>
        /// This action result returns the kategoriebearbeitung view. Will be executed after the editing is saved. The changes are saved in the database.
        /// </summary>
        /// <param name="category">A category object is required to make its values available for editing.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kategoriebearbeitung([Bind(Include = "Id,parent_id,name,description,icon")] category category)
        {
            if (ModelState.IsValid)
            {
                DatabaseEntities.Entry(category).State = EntityState.Modified;
                DatabaseEntities.SaveChanges();
                return RedirectToAction("Kategorieverwaltung");
            }
            return View(category);
        }

        /// <summary>
        /// This action result returns the kategorieentfernung view. Shows the category to accept again before deletion.
        /// </summary>
        /// <param name="id">An ID is required to delete a category.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Kategorieentfernung(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = DatabaseEntities.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        /// <summary>
        /// This method is executed after the delete button is pressed.
        /// </summary>
        /// <param name="id">An ID is required to delete a category.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost, ActionName("Kategorieentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult Kategorieentfernungconfirmed(int id)
        {
            category category = DatabaseEntities.category.Find(id);
            DatabaseEntities.category.Remove(category);
            DatabaseEntities.SaveChanges();
            return RedirectToAction("Kategorieverwaltung");
        }

        /// <summary>
        /// Stores the records associated with the record on the server. For each file, a reference is stored 
        /// in the database so that the files can be found again later.
        /// </summary>
        /// <param name="files">A list of files that belong to a record.</param>
        /// <param name="recordID">The ID of the record to which the files belong.</param>
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
                    
                    DatabaseEntities.file.Add(new Models.file
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

            DatabaseEntities.SaveChanges();
        }

        /// <summary>
        /// This method determines the file extension of any file.
        /// </summary>
        /// <param name="fileName">The file name from which the file extension should be found out.</param>
        /// <returns>Returns the file extension of a file.</returns>
        private string getFileIcon(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension.Remove(0, 1);
        }

        /// <summary>
        /// Allows you to download files from the server. The number of file downloads is counted up and stored in the database.
        /// </summary>
        /// <param name="fileName">A filename is required to find the file in the download path.</param>
        /// <param name="recordId">An ID of the record is required to find the download path.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult DownLoad(string fileName, int recordId)
        {
            if (fileName == null || recordId == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string path = Server.MapPath("~/App_Data/uploads/"+ recordId + "/"+ fileName);
            string mime = MimeMapping.GetMimeMapping(path);
            
            file file = DatabaseEntities.file.Where(f => f.name == fileName && f.record_id == recordId).First();
            if (file != null) { 
                file.download_count++;
                DatabaseEntities.SaveChanges();
            }

            return File(path, mime, file.name);
        }

        /// <summary>
        /// Removes a file from the server directory and deletes the reference in the database. The file no longer exists.
        /// </summary>
        /// <param name="id">The file ID to find the file metadata in the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Dateientfernung(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            file file = DatabaseEntities.file.Find(id);

            if (file == null)
            {
                return HttpNotFound();
            }
                        
            DatabaseEntities.file.Remove(file);
            deleteFile(file);
            DatabaseEntities.SaveChanges();

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
            if (requiredUserRole == null)
            {
                return false;
            }

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

        /// <summary>
        /// Deletes file on Server. Does not delete DB-entry of file.
        /// </summary>
        /// <param name="file">File to be deleted</param>
        private void deleteFile(file file)
        {
            string path = Server.MapPath("~/App_Data/uploads/" + file.record_id + "/" + file.name);
            System.IO.File.Delete(path);
        }
    }
}