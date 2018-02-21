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
            Entities db = new Entities();
            var projects = db.projects.Where(p => p.userID == 1);
            return View(projects.ToList());
        }
        public ActionResult createProject()
        {
            ViewBag.Message = "Form for creating new research projects into the management system";
            return View();
        }

        [HttpPost]
        public ActionResult createProject(project model, HttpPostedFileBase file)
        {
            var path ="";
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/ExpenditureFiles"), fileName);
                    file.SaveAs(path);
                    Console.WriteLine(fileName);
                    Console.WriteLine(path);

                }
            }
            catch
            {
                //ViewBag.Message = "Upload failed";
                return RedirectToAction("createProject");
            }

            if (ModelState.IsValid)
            {
                var db = new Entities();
                db.projects.Add(new project
                {
                    userID = 1,
                    dateCreated = DateTime.Now.ToUniversalTime(),
                    projectStage = 1,
                    pName = model.pName,
                    pAbstract = model.pAbstract,
                    pDesc = model.pDesc,
                    projectFile = path
                 });
                db.SaveChanges();
                ViewBag.Message = "Created Project";
                return RedirectToAction("createProject");

            }

            return View(model);
        }
    }

}