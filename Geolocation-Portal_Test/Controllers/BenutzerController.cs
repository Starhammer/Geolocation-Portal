using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Geolocation_Portal_Test.Models;

namespace Geolocation_Portal_Test.Controllers
{
    public class BenutzerController : Controller
    {
        /// <summary>
        /// Object of the database tables. Each database table is defined as a model below the entities.
        /// The entity object allows you to select tupels from the database and return a model of each tupel.
        /// </summary>
        private Entities DatabaseEntities = new Entities();

        public LoginModel LoginModel
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// This action result returns the anmelden view. The user can log in with his account via the login mask.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Anmelden()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the anmelden view. After the user has entered his login data and pressed 
        /// the login button, the system checks whether the user exists in the user database.
        /// </summary>
        /// <param name="objUser">The user object, which consists of the user name and password entered in the login screen.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anmelden(user objUser)
        {
            if (objUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                string password = CreateMD5(objUser.password);

                var obj = DatabaseEntities.user.Where(useraccount => string.Equals(useraccount.username, objUser.username) && string.Equals(useraccount.password, password)).FirstOrDefault();
                    
                if (obj != null)
                {
                    Session["UserID"] = obj.Id.ToString();
                    Session["UserName"] = obj.username.ToString();
                    Session["UserRole"] = obj.role_id.ToString();
                    Session["UserFullName"] = obj.first_name.ToString()+" "+ obj.last_name.ToString();
                    return RedirectToAction("WelcomePage");
                }
                else
                {
                    ViewBag.message = "Der eingegebene Benutzername oder das eingegebene Passwort ist nicht korrekt.Bitte versuchen Sie es erneut.";
                    return View("Anmelden");
                }
            }

            return View(new user());
        }

        /// <summary>
        /// Encrypts a string with the MD5 hash procedure.
        /// </summary>
        /// <param name="input">The string to be encrypted.</param>
        /// <returns>Returns an encrypted string as a string.</returns>
        private string CreateMD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// This action result returns the welcome page view. After the login process the 
        /// user is redirected to a welcome page. There the user gets some information 
        /// about his or her options on the portal.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult WelcomePage()
        {
            if (!checkSession(3))
            {
                return RedirectToAction("Anmelden");
            }

            role role = DatabaseEntities.role.Find(Convert.ToInt32(Session["UserRole"]));
            if (role == null)
            {
                return HttpNotFound();
            }
            
            return View(role);
        }

        /// <summary>
        /// This action result returns the abmelden view. When the user initiates the 
        /// logout process, the current session is destroyed.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Abmelden()
        {
            if (!checkSession(3))
            {
                return RedirectToAction("Anmelden");
            }

            Session.Clear();

            return View();
        }

        /// <summary>
        /// This action result returns the profil view. Every logged in user can view his profile data.
        /// This method returns the user data.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Profil()
        {
            if (!checkSession(3))
            {
                return RedirectToAction("Anmelden");
            }

            int userID = Convert.ToInt32(Session["UserID"], new CultureInfo("de-DE"));
            user loggedInUser = DatabaseEntities.user.Find(userID);

            ViewBag.id = loggedInUser.Id;
            ViewBag.role_id = loggedInUser.role_id;
            ViewBag.role = loggedInUser.role.name;
            ViewBag.role_description = loggedInUser.role.description;
            ViewBag.first_name = loggedInUser.first_name;
            ViewBag.last_name = loggedInUser.last_name;
            ViewBag.last_login = loggedInUser.last_login;
            ViewBag.last_passwort_change = loggedInUser.last_password_change;
            ViewBag.account_active = loggedInUser.account_active;
            ViewBag.create_date = loggedInUser.create_date;
            ViewBag.department_id = loggedInUser.department_id;
            ViewBag.login_attempts = loggedInUser.login_attempts;
            ViewBag.password = loggedInUser.password;
            ViewBag.username = loggedInUser.username;

            return View();
        }

        /// <summary>
        /// This action result returns the benutzerverwaltung view. This method 
        /// allows you to manage all registered users.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Benutzerverwaltung()
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            return View(DatabaseEntities.user.ToList());
        }

        /// <summary>
        /// This action result returns the benutzerdetails view. This method shows the 
        /// details of a user. No changes can be made on the mask.
        /// </summary>
        /// <param name="id">An ID is required to search the user inside the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Benutzerdetails(int? id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            user user = DatabaseEntities.user.Find(id);

            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name");
            //ViewBag.department_id = new SelectList(DatabaseEntities.department, "Id", "name"); ToDo: If department id table in DatabaseEntities exists uncomment row.

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        /// <summary>
        /// This action result returns the benutzererstellung view. This method provides 
        /// the roles and departments from the database.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Benutzererstellung()
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name");
            // ViewBag.department_id = new SelectList(DatabaseEntities.department, "Id", "name"); ToDo: If department id table in DatabaseEntities exists uncomment row.

            return View();
        }

        /// <summary>
        /// This action result returns the benutzererstellung view. A user is added to the database.
        /// </summary>
        /// <param name="user">A user object with the data from the form.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Benutzererstellung([Bind(Include = "Id,role_id,department_id,first_name,last_name,username,password,last_password_change,create_date,account_active,login_attempts,last_login")] user user)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                user.create_date = DateTime.Now;
                user.last_password_change = DateTime.Now;
                user.password = CreateMD5(user.password);

                var user_search_data = from userdata in DatabaseEntities.user
                           where userdata.username == user.username
                           select userdata;

                var user_search = user_search_data.ToList();

                if (user_search.Count() == 0)
                {
                    DatabaseEntities.user.Add(user);
                    DatabaseEntities.SaveChanges();
                    return RedirectToAction("Benutzerverwaltung");
                }
                else
                {
                    // ToDo: Benutzer exisitiert bereits. Fehlermeldung ausgeben.
                    return RedirectToAction("Benutzererstellung");
                }
            }

            return View(user);
        }

        /// <summary>
        /// This action result returns the benutzerbearbeitung view. This method provides 
        /// the roles and departments from the database.
        /// </summary>
        /// <param name="id">An ID is required to search the user inside the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Benutzerbearbeitung(int? id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            user user = DatabaseEntities.user.Find(id);

            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name");
            //ViewBag.department_id = new SelectList(DatabaseEntities.department, "Id", "name"); ToDo: If department id table in DatabaseEntities exists uncomment row.

            if (user == null)
            {
                return HttpNotFound();
            }
            
            return View(user);
        }

        /// <summary>
        /// This action result returns the benutzerbearbeitung view. This method provides 
        /// the roles and departments from the database. After the user has been edited, the 
        /// new data is now stored in the database.
        /// </summary>
        /// <param name="user">A user object with the data from the form.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Benutzerbearbeitung([Bind(Include = "Id,role_id,department_id,first_name,last_name,username,password,last_password_change,create_date,account_active,login_attempts,last_login")] user user)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                user.password = CreateMD5(user.password);

                DatabaseEntities.Entry(user).State = EntityState.Modified;
                DatabaseEntities.SaveChanges();
                return RedirectToAction("Benutzerverwaltung");
            }

            return View(user);
        }

        /// <summary>
        /// This action result returns the benutzerentfernung view. Searches for the user to be 
        /// removed in the database and displays it. The user must then confirm the deletion process again.
        /// </summary>
        /// <param name="id">An ID is required to delete the user from the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Benutzerentfernung(int? id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            user user = DatabaseEntities.user.Find(id);

            ViewBag.role_id = new SelectList(DatabaseEntities.role, "Id", "name");
            //ViewBag.department_id = new SelectList(DatabaseEntities.department, "Id", "name"); ToDo: If department id table in DatabaseEntities exists uncomment row.

            if (user == null)
            {
                return HttpNotFound();
            }
            
            return View(user);
        }

        /// <summary>
        /// This action result returns the benutzerentfernung view. As soon as the user 
        /// confirms the deletion, the user is removed from the database.
        /// </summary>
        /// <param name="id">An ID is required to delete the user from the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost, ActionName("Benutzerentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult BenutzerentfernungConfirmed(int id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            user user = DatabaseEntities.user.Find(id);
            DatabaseEntities.user.Remove(user);
            DatabaseEntities.SaveChanges();

            return RedirectToAction("Benutzerverwaltung");
        }

        /// <summary>
        /// This action result returns the rollenverwaltung view. Returns an overview of 
        /// all roles from the database.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Rollenverwaltung()
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            return View(DatabaseEntities.role.ToList());
        }

        /// <summary>
        /// This action result returns the rollendetails view. Recovers the details of a role.
        /// </summary>
        /// <param name="id">An ID is required to find the role in the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Rollendetails(int? id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            role role = DatabaseEntities.role.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        /// <summary>
        /// This action result returns the rollenerstellung view. Allows you to add a role to the database.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Rollenerstellung()
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            return View();
        }

        /// <summary>
        /// This action result returns the rollenerstellung view. Saves the role in the database.
        /// </summary>
        /// <param name="role">A role object with the data from the input form.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rollenerstellung([Bind(Include = "Id,name,description")] role role)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                // A role may only exist once in the database.
                var role_search_data = from roledata in DatabaseEntities.role
                                       where roledata.name == role.name
                                       select roledata;

                var role_search = role_search_data.ToList();

                if (role_search.Count() == 0)
                {
                    DatabaseEntities.role.Add(role);
                    DatabaseEntities.SaveChanges();
                    return RedirectToAction("Rollenverwaltung");
                }
                else
                {
                    ViewBag.userErrorMessage = "Der Name der Rolle existiert bereits.";
                }
            }

            return View(role);
        }

        /// <summary>
        /// This action result returns the rollenbearbeitung view. Returns information about 
        /// the role to be edit the role.
        /// </summary>
        /// <param name="id">An ID is required to find the role in the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Rollenbearbeitung(int? id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            role role = DatabaseEntities.role.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        /// <summary>
        /// This action result returns the rollenbearbeitung view. Once processing is complete, 
        /// the data is now updated in the database.
        /// </summary>
        /// <param name="role">A role object with the data from the input form.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rollenbearbeitung([Bind(Include = "Id,name,description")] role role)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                // A role may only exist once in the database.
                var role_search_data = from roledata in DatabaseEntities.role
                                       where roledata.name == role.name
                                       select roledata;

                var role_search = role_search_data.ToList();

                if (role_search.Count() == 0)
                {
                    DatabaseEntities.Entry(role).State = EntityState.Modified;
                    DatabaseEntities.SaveChanges();
                    return RedirectToAction("Rollenverwaltung");
                }
            }

            return View(role);
        }

        /// <summary>
        /// This action result returns the rollenentfernung view. Provides the role data from 
        /// the database. The user must then confirm the deletion of the role again.
        /// </summary>
        /// <param name="id">An ID is needed to find the role to be deleted in the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Rollenentfernung(int? id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            role role = DatabaseEntities.role.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        /// <summary>
        /// This action result returns the rollenentfernung view. After the user has 
        /// confirmed the deletion, the role is removed from the database.
        /// </summary>
        /// <param name="id">An ID is needed to find the role to be deleted in the database.</param>
        /// <returns>Returns a view to the browser.</returns>
        [HttpPost, ActionName("Rollenentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult RollenentfernungConfirmed(int id)
        {
            if (!checkSession(2))
            {
                return RedirectToAction("Anmelden");
            }

            role role = DatabaseEntities.role.Find(id);
            DatabaseEntities.role.Remove(role);
            DatabaseEntities.SaveChanges();
            return RedirectToAction("Rollenverwaltung");
        }

        /// <summary>
        /// Terminates the connection to the database by destroying the entity object.
        /// </summary>
        /// <param name="disposing">Connection is only destroyed if true.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DatabaseEntities.Dispose();
            }

            base.Dispose(disposing);
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
    }
}