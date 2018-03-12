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
            int session = Convert.ToInt32(Session["UserID"]);
            Entities db = new Entities();
            var projects = db.projects.Where(p => p.userID == session);
            return viewIndexPage(Session["UserID"]);
        }

        public ActionResult viewIndexPage(object UserID)
        {
            int session = Convert.ToInt32(UserID);
            Entities db = new Entities();
            var projects = db.projects.Where(p => p.userID == session);
            return View(projects.ToList());
        }

        public ActionResult EditProject(int projectID)
        {
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
                return RedirectToAction("createProject");
            }

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(Session["UserID"].ToString());
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

        public ActionResult sign(int projectID)
        {
            int id = projectID;
            string session_capture = Session["StaffPosition"].ToString();

            string label = HelperClasses.SharedControllerMethods.IdToLabel(session_capture);

            // return our project to be changed (should be only 1)
            var db = new Entities();
            var projectToEdit = db.projects.Where(p => p.projectID == id).First();

            projectToEdit.projectStage = HelperClasses.SharedControllerMethods.Signature(session_capture);

            // update database
            db.Set<project>().Attach(projectToEdit);
            db.Entry(projectToEdit).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

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