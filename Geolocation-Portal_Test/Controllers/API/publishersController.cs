﻿using System;
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
    builder.EntitySet<publisher>("publishers");
    builder.EntitySet<record>("record"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class publishersController : ODataController
    {
        private Entities db = new Entities();

        // GET: odata/publishers
        [EnableQuery]
        public IQueryable<publisher> Getpublishers()
        {
            return db.publisher;
        }

        // GET: odata/publishers(5)
        [EnableQuery]
        public SingleResult<publisher> Getpublisher([FromODataUri] int key)
        {
            return SingleResult.Create(db.publisher.Where(publisher => publisher.Id == key));
        }

        // PUT: odata/publishers(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<publisher> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            publisher publisher = db.publisher.Find(key);
            if (publisher == null)
            {
                return NotFound();
            }

            patch.Put(publisher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!publisherExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(publisher);
        }

        // POST: odata/publishers
        public IHttpActionResult Post(publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.publisher.Add(publisher);
            db.SaveChanges();

            return Created(publisher);
        }

        // PATCH: odata/publishers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<publisher> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            publisher publisher = db.publisher.Find(key);
            if (publisher == null)
            {
                return NotFound();
            }

            patch.Patch(publisher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!publisherExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(publisher);
        }

        // DELETE: odata/publishers(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            publisher publisher = db.publisher.Find(key);
            if (publisher == null)
            {
                return NotFound();
            }

            db.publisher.Remove(publisher);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/publishers(5)/record
        [EnableQuery]
        public IQueryable<record> Getrecord([FromODataUri] int key)
        {
            return db.publisher.Where(m => m.Id == key).SelectMany(m => m.record);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool publisherExists(int key)
        {
            return db.publisher.Count(e => e.Id == key) > 0;
        }
    }
}
