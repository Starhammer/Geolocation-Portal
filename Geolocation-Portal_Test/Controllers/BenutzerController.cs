using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
                using (Entities db = new Entities())
                {
                    var obj = db.user.Where(a => string.Equals(a.username, objUser.username) && string.Equals(a.password, objUser.password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.Id.ToString();
                        Session["UserName"] = obj.username.ToString();
                        Session["UserRole"] = obj.role_id.ToString();
                        return RedirectToAction("WelcomePage");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult WelcomePage()
        {
            if (Session["UserID"] != null)
            {
                role role = db.role.Find(Convert.ToInt32(Session["UserRole"]));
                if (role == null)
                {
                    return HttpNotFound();
                }
                return View(role);
            }
            else
            {
                return RedirectToAction("Anmelden");
            }
        }

        public ActionResult Abmelden()
        {
            Session.Clear();
            return View();
        }

        // GET: user/Details/5
        public ActionResult Benutzerdetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.user.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Benutzerverwaltung()
        {
            return View(db.user.ToList());
        }

        public ActionResult Rollenverwaltung()
        {
            return View(db.role.ToList());
        }

        public ActionResult Benutzererstellung()
        {
            return View();
        }
        
        // POST: user/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Benutzererstellung([Bind(Include = "Id,role_id,department_id,first_name,last_name,username,password,last_password_change,create_date,account_active,login_attempts,last_login")] user user)
        {
            if (ModelState.IsValid)
            {
                db.user.Add(user);
                db.SaveChanges();
                return RedirectToAction("Benutzerverwaltung");
            }

            return View(user);
        }

        // GET: user/Edit/5
        public ActionResult Benutzerbearbeitung(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.user.Find(id);
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
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Benutzerverwaltung");
            }
            return View(user);
        }

        // GET: user/Delete/5
        public ActionResult Benutzerentfernung(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.user.Find(id);
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
            user user = db.user.Find(id);
            db.user.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Benutzerverwaltung");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}