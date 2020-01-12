using Geolocation_Portal_Test.Models;
using System;
using System.Collections.Generic;
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
        private Entities db = new Entities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Verwaltung()
        {
            if (Session["UserRole"] != null)
            {
                role role = db.role.Find(Convert.ToInt32(Session["UserRole"]));
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

        public ActionResult Datenschutz()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Impressum()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Kontakt()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Kontakt(string name, string senderemail, string message)
        {
            SendContactEmail(name, senderemail, message);
            return Content("Thanks for contacting us! We'll be in contact with you soon!");
        }

        /**
         * https://forums.asp.net/t/2115115.aspx?How+to+setup+a+contact+us+form+
         */
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

        public ActionResult Nutzungsbedingungen()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult WeitereGemeindeseiten()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}