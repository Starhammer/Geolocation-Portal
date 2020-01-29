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
    builder.EntitySet<licence>("licence");
    builder.EntitySet<record>("record"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class licenceController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/licence
        [EnableQuery]
        public IQueryable<licence> Getlicence()
        {
            return db.licence;
        }

        // GET: odata/licence(5)
        [EnableQuery]
        public SingleResult<licence> Getlicence([FromODataUri] int key)
        {
            return SingleResult.Create(db.licence.Where(licence => licence.Id == key));
        }

        // PUT: odata/licence(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<licence> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            licence licence = db.licence.Find(key);
            if (licence == null)
            {
                return NotFound();
            }

            patch.Put(licence);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!licenceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(licence);
        }

        // POST: odata/licence
        public IHttpActionResult Post(licence licence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.licence.Add(licence);
            db.SaveChanges();

            return Created(licence);
        }

        // PATCH: odata/licence(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<licence> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            licence licence = db.licence.Find(key);
            if (licence == null)
            {
                return NotFound();
            }

            patch.Patch(licence);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!licenceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(licence);
        }

        // DELETE: odata/licence(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            licence licence = db.licence.Find(key);
            if (licence == null)
            {
                return NotFound();
            }

            db.licence.Remove(licence);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/licence(5)/record
        [EnableQuery]
        public IQueryable<record> Getrecord([FromODataUri] int key)
        {
            return db.licence.Where(m => m.Id == key).SelectMany(m => m.record);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool licenceExists(int key)
        {
            return db.licence.Count(e => e.Id == key) > 0;
        }
    }
}
