using System;
using System.Web.Helpers;
using System.Web.Mvc;
using ResearchManager.Models;
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


                // Here is where we will set the 
                // temp variables for email content
                string recipient = obj.ToEmail;
                string subject = obj.EmailSubject;
                string body = obj.EMailBody;
                string cc = obj.EmailCC;
                string bcc = obj.EmailBCC;

                WebMail.Send(to: recipient, subject: subject, body: body, cc: cc, bcc: bcc, isBodyHtml: true);
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