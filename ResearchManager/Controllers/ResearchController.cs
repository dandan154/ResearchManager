using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
<<<<<<< HEAD
using System.Security.Cryptography;
using System.Text;
=======
using System.Web.Helpers;
>>>>>>> d7cb739e87a89c8022b378db97a418c0a1b70235

namespace ResearchManager.Controllers
{
    public class ResearchController : Controller
    {
        // GET: Research
        public ActionResult Index()
        {
            Entities db = new Entities();
            var projects = db.projects.Where(p => p.userID == 1);
            return View(projects.ToList());
        }
        public ActionResult createProject()
        {
            ViewBag.Message = "Form for creating new research projects into the management system";
            return View();
        }

        public FileResult download(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == progID).First();

            return File(dProject.projectFile, "application/" + Path.GetExtension(dProject.projectFile),dProject.pName+ "-ExpenditureFile" + Path.GetExtension(dProject.projectFile));
        }

        [HttpPost]
        public ActionResult createProject(project model, HttpPostedFileBase file)
        {
            int uID = Convert.ToInt32(Session["UserID"]);
            var allowedExtensions = new[] { ".xls", ".xlsx"};
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                TempData["alert"] = "Select a file with extension type: " + string.Join(" ", allowedExtensions); ;
                return RedirectToAction("createProject");
            }
            var path ="";
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
                    path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"),newName);
                    file.SaveAs(path);
                }
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("createProject");
            }

            if (ModelState.IsValid)
            {
                var db = new Entities();
                db.projects.Add(new project
                {
                    userID = Convert.ToInt32(Session["UserID"]),
                    dateCreated = DateTime.Now.ToUniversalTime(),
                    projectStage = "Created",
                    pName = model.pName,
                    pAbstract = model.pAbstract,
                    pDesc = model.pDesc,
                    projectFile = path,
                 });
                db.SaveChanges();
                ViewBag.Message = "Created Project";
                return RedirectToAction("Index");

            }

            return View(model);
        }


        // Convert user id to project label
        string IdToLabel(string id)
        {
            // map user position to signature
            if (id != "")
            {
                if (id == "RIS")
                    return "Created";

                if (id == "Researcher")
                    return "Researcher_Signs";

                if (id == "AssociateDean")
                    return "Associate_Dean_Signs";

                if (id == "Dean")
                    return "Dean_Signs";
            }
            else
            {
                return "Invalid user ID - Caught in try/catch";
            }
            return "Error";
        }

        public ActionResult sign(int projectID)
        {
            int id = projectID;
            string session_capture = Session["StaffPosition"].ToString();

            string label = IdToLabel(session_capture);

            // return our project to be changed (should be only 1)
            var db = new Entities();
            var projectToEdit = db.projects.Where(p => p.projectID == id).First();

            // update signatures based on current user
            if (session_capture == "RIS")
            {
                projectToEdit.projectStage = "Researcher_Signs";
            }
            if (session_capture == "Researcher")
            {
                projectToEdit.projectStage = "Associate_Dean_Signs";
            }
            if (session_capture == "AssociateDean")
            {
                projectToEdit.projectStage = "Dean_Signs";
            }
            if (session_capture == "Dean")
            {
                projectToEdit.projectStage = "Completed";
            }

            // update database
            db.Set<project>().Attach(projectToEdit);
            db.Entry(projectToEdit).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var projects = db.projects.Where(p => p.projectStage == label);

            string email = positionToNewPosition(session_capture);
            EmailHandler(email, projectToEdit.pName, projectToEdit.pDesc);

            // show all projects without previously changed one
            if (Session["StaffPosition"].ToString() == "RIS")
            {
                projects = db.projects.Where(p => p.projectStage == label);
            }
            return RedirectToAction("Index", projects.ToList());//(projects.ToList());
        }

        string positionToNewPosition(string pos)
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

        // handle email sending
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
                string body = "Project " + projectName + " requires signature. \nThank you.\nThis is a no reply email, any replies will not be answered.\n Dundee Research Project Manager";
                WebMail.Send(to: email, subject: title, body: body, cc: "", bcc: "", isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
            }
        }
        // <Connor's edits
    }

}