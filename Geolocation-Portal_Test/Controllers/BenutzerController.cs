using System;
using System.Data;
using System.Data.Entity;
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
        private Entities myDatabaseEntities = new Entities();

        /// <summary>
        /// This action result returns the anmelden view. The user can log in with his account via the login mask.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Anmelden()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the anmelden view.
        /// </summary>
        /// <param name="objUser"></param>
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

                var obj = myDatabaseEntities.user.Where(useraccount => string.Equals(useraccount.username, objUser.username) && string.Equals(useraccount.password, password)).FirstOrDefault();
                    
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

        public ActionResult WelcomePage()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            role role = myDatabaseEntities.role.Find(Convert.ToInt32(Session["UserRole"]));
            if (role == null)
            {
                return HttpNotFound();
            }
            
            return View(role);
        }

        public ActionResult Abmelden()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            Session.Clear();
            return View();
        }

        public ActionResult Profil()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            return View();
        }

        public ActionResult Benutzerverwaltung()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            return View(myDatabaseEntities.user.ToList());
        }

        // GET: user/Details/5
        public ActionResult Benutzerdetails(int? id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            user user = myDatabaseEntities.user.Find(id);

            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name");
            //ViewBag.department_id = new SelectList(db.department, "Id", "name"); ToDo: If department id table in db exists uncomment row.

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public ActionResult Benutzererstellung()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name");
            // ViewBag.department_id = new SelectList(db.department, "Id", "name"); ToDo: If department id table in db exists uncomment row.

            return View();
        }

        // POST: user/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Benutzererstellung([Bind(Include = "Id,role_id,department_id,first_name,last_name,username,password,last_password_change,create_date,account_active,login_attempts,last_login")] user user)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                user.create_date = DateTime.Now;
                user.last_password_change = DateTime.Now;
                user.password = CreateMD5(user.password);

                var user_search_data = from userdata in myDatabaseEntities.user
                           where userdata.username == user.username
                           select userdata;

                var user_search = user_search_data.ToList();

                if (user_search.Count() == 0)
                {
                    myDatabaseEntities.user.Add(user);
                    myDatabaseEntities.SaveChanges();
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

        // GET: user/Edit/5
        public ActionResult Benutzerbearbeitung(int? id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            user user = myDatabaseEntities.user.Find(id);

            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name");
            //ViewBag.department_id = new SelectList(db.department, "Id", "name"); ToDo: If department id table in db exists uncomment row.

            if (user == null)
            {
                return HttpNotFound();
            }
            
            return View(user);
        }

        // POST: user/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Benutzerbearbeitung([Bind(Include = "Id,role_id,department_id,first_name,last_name,username,password,last_password_change,create_date,account_active,login_attempts,last_login")] user user)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                user.password = CreateMD5(user.password);

                myDatabaseEntities.Entry(user).State = EntityState.Modified;
                myDatabaseEntities.SaveChanges();
                return RedirectToAction("Benutzerverwaltung");
            }

            return View(user);
        }

        // GET: user/Delete/5
        public ActionResult Benutzerentfernung(int? id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            user user = myDatabaseEntities.user.Find(id);

            ViewBag.role_id = new SelectList(myDatabaseEntities.role, "Id", "name");
            //ViewBag.department_id = new SelectList(db.department, "Id", "name"); ToDo: If department id table in db exists uncomment row.

            if (user == null)
            {
                return HttpNotFound();
            }
            
            return View(user);
        }

        // POST: user/Delete/5
        [HttpPost, ActionName("Benutzerentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult BenutzerentfernungConfirmed(int id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            user user = myDatabaseEntities.user.Find(id);
            myDatabaseEntities.user.Remove(user);
            myDatabaseEntities.SaveChanges();
            return RedirectToAction("Benutzerverwaltung");
        }

        public ActionResult Rollenverwaltung()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            return View(myDatabaseEntities.role.ToList());
        }// GET: user/Details/5
        public ActionResult Rollendetails(int? id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            role role = myDatabaseEntities.role.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        public ActionResult Rollenerstellung()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            return View();
        }

        // POST: user/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rollenerstellung([Bind(Include = "Id,name,description")] role role)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                // A role may only exist once in the database.
                var role_search_data = from roledata in myDatabaseEntities.role
                                       where roledata.name == role.name
                                       select roledata;

                var role_search = role_search_data.ToList();

                if (role_search.Count() == 0)
                {
                    myDatabaseEntities.role.Add(role);
                    myDatabaseEntities.SaveChanges();
                    return RedirectToAction("Rollenverwaltung");
                }
                else
                {
                    ViewBag.userErrorMessage = "Der Name der Rolle existiert bereits.";
                }
            }

            return View(role);
        }

        // GET: user/Edit/5
        public ActionResult Rollenbearbeitung(int? id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            role role = myDatabaseEntities.role.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        // POST: user/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rollenbearbeitung([Bind(Include = "Id,name,description")] role role)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (ModelState.IsValid)
            {
                // A role may only exist once in the database.
                var role_search_data = from roledata in myDatabaseEntities.role
                                       where roledata.name == role.name
                                       select roledata;

                var role_search = role_search_data.ToList();

                if (role_search.Count() == 0)
                {
                    myDatabaseEntities.Entry(role).State = EntityState.Modified;
                    myDatabaseEntities.SaveChanges();
                    return RedirectToAction("Rollenverwaltung");
                }
            }

            return View(role);
        }

        // GET: user/Delete/5
        public ActionResult Rollenentfernung(int? id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            role role = myDatabaseEntities.role.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        // POST: user/Delete/5
        [HttpPost, ActionName("Rollenentfernung")]
        [ValidateAntiForgeryToken]
        public ActionResult RollenentfernungConfirmed(int id)
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            role role = myDatabaseEntities.role.Find(id);
            myDatabaseEntities.role.Remove(role);
            myDatabaseEntities.SaveChanges();
            return RedirectToAction("Rollenverwaltung");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                myDatabaseEntities.Dispose();
            }

            base.Dispose(disposing);
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
    }
}