using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class FAQController : Controller
    {
        /// <summary>
        /// This action result returns the index view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Enables you to download the User Guide.
        /// </summary>
        /// <returns>Return of the User Guide file.</returns>
        public ActionResult APIGuideDownload()
        {
            string path = Server.MapPath("~/App_Data/API_Dokumentation.pdf");
            string mime = MimeMapping.GetMimeMapping(path);

            return File(path, mime, "API_Dokumentation.pdf");
        }
    }
}