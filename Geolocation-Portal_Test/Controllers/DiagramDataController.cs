using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class DiagramDataController : Controller
    {
        // GET: Diagramm
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.FileId = id;
            }

            return View();
        }

        /**
         * Zeigt einen Datensatz als Diagramm an.
         */
        public ActionResult Diagramm()
        {
            return View();
        }

        /**
         * Erstellt aus einer Datei ein Diagramm.
         */
        public ActionResult createDiagram()
        {
            return View();
        }
    }
}