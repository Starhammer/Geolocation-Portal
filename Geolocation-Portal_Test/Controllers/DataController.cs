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

        private Entities db = new Entities();

        // GET: api/data/5
        [ResponseType(typeof(licence))]
        public async Task<IHttpActionResult> GetGeoData(int id)
        {
            file file = await db.file.FindAsync(id);
            if (file == null || file.map_data == false)
            {
                return NotFound();
            }

            string allText = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/uploads/"+file.record.Id+"/"+file.name));
            
            object jsonObject = JsonConvert.DeserializeObject(allText);
            return Ok(jsonObject);
        }
    }
}
