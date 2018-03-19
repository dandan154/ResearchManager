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

        public ActionResult Details(int id)
        {
            //TempData Check and Renewal
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            ViewBag.DashboardText = "RIS Staff Dashboard";

            try
            {   //Use searchTerm to query the database for project details and store this in a variable project
                Entities db = new Entities();
                var project = db.projects.Where(p => p.projectID == id).First();
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
            //TempData Check and Renewal
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            ViewBag.DashboardText = "RIS Staff Dashboard";
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == projectID).First();
            return View(sampleProject);
        }

        [HttpPost]
        public ActionResult ReuploadExpend(int projectID, HttpPostedFileBase file)
        {
            //TempData Check and Renewal
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

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
                    while (System.IO.File.Exists(path) == true)
                    {
                        int rand4 = randInt.Next(1, 10000);
                        int rand5 = randInt.Next(1, 10000);
                        int rand6 = randInt.Next(1, 10000);
                        String randFour = Convert.ToString(rand1);
                        String randFive = Convert.ToString(rand2);
                        String randSiz = Convert.ToString(rand3);
                        String TestName = randOne + randTwo + randThree + "." + fileextension;
                        path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"), TestName);
                    }
                    file.SaveAs(path);
                }
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Index");
            }

            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == projectID).First();
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

        // GET: RIS
        public ActionResult Index()
        {
            //TempData Check and Renewal
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            ViewBag.DashboardText = "RIS Staff Dashboard";

            string label = HelperClasses.SharedControllerMethods.IdToLabel(active.staffPosition);

            // Create new Entities object. This is a reference to the database.
            Entities db = new Entities();

            var projects = db.projects.Where(p => p.projectStage == label);

            // RIS can view all projects
            if (active.staffPosition == "RIS")
            {
                projects = db.projects;
            }
            // everybody else is limited
            else
            {
                projects = db.projects.Where(p => p.projectStage == label);
            }

            return View(projects.ToList());
        }

        public FileResult Download(int projectID)
        {
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == projectID).First();
            return File(dProject.projectFile, "application/" + Path.GetExtension(dProject.projectFile), dProject.pName + "-ExpenditureFile" + Path.GetExtension(dProject.projectFile));
        }
    
        public ActionResult Sign(int projectID)
        {
            //TempData Check and Renewal
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            ViewBag.DashboardText = "RIS Staff Dashboard";
            string label = HelperClasses.SharedControllerMethods.IdToLabel(active.staffPosition);

            // return our project to be changed (should be only 1)
            var db = new Entities();
            var projects = db.projects.Where(p => p.projectStage == label);
            var projectToEdit = db.projects.Where(p => p.projectID == projectID).First();

            if ((active.staffPosition == "RIS" && projectToEdit.projectStage == "Awaiting further action from RIS"))
            {
                // update signatures based on current user
                projectToEdit.projectStage = HelperClasses.SharedControllerMethods.Signature(active.staffPosition);

                // update database
                db.Set<project>().Attach(projectToEdit);
                db.Entry(projectToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["alert"] = "You have signed " + projectToEdit.pName;

                string email = HelperClasses.SharedControllerMethods.PositionToNewPosition(active.staffPosition);
                HelperClasses.SharedControllerMethods.EmailHandler(email, projectToEdit.pName, projectToEdit.pDesc); 
         
            }
            else
            {
                TempData["alert"] = "You do not have permission to sign " + projectToEdit.pName;
            }
            // show all projects without previously changed one
            if (active.staffPosition == "RIS") {
                projects = db.projects.Where(p => p.projectStage == label);
            }
            return RedirectToAction("Index", projects.ToList());
        }
    }
}