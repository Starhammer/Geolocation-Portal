using Geolocation_Portal_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Geolocation_Portal_Test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Verwaltung()
        {
            return View();
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

        /* Auskommentiert von Michael, weil der Code aktuell noch nicht einwandfrei funktioniert (Kontaktformular)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Kontakt(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                var mail = new MailMessage();
                mail.To.Add(new MailAddress(model.SenderEmail));
                mail.Subject = "Your Email Subject";
                mail.Body = string.Format("<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>", model.SenderName, model.SenderEmail, model.Message);
                mail.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(mail);
                    return RedirectToAction("SuccessMessage");
                }
            }
            return View(model);
        }

        public ActionResult SuccessMessage()
        {
            return View();
        }
        */

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