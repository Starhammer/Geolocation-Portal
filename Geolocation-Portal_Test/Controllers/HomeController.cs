using Geolocation_Portal_Test.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Object of the database tables. Each database table is defined as a model below the entities.
        /// The entity object allows you to select tupels from the database and return a model of each tupel.
        /// </summary>
        private Entities DatabaseEntities = new Entities();

        public ContactModel ContactModel
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// This action result returns the index view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the verwaltung view. Here every registered user has an 
        /// overview of the portal functions. However, he only sees the functions that are 
        /// available for his role.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Verwaltung()
        {
            if (Session["UserRole"] != null)
            {
                role role = DatabaseEntities.role.Find(Convert.ToInt32(Session["UserRole"], new CultureInfo("de-DE")));
                if (role == null)
                {
                    return HttpNotFound();
                }
                return View(role);
            }
            else
            {
                return RedirectToAction("Anmelden", "Benutzer");
            }
        }

        /// <summary>
        /// This action result returns the datenschutz view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Datenschutz()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the impressum view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Impressum()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the kontakt view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Kontakt()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the kontakt view.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="senderemail"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Kontakt(string name, string senderemail, string message)
        {
            SendContactEmail(name, senderemail, message);
            return Content("Thanks for contacting us! We'll be in contact with you soon!");
        }

        /// <summary>
        /// Not implementet now, but see https://forums.asp.net/t/2115115.aspx?How+to+setup+a+contact+us+form+ for more infos.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="senderemail"></param>
        /// <param name="message"></param>
        public void SendContactEmail(string user, string senderemail, string message)
        {
            // Your hard-coded email values (where the email will be sent from), this could be
            // define in a config file, etc.
            var email = "{your-email@domain.com}";
            var password = "{your-password}";

            // Your target (you may want to ensure that you have a property within your form so that you know
            // who to send the email to
            string address = "{contact@domain.com}";

            // Builds a message and necessary credentials (example using Gmail)
            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            // This email will be sent from you
            msg.From = new MailAddress(email);
            // Your target email address
            msg.To.Add(new MailAddress(address));
            msg.Subject = $"{user} just filled out the Contact Form!";
            // Build the body of your email using the Body property of your message
            msg.Body = message;

            // Wires up and send the email
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }

        /// <summary>
        /// This action result returns the nutzungsbedingungen view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Nutzungsbedingungen()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the weiteregemeindeseiten view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult WeitereGemeindeseiten()
        {
            return View();
        }

        /// <summary>
        /// This action result returns the dashboard view.
        /// </summary>
        /// <returns>Returns a view to the browser.</returns>
        public ActionResult Dashboard()
        {
            return View();
        }

        /// <summary>
        /// Enables you to download the User Guide.
        /// </summary>
        /// <returns>Return of the User Guide file.</returns>
        public ActionResult UserGuideDownload()
        {
            string path = Server.MapPath("~/App_Data/User-Guide.pdf");
            string mime = MimeMapping.GetMimeMapping(path);

            return File(path, mime, "User-Guide.pdf");
        }
    }
}