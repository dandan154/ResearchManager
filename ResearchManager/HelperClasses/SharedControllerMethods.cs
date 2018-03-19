using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.IO; 

namespace ResearchManager.HelperClasses
{
    public static class SharedControllerMethods
    {
        public static void EmailHandler(string email, string projectName, string projectDe)
        {
            try
            {
                // email settings
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = true;
                WebMail.UserName = "donotreply.rsmanagerdundee@gmail.com";
                WebMail.Password = "agile100";
                WebMail.From = "donotreply.rsmanagerdundee@gmail.com";

                // build email and send
                string title = "Project Signature Required";
                string body = "Project " + projectName + " requires signature. \nThank you.\nThis is a no reply email, any replies will not be answered.\n Dundee Research Project Manager";

                WebMail.Send(to: email, subject: title, body: body, cc: "", bcc: "", isBodyHtml: true);

            }
            catch (Exception)
            {
            }
        }

        public static string PositionToNewPosition(string pos)
        {
            var db = new Entities();
            if (pos == "RIS")
            {
                string l = "Researcher";
                var userToEmail = db.users.Where(u => u.staffPosition == l).First();
                return userToEmail.Email;
            }
            else if (pos == "Researcher")
            {
                string l = "AssociateDean";
                var userToEmail = db.users.Where(u => u.staffPosition == l).First();
                return userToEmail.Email;
            }
            else if (pos == "AssociateDean")
            {
                string l = "Dean";
                var userToEmail = db.users.Where(u => u.staffPosition == l).First();
                return userToEmail.Email;
            }
            else if (pos == "Dean")
            {
                return ("donotreply.rsmanagerdundee@gmail.com");
            }
            else
            {
                return ("donotreply.rsmanagerdundee@gmail.com");
            }
        }

        // Convert user id to project label
        public static string IdToLabel(string id)
        {
            // map user position to signature
            if (id != "")
            {
                if (id == "RIS")
                    return "Project created";

                if (id == "Researcher")
                    return "Awaiting further action from Researcher";

                if (id == "AssociateDean")
                    return "Awaiting further action from Associate Dean";

                if (id == "Dean")
                    return "Awaiting further action from Dean";
            }
            else
            {
                return "Invalid user ID - Caught in try/catch";
            }
            return "Error";
        }

        public static string Signature(string s)
        {

            // update signatures based on current user
            if (s == "RIS")
            {
                return "Awaiting further action from Researcher";
            }
            else if (s == "Researcher")
            {
                return "Awaiting further action from Associate Dean";
            }
            else if (s == "AssociateDean")
            {
                return "Awaiting further action from Dean";
            }
            else if (s == "Dean")
            {
                return "Approved";
            }

            return null; 
        }

       
    }
}