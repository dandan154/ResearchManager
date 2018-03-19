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
    public class ResearchController : Controller
    {
        // GET: Research
        public ActionResult Index()
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                System.Diagnostics.Debug.Print("here");
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                System.Diagnostics.Debug.Print("here2");
                TempData["ActiveUser"] = active; 
            }

            ViewBag.DashboardText = "Researcher Dashboard";

            Entities db = new Entities();
            var projects = db.projects.Where(p => p.userID == active.userID);
            System.Diagnostics.Debug.Print("here4");
            
            return View("Index",projects.ToList());
        }

        public ActionResult Details(int id)
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            ViewBag.DashboardText = "Researcher Dashboard";

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

        public ActionResult EditProject(int projectID)
        {
            ViewBag.DashboardText = "Researcher Dashboard";
            int progID = projectID;
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == progID).First();
            return View(sampleProject);
        }

        [HttpPost]
        public ActionResult EditProject(project edited)
        {
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == edited.projectID).First();
            sampleProject.pName = edited.pName;
            sampleProject.pDesc = edited.pDesc;
            sampleProject.pAbstract = edited.pAbstract;
            db.Set<project>().Attach(sampleProject);
            db.Entry(sampleProject).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CreateProject()
        {
            ViewBag.DashboardText = "Researcher Dashboard";
            ViewBag.Message = "Form for creating new research projects into the management system";
            return View();
        }

        public FileResult Download(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == progID).First();

            return File(dProject.projectFile, "application/" + Path.GetExtension(dProject.projectFile),dProject.pName+ "-ExpenditureFile" + Path.GetExtension(dProject.projectFile));
        }

        [HttpPost]
        public ActionResult CreateProject(project model, HttpPostedFileBase file)
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
                    int rand1, rand2, rand3;
                    string randOne, randTwo, randThree, newName, fileName, fileExtension;
                    Random randInt = new Random();
                    do
                    {
                        fileName = Path.GetFileName(file.FileName);
                        fileExtension = Path.GetExtension(fileName);

                        rand1 = randInt.Next(1, 10000);
                        rand2 = randInt.Next(1, 10000);
                        rand3 = randInt.Next(1, 10000);

                        randOne = Convert.ToString(rand1);
                        randTwo = Convert.ToString(rand2);
                        randThree = Convert.ToString(rand3);

                        newName = randOne + randTwo + randThree + fileExtension;
                        path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"), newName);
                    }
                    while (System.IO.File.Exists(path) == true);

                    file.SaveAs(path);
                }
            }
            catch
            {
                TempData["alert"] = "Error Uploading";
                return RedirectToAction("CreateProject");
            }

            if (ModelState.IsValid)
            {
                var db = new Entities();
                db.projects.Add(new project
                {
                    userID = active.userID,
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

        public ActionResult Sign(int projectID)
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            string label = HelperClasses.SharedControllerMethods.IdToLabel(active.staffPosition);

            // return our project to be changed (should be only 1)
            var db = new Entities(); 
            var projectToEdit = db.projects.Where(p => p.projectID == projectID).First();

            if (projectToEdit.projectStage == "Awaiting further action from Researcher")
                projectToEdit.projectStage = HelperClasses.SharedControllerMethods.Signature(active.staffPosition);
            else if (projectToEdit.projectStage == "Created")
                projectToEdit.projectStage = "Awaiting further action from RIS";

            // update database
            db.Set<project>().Attach(projectToEdit);
            db.Entry(projectToEdit).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var projects = db.projects.Where(p => p.projectStage == label);

            string email = HelperClasses.SharedControllerMethods.PositionToNewPosition(active.staffPosition);
            HelperClasses.SharedControllerMethods.EmailHandler(email, projectToEdit.pName, projectToEdit.pDesc);

            // show all projects without previously changed one
            if (active.staffPosition == "RIS")
            {
                projects = db.projects.Where(p => p.projectStage == label);
            }
            return RedirectToAction("Index", projects.ToList());
        }

    }

}