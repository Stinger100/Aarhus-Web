using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModel;
using System.Net.Mail;
using Umbraco.Core.Models;
namespace AarhusWebDevCoop.Controller
{ 
    public class ContactFormSurfaceController : SurfaceController
    { 
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("_ContactForm", new ContactForm()); 
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)

            {
                return CurrentUmbracoPage();
            }

            MailMessage message = new MailMessage();
            message.To.Add("marian.zoicas@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;
            
            using (SmtpClient smtp = new SmtpClient())
            {
                
                
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true; smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("marian.zoicas@gmail.com", "extrem21");
                smtp.EnableSsl = true;
                // send mail
                smtp.Send(message);
                TempData["success"] = true;
            }
            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");
            comment.SetValue("messageName", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("messageContent", model.Message);

            Services.ContentService.Save(comment);
            //Services.ContentService.SaveAndPublishWithStatus(comment);

            return RedirectToCurrentUmbracoPage();

            
            }

       

    }
}