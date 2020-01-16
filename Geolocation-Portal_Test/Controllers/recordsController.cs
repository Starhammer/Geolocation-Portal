using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Geolocation_Portal_Test.Models;

namespace Geolocation_Portal_Test.Controllers
{
    public class recordsController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/records
        public IQueryable<record> Getrecord()
        {
            return db.record;
        }

        // GET: api/records/5
        [ResponseType(typeof(record))]
        public IHttpActionResult Getrecord(int id)
        {
            record record = db.record.Find(id);
            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        // PUT: api/records/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putrecord(int id, record record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != record.Id)
            {
                return BadRequest();
            }

            db.Entry(record).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!recordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/records
        [ResponseType(typeof(record))]
        public IHttpActionResult Postrecord(record record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.record.Add(record);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = record.Id }, record);
        }

        // DELETE: api/records/5
        [ResponseType(typeof(record))]
        public IHttpActionResult Deleterecord(int id)
        {
            record record = db.record.Find(id);
            if (record == null)
            {
                return NotFound();
            }

            db.record.Remove(record);
            db.SaveChanges();

            return Ok(record);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool recordExists(int id)
        {
            return db.record.Count(e => e.Id == id) > 0;
        }
    }
}