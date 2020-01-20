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
        private Entities db = new Entities();

        public ActionResult Anmelden()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anmelden(user objUser)
        {
            if (ModelState.IsValid)
            {
                string password = CreateMD5(objUser.password);

                using (Entities db = new Entities())
                {
                    var obj = db.user.Where(useraccount => string.Equals(useraccount.username, objUser.username) && string.Equals(useraccount.password, password)).FirstOrDefault();
                    
                    if (obj != null)
                    {
                        Session["UserID"] = obj.Id.ToString();
                        Session["UserName"] = obj.username.ToString();
                        Session["UserRole"] = obj.role_id.ToString();
                        return RedirectToAction("WelcomePage");
                    }
                    else
                    {
                        ViewBag.message = "Der eingegebene Benutzername oder das eingegebene Passwort ist nicht korrekt.Bitte versuchen Sie es erneut.";
                        return View("Anmelden");
                    }
                }
            }

            return View(new user());
        }

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

            role role = db.role.Find(Convert.ToInt32(Session["UserRole"]));
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

            return View(db.user.ToList());
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

            user user = db.user.Find(id);

            ViewBag.role_id = new SelectList(db.role, "Id", "name");
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

            ViewBag.role_id = new SelectList(db.role, "Id", "name");
            //ViewBag.department_id = new SelectList(db.department, "Id", "name"); ToDo: If department id table in db exists uncomment row.

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

                var user_search_data = from userdata in db.user
                           where userdata.username == user.username
                           select userdata;

                var user_search = user_search_data.ToList();

                if (user_search.Count() == 0)
                {
                    db.user.Add(user);
                    db.SaveChanges();
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

            user user = db.user.Find(id);

            ViewBag.role_id = new SelectList(db.role, "Id", "name");
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

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
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
            
            user user = db.user.Find(id);

            ViewBag.role_id = new SelectList(db.role, "Id", "name");
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

            user user = db.user.Find(id);
            db.user.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Benutzerverwaltung");
        }

        public ActionResult Rollenverwaltung()
        {
            if (!checkSession())
            {
                return RedirectToAction("Anmelden");
            }

            return View(db.role.ToList());
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

            role role = db.role.Find(id);

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
                db.role.Add(role);
                db.SaveChanges();
                return RedirectToAction("Rollenverwaltung");
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

            role role = db.role.Find(id);

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
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Rollenverwaltung");
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

            role role = db.role.Find(id);

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

            role role = db.role.Find(id);
            db.role.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Rollenverwaltung");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
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