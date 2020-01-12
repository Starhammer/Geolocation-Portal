using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Geolocation_Portal_Test.Models;

namespace Geolocation_Portal_Test.Controllers
{
    public class recordsTESTController : Controller
    {
        private Entities db = new Entities();

        // GET: recordsTEST
        public ActionResult Index()
        {
            var record = db.record.Include(r => r.category).Include(r => r.publisher).Include(r => r.licence);
            return View(record.ToList());
        }

        // GET: recordsTEST/Details/5
        public ActionResult Details(int? id)
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

        // GET: recordsTEST/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.category, "Id", "name");
            ViewBag.publisher_id = new SelectList(db.publisher, "Id", "name");
            ViewBag.licence_id = new SelectList(db.licence, "Id", "name");
            return View();
        }

        // POST: recordsTEST/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active")] record record)
        {
            if (ModelState.IsValid)
            {
                db.record.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(db.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(db.licence, "Id", "name", record.licence_id);
            return View(record);
        }

        // GET: recordsTEST/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.category_id = new SelectList(db.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(db.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(db.licence, "Id", "name", record.licence_id);
            return View(record);
        }

        // POST: recordsTEST/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dataset_upload,dataset_modified_date,title,description,category_id,licence_id,publisher_id,rating,role_id,record_active")] record record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.category, "Id", "name", record.category_id);
            ViewBag.publisher_id = new SelectList(db.publisher, "Id", "name", record.publisher_id);
            ViewBag.licence_id = new SelectList(db.licence, "Id", "name", record.licence_id);
            return View(record);
        }

        // GET: recordsTEST/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: recordsTEST/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            record record = db.record.Find(id);
            db.record.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
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
