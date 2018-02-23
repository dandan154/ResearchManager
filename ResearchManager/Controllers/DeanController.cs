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
    public class DeanController : Controller
    {
        // GET: RIS
        public ActionResult Index()
        {
            string session_capture = Convert.ToString(Session["StaffPosition"]);

            string label = HelperClasses.SharedControllerMethods.IdToLabel(session_capture);


            // Create new Entities object. This is a reference to the database.
            Entities db = new Entities();

            //Create a variable to store projects data from the database
            //var projects = db.projects;

            // <Connor's edits
            var projects = db.projects.Where(p => p.projectStage == label);
            // RIS can view all projects
            if (session_capture == "RIS")
            {
                projects = db.projects;
            }
            // everybody else is limited
            else
            {
                projects = db.projects.Where(p => p.projectStage == label);
            }
            // <Connor's edits


            return View(projects.ToList());
        }


        public ActionResult viewProject()
        {
            return View();
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

        public ActionResult reUploadExpend(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == progID).First();
            return View(sampleProject);
        }

        [HttpPost]
        public ActionResult reUploadExpend(int projectID, HttpPostedFileBase file)
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
                    path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"), fileName);
                    file.SaveAs(path);
                }
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("createProject");
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

        public ActionResult View(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == progID).First();
            return View(sampleProject);
        }
        public FileResult Download(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == progID).First();

            return File(dProject.projectFile, "application/" + Path.GetExtension(dProject.projectFile), dProject.pName + "-ExpenditureFile" + Path.GetExtension(dProject.projectFile));
        }

        public ActionResult sign(int projectID)
        {
            int id = projectID;
            string session_capture = Session["StaffPosition"].ToString();

            string label = HelperClasses.SharedControllerMethods.IdToLabel(session_capture);

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

            TempData["alert"] = "You have signed " + projectToEdit.pName;

            var projects = db.projects.Where(p => p.projectStage == label);

            string email = HelperClasses.SharedControllerMethods.PositionToNewPosition(session_capture);
            HelperClasses.SharedControllerMethods.EmailHandler(email, projectToEdit.pName, projectToEdit.pDesc);

            // show all projects without previously changed one
            if (Session["StaffPosition"].ToString() == "RIS")
            {
                projects = db.projects.Where(p => p.projectStage == label);
            }
            return RedirectToAction("Index", projects.ToList());//(projects.ToList());
        }
    }
}