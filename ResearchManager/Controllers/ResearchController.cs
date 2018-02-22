using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchManager.Controllers
{
    public class ResearchController : Controller
    {   
    
        // GET: Research
        public ActionResult Index()
        {
            int usID = Convert.ToInt32(Session["UserID"]);
            //Session variable test 
            if (Session["UserID"] == null)
                return RedirectToAction("Home","SignIn");
            Entities db = new Entities();
            var projects = db.projects.Where(p => p.userID == usID);
            return View(projects.ToList());
        }
        public ActionResult createProject()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Home", "SignIn");
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
            var allowedExtensions = new[] { ".xls", ".xlsx"};
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                TempData["alert"] = "Select a file with extension type: " + string.Join(" ", allowedExtensions); ;
                return RedirectToAction("Research","createProject");
            }
            var path ="";
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"),fileName);
                    file.SaveAs(path);
                }
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Research","createProject");
            }

            if (ModelState.IsValid)
            {
                int usID = Convert.ToInt32(Session["UserID"]);
                var db = new Entities();
                db.projects.Add(new project
                {
                    userID = usID,
                    dateCreated = DateTime.Now.ToUniversalTime(),
                    projectStage = "Created",
                    pName = model.pName,
                    pAbstract = model.pAbstract,
                    pDesc = model.pDesc,
                    projectFile = path
                 });
                db.SaveChanges();
                ViewBag.Message = "Created Project";
                return RedirectToAction("Research","Index");

            }

            return View(model);
        }
    }

}