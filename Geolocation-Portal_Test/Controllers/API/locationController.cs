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
    builder.EntitySet<location>("location");
    builder.EntitySet<record>("record"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class locationController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/location
        [EnableQuery]
        public IQueryable<location> Getlocation()
        {
            return db.location;
        }

        // GET: odata/location(5)
        [EnableQuery]
        public SingleResult<location> Getlocation([FromODataUri] int key)
        {
            return SingleResult.Create(db.location.Where(location => location.Id == key));
        }

        // PUT: odata/location(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<location> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            location location = db.location.Find(key);
            if (location == null)
            {
                return NotFound();
            }

            patch.Put(location);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!locationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(location);
        }

        // POST: odata/location
        public IHttpActionResult Post(location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.location.Add(location);
            db.SaveChanges();

            return Created(location);
        }

        // PATCH: odata/location(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<location> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            location location = db.location.Find(key);
            if (location == null)
            {
                return NotFound();
            }

            patch.Patch(location);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!locationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(location);
        }

        // DELETE: odata/location(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            location location = db.location.Find(key);
            if (location == null)
            {
                return NotFound();
            }

            db.location.Remove(location);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/location(5)/record
        [EnableQuery]
        public IQueryable<record> Getrecord([FromODataUri] int key)
        {
            return db.location.Where(m => m.Id == key).SelectMany(m => m.record);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool locationExists(int key)
        {
            return db.location.Count(e => e.Id == key) > 0;
        }
    }
}
