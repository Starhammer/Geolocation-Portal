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
    builder.EntitySet<comment>("comment");
    builder.EntitySet<record>("record"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class commentController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/comment
        [EnableQuery]
        public IQueryable<comment> Getcomment()
        {
            return db.comment;
        }

        // GET: odata/comment(5)
        [EnableQuery]
        public SingleResult<comment> Getcomment([FromODataUri] int key)
        {
            return SingleResult.Create(db.comment.Where(comment => comment.Id == key));
        }

        // PUT: odata/comment(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<comment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            comment comment = db.comment.Find(key);
            if (comment == null)
            {
                return NotFound();
            }

            patch.Put(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!commentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(comment);
        }

        // POST: odata/comment
        public IHttpActionResult Post(comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.comment.Add(comment);
            db.SaveChanges();

            return Created(comment);
        }

        // PATCH: odata/comment(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<comment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            comment comment = db.comment.Find(key);
            if (comment == null)
            {
                return NotFound();
            }

            patch.Patch(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!commentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(comment);
        }

        // DELETE: odata/comment(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            comment comment = db.comment.Find(key);
            if (comment == null)
            {
                return NotFound();
            }

            db.comment.Remove(comment);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/comment(5)/record
        [EnableQuery]
        public SingleResult<record> Getrecord([FromODataUri] int key)
        {
            return SingleResult.Create(db.comment.Where(m => m.Id == key).Select(m => m.record));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool commentExists(int key)
        {
            return db.comment.Count(e => e.Id == key) > 0;
        }
    }
}
