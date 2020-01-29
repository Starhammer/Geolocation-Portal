using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Geolocation_Portal_Test.Models;

namespace Geolocation_Portal_Test.Controllers.API
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Geolocation_Portal_Test.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<record>("record");
    builder.EntitySet<category>("category"); 
    builder.EntitySet<file>("file"); 
    builder.EntitySet<location>("location"); 
    builder.EntitySet<publisher>("publisher"); 
    builder.EntitySet<licence>("licence"); 
    builder.EntitySet<comment>("comment"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class recordController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/record
        [EnableQuery]
        public IQueryable<record> Getrecord()
        {
            return db.record;
        }

        // GET: odata/record(5)
        [EnableQuery]
        public SingleResult<record> Getrecord([FromODataUri] int key)
        {
            return SingleResult.Create(db.record.Where(record => record.Id == key));
        }

        // PUT: odata/record(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<record> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            record record = db.record.Find(key);
            if (record == null)
            {
                return NotFound();
            }

            patch.Put(record);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!recordExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(record);
        }

        // POST: odata/record
        public IHttpActionResult Post(record record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.record.Add(record);
            db.SaveChanges();

            return Created(record);
        }

        // PATCH: odata/record(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<record> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            record record = db.record.Find(key);
            if (record == null)
            {
                return NotFound();
            }

            patch.Patch(record);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!recordExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(record);
        }

        // DELETE: odata/record(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            record record = db.record.Find(key);
            if (record == null)
            {
                return NotFound();
            }

            db.record.Remove(record);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/record(5)/category
        [EnableQuery]
        public SingleResult<category> Getcategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.record.Where(m => m.Id == key).Select(m => m.category));
        }

        // GET: odata/record(5)/file
        [EnableQuery]
        public IQueryable<file> Getfile([FromODataUri] int key)
        {
            return db.record.Where(m => m.Id == key).SelectMany(m => m.file);
        }

        // GET: odata/record(5)/location
        [EnableQuery]
        public SingleResult<location> Getlocation([FromODataUri] int key)
        {
            return SingleResult.Create(db.record.Where(m => m.Id == key).Select(m => m.location));
        }

        // GET: odata/record(5)/publisher
        [EnableQuery]
        public SingleResult<publisher> Getpublisher([FromODataUri] int key)
        {
            return SingleResult.Create(db.record.Where(m => m.Id == key).Select(m => m.publisher));
        }

        // GET: odata/record(5)/licence
        [EnableQuery]
        public SingleResult<licence> Getlicence([FromODataUri] int key)
        {
            return SingleResult.Create(db.record.Where(m => m.Id == key).Select(m => m.licence));
        }

        // GET: odata/record(5)/comment
        [EnableQuery]
        public IQueryable<comment> Getcomment([FromODataUri] int key)
        {
            return db.record.Where(m => m.Id == key).SelectMany(m => m.comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool recordExists(int key)
        {
            return db.record.Count(e => e.Id == key) > 0;
        }
    }
}
