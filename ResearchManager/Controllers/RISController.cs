using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers; 

namespace ResearchManager.Controllers
{
    public class RISController : Controller
    {

        // GET: RIS
        public ActionResult Index()
        {
            // Create new Entities object. This is a reference to the database.
            Entities db = new Entities();

            //Create a variable to store projects data from the database
            var projects = db.projects;

            return View(projects.ToList());
        }

        public ActionResult Details(int id)
        {
            try
            {   //Use searchTerm to query the database for project details and store this in a variable project
                int searchTerm = id;
                Entities db = new Entities();
                var project = db.projects.Where(p => p.projectID == searchTerm).First();
                return View(project);
            }
            catch
            {
                //Return to Index if error occurs
                return RedirectToAction("Index");
            }
        }

        public ActionResult ReuploadExpend(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == progID).First();
            return View(sampleProject);
        }

        [HttpPost]
        public ActionResult ReuploadExpend(int projectID, HttpPostedFileBase file)
        {
            var allowedExtensions = new[] { ".xls", ".xlsx" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                TempData["alert"] = "Select a file with extension type: " + string.Join(" ", allowedExtensions); ;
                return RedirectToAction("Index");
            }
            var path = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var fileextension = Path.GetExtension(fileName);
                    Random randInt = new Random();
                    int rand1 = randInt.Next(1, 10000);
                    int rand2 = randInt.Next(1, 10000);
                    int rand3 = randInt.Next(1, 10000);
                    String randOne = Convert.ToString(rand1);
                    String randTwo = Convert.ToString(rand2);
                    String randThree = Convert.ToString(rand3);
                    String newName = randOne + randTwo + randThree + "." + fileextension;
                    path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"), newName);
                    file.SaveAs(path);
                }
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Index");
            }

            int progID = projectID;
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == progID).First();
            var fToDel = sampleProject.projectFile;
            sampleProject.projectFile = path;
            db.Set<project>().Attach(sampleProject);
            db.Entry(sampleProject).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            if (System.IO.File.Exists(fToDel))
            {
                System.IO.File.Delete(fToDel);
            }

            return RedirectToAction("Index");
        }

        public FileResult Download(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == progID).First();

            return File(dProject.projectFile, "application/" + Path.GetExtension(dProject.projectFile), dProject.pName + "-ExpenditureFile" + Path.GetExtension(dProject.projectFile));
        }

        string IdToLabel(string id)
        {
            // map user position to signature
            if (id != "")
            {
                if (id == "RIS")
                    return "Created";

                if (id == "Researcher")
                    return "Researcher_Signs";

                if (id == "Associate Dean")
                    return "Associate_Dean_Signs";

                if (id == "Dean")
                    return "Dean_Signs";
            }
            return null;
        }

        void EmailHandler(string email, string projectName, string projectDe)
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
                string body = "Project " + projectName + " requires signature. \nThank you.";
                WebMail.Send(to: email, subject: title, body: body, cc: "", bcc: "", isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
            }
        }

    }
}