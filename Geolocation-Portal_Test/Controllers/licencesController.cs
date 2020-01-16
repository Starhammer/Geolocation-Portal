using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Geolocation_Portal_Test.Models;

namespace Geolocation_Portal_Test.Controllers
{
    public class licencesController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/licences
        public IQueryable<licence> Getlicence()
        {
            return db.licence;
        }

        // GET: api/licences/5
        [ResponseType(typeof(licence))]
        public async Task<IHttpActionResult> Getlicence(int id)
        {
            licence licence = await db.licence.FindAsync(id);
            if (licence == null)
            {
                return NotFound();
            }

            return Ok(licence);
        }

        // PUT: api/licences/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putlicence(int id, licence licence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != licence.Id)
            {
                return BadRequest();
            }

            db.Entry(licence).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!licenceExists(id))
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

        // POST: api/licences
        [ResponseType(typeof(licence))]
        public async Task<IHttpActionResult> Postlicence(licence licence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.licence.Add(licence);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = licence.Id }, licence);
        }

        // DELETE: api/licences/5
        [ResponseType(typeof(licence))]
        public async Task<IHttpActionResult> Deletelicence(int id)
        {
            licence licence = await db.licence.FindAsync(id);
            if (licence == null)
            {
                return NotFound();
            }

            db.licence.Remove(licence);
            await db.SaveChangesAsync();

            return Ok(licence);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool licenceExists(int id)
        {
            return db.licence.Count(e => e.Id == id) > 0;
        }
    }
}