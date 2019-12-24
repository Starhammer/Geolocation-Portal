using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class BenutzerController : Controller
    {
        // GET: Benutzer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Abmelden()
        {
            return View();
        }

        public ActionResult Verwalten()
        {
            return View();
        }
    }
}