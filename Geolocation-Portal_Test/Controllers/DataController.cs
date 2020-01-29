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

        private Entities DatabaseEntities = new Entities();

        // GET: api/data/5
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
