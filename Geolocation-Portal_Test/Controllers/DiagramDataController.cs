using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class DiagramDataController : Controller
    {
        /// <summary>
        /// This action result returns the index view.
        /// </summary>
        /// <param name="id">A data set ID to generate a diagram from it.</param>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.FileId = id;
            }

            return View();
        }

        /// <summary>
        /// Displays a data set as a diagram.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Diagramm()
        {
            return View();
        }

        /// <summary>
        /// Creates a diagram from a file.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult createDiagram()
        {
            return View();
        }
    }
}