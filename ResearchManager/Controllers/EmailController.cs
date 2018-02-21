using System;
using System.Web.Helpers;
using System.Web.Mvc;
using ResearchManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace ResearchManager.Controllers
{
    public class EmailController : Controller
    {
        // GET: Home  
        public ActionResult SendEmail()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(EmailModel obj)
        {

            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = "donotreply.rsmanagerdundee@gmail.com";
                WebMail.Password = "agile100";

                //Sender email address.  
                WebMail.From = "donotreply.rsmanagerdundee@gmail.com";

                // get details from database here, replacing the above variables as needed

                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";

            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
            return View();
        }
    }
}