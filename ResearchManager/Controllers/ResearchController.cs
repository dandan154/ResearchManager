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
            return View();
        }
        public ActionResult createProject()
        { 
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
                ViewBag.Message = "Upload failed";
                //return RedirectToAction("createProject");
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
                return RedirectToAction("createProject");
            }

            return View(model);
        }
        
    }
}