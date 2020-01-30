using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Geolocation_Portal_Test.Models;
using Newtonsoft.Json;
using System.Web;

namespace Geolocation_Portal_Test.Controllers
{
    public class DataController : ApiController
    {
        /// <summary>
        /// Object of the database tables. Each database table is defined as a model below the entities.
        /// The entity object allows you to select tupels from the database and return a model of each tupel.
        /// </summary>
        private Entities DatabaseEntities = new Entities();

        /// <summary>
        /// Determines the path of the geographical file assigned to a specific record. 
        /// </summary>
        /// <param name="id">The record Id to determine the geographical file.</param>
        /// <returns>Returns a json object.</returns>
        [ResponseType(typeof(licence))]
        public async Task<IHttpActionResult> GetGeoData(int id)
        {
            record record = await DatabaseEntities.record.FindAsync(id);
            if (record == null || record.geo_data == false)
            {
                return BadRequest();
            }
            foreach(file file in record.file)
            {
                if (file.name.Contains(".geojson"))
                {
                    string allText = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + id + "/" + file.name));
                    object jsonObject = JsonConvert.DeserializeObject(allText);
                    return Ok(jsonObject);
                }
                else
                {
                    return NotFound();
                }
            }

            return NotFound();
        }
    }
}
