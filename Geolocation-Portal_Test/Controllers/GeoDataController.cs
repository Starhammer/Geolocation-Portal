using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            if (id != null)
            {
                // ToDo: Session in Json Variable speichern.
                //Json recordList = Session["MapRecordShoppingCart"];

                //Session["MapRecordShoppingCart"] = "";

                if (Session["MapRecordShoppingCart"] != null)
                {
                    // Session Variable an View übergeben.
                    int[] recordIDs = { 4003, 5004, 7002 };

                    ViewBag.FileId = recordIDs;
                }
                else
                {
                    int[] recordIDs = { Convert.ToInt32(id, new CultureInfo("de-DE")) };

                    ViewBag.FileId = recordIDs;
                }
            }
            
            return View();
        }
    }
}