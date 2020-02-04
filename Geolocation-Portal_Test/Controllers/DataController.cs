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
using Newtonsoft.Json.Linq;
using System.IO;

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
        /// Determines the geographical file assigned to a lot of specific record. 
        /// </summary>
        /// <param name="id">An array of record ids</param>
        /// <returns>Returns a json object.</returns>
        public async Task<IHttpActionResult> GetGeoData(string id)
        {
            int[] ids = id.Split(',').Select(int.Parse).ToArray();

            string firstElement = @"{ 'type': 'FeatureCollection', 'features': [] }";

            JObject geojson = JObject.Parse(firstElement);

            JArray geoJsonFiles = (JArray)geojson["features"];

            foreach (int record_id in ids)
            {
                record record = await DatabaseEntities.record.FindAsync(record_id);

                if (record == null || record.geo_data == false)
                {
                    return BadRequest();
                }

                foreach (file file in record.file)
                {
                    if (file.name.Contains(".geojson"))
                    {
                        JObject jsonObject = JObject.Parse(File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + record_id + "/" + file.name)));

                        JArray dataOfJson = (JArray)jsonObject.SelectToken("features");
                        
                        foreach (JObject innerData in dataOfJson)
                        {
                            geoJsonFiles.Add(innerData);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            object geoJsonFilesObject = JsonConvert.DeserializeObject(geojson.ToString());

            return Ok(geoJsonFilesObject);
        }
    }
}
