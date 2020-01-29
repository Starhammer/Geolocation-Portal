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
    builder.EntitySet<file>("file");
    builder.EntitySet<record>("record"); 
    builder.EntitySet<searchtag>("searchtag"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class fileController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/file
        [EnableQuery]
        public IQueryable<file> Getfile()
        {
            return db.file;
        }

        // GET: odata/file(5)
        [EnableQuery]
        public SingleResult<file> Getfile([FromODataUri] int key)
        {
            return SingleResult.Create(db.file.Where(file => file.Id == key));
        }

        // PUT: odata/file(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<file> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            file file = db.file.Find(key);
            if (file == null)
            {
                return NotFound();
            }

            patch.Put(file);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!fileExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(file);
        }

        // POST: odata/file
        public IHttpActionResult Post(file file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.file.Add(file);
            db.SaveChanges();

            return Created(file);
        }

        // PATCH: odata/file(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<file> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            file file = db.file.Find(key);
            if (file == null)
            {
                return NotFound();
            }

            patch.Patch(file);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!fileExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(file);
        }

        // DELETE: odata/file(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            file file = db.file.Find(key);
            if (file == null)
            {
                return NotFound();
            }

            db.file.Remove(file);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/file(5)/record
        [EnableQuery]
        public SingleResult<record> Getrecord([FromODataUri] int key)
        {
            return SingleResult.Create(db.file.Where(m => m.Id == key).Select(m => m.record));
        }

        // GET: odata/file(5)/searchtag
        [EnableQuery]
        public SingleResult<searchtag> Getsearchtag([FromODataUri] int key)
        {
            return SingleResult.Create(db.file.Where(m => m.Id == key).Select(m => m.searchtag));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool fileExists(int key)
        {
            return db.file.Count(e => e.Id == key) > 0;
        }
    }
}
