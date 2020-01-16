using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class GeoDataController : Controller
    {
        // GET: GeoData
        public ActionResult Index(int? id)
        {
            if (id != null){
                ViewBag.FileId = id;
            }
            
            return View();
        }
    }
}