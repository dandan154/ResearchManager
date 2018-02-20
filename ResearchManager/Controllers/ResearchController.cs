using System;
using System.Collections.Generic;
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
        public ActionResult createProject(project model)
        {
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
                    projectFile = model.projectFile
                 });
                db.SaveChanges();
                return RedirectToAction("createProject");
            }

            return View(model);
        }
        
    }
}