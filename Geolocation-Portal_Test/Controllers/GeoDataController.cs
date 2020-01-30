using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class GeoDataController : Controller
    {
        /// <summary>
        /// This action result returns the kategoriebearbeitung view. Checks whether one or more data sets should be displayed on the map.
        /// </summary>
        /// <param name="id">An ID is required to load the geojson file from a record.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Index(int? id)
        {
            // parameter verification cop to avoid errors.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.FileId = id;

            return View();
        }
    }
}