using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using ResearchManager.HelperClasses;

namespace ResearchManager.Controllers
{
    public class RISController : Controller
    {

        public ActionResult Details(int id = -1) //
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
                if (active.staffPosition != "RIS")
                {
                    return RedirectToAction("ControllerChange", "Home");
                }

            }

            ViewBag.DashboardText = "RIS Staff Dashboard";

            try
            {   //Use searchTerm to query the database for project details and store this in a variable project
                Entities db = new Entities();
                var project = db.projects.Where(p => p.projectID == id).First();
                return View("Details",project);
            }
            catch
            {
                //Return to Index if error occurs
                return RedirectToAction("Index");
            }
        }

        public ActionResult ReuploadExpend(int projectID) //
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
                if (active.staffPosition != "RIS")
                {
                    return RedirectToAction("ControllerChange", "Home");
                }

            }

            ViewBag.DashboardText = "RIS Staff Dashboard";
            Entities db = new Entities();
            var sampleProject = db.projects.Where(p => p.projectID == projectID).First();
            return View("ReuploadExpend",sampleProject);
        }

        [HttpPost]
        public ActionResult ReuploadExpend(int projectID, HttpPostedFileBase file) // -
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
                if (active.staffPosition != "RIS")
                {
                    return RedirectToAction("ControllerChange", "Home");
                }

            }

            var allowedExtensions = new[] { ".xls", ".xlsx" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                TempData["alert"] = "Select a file with extension type: " + string.Join(" ", allowedExtensions);
                return RedirectToAction("Index");
            }
            var path = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    System.Diagnostics.Debug.WriteLine("filelength > 0");
                    var fileName = Path.GetFileName(file.FileName);
                    var fileextension = Path.GetExtension(fileName);

                    do
                    {
                        const int STRING_LENGTH = 32;
                        fileName = Crypto.GenerateSalt(STRING_LENGTH).Substring(0, STRING_LENGTH);
                        String TestName = fileName + fileextension;
                        path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"), TestName);
                    } while (System.IO.File.Exists(path) == true);

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
            SharedControllerMethods.addToHistory(active.userID, projectID, "Modified the project file");
            if (System.IO.File.Exists(fToDel))
            {
                System.IO.File.Delete(fToDel);
            }

            return RedirectToAction("Index");
        }

        // GET: RIS
        public ActionResult Index()//
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
                if (active.staffPosition != "RIS")
                {
                    return RedirectToAction("ControllerChange", "Home");
                }

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

            return View("Index",projects.ToList());
        }

        public FileResult Download(int projectID) //
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                RedirectToAction("SignIn", "Home");
                return null;
            }
            else
            {
                TempData["ActiveUser"] = active;
                if (active.staffPosition != "RIS")
                {
                    RedirectToAction("ControllerChange", "Home");
                    return null;
                }

            }
            int progID = projectID;
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == progID).First();

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
                if (active.staffPosition != "RIS")
                {
                    return RedirectToAction("ControllerChange", "Home");
                }

            }

            ViewBag.DashboardText = "RIS Staff Dashboard";
            string label = HelperClasses.SharedControllerMethods.IdToLabel(active.staffPosition);

            // return our project to be changed (should be only 1)
            var db = new Entities();
            var projects = db.projects.Where(p => p.projectStage == label);
            var projectToEdit = db.projects.Where(p => p.projectID == projectID).First();

            if ((active.staffPosition == "RIS" && projectToEdit.projectStage == "Created"))
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
                HelperClasses.SharedControllerMethods.addToHistory(active.userID, projectID, "RIS Staff Signed The Project");
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

        public ActionResult Clarification(int projectID)
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
                if (active.staffPosition != "RIS")
                {
                    return RedirectToAction("ControllerChange", "Home");
                }

            }
            string label = "Need Further Clarification";
            var db = new Entities();
            var projectToEdit = db.projects.Where(p => p.projectID == projectID).First();
            if ((active.staffPosition == "RIS" && projectToEdit.projectStage == "Created"))
            {
                // update signatures based on current user
                projectToEdit.projectStage = "awaitingClarification";

                // update database
                db.Set<project>().Attach(projectToEdit);
                db.Entry(projectToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["alert"] = "You have returned " + projectToEdit.pName + " for further clarification";

                string email = HelperClasses.SharedControllerMethods.PositionToNewPosition(active.staffPosition);
                HelperClasses.SharedControllerMethods.addToHistory(active.userID, projectID, "RIS Staff requested clarification");
            }
            else
            {
                TempData["alert"] = "You do not have permission to request clarification for : " + projectToEdit.pName;
            }

            return RedirectToAction("Index");
        }
    }
}