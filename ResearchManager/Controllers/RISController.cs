using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO; 
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchManager.Controllers
{
    public class RISController : Controller
    {

        // GET: RIS
        public ActionResult Index()
        {
            // Create new Entities object. This is a reference to the database.
            Entities db = new Entities();

            //Create a variable to store projects data from the database
            var projects = db.projects;

            return View(projects.ToList());
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

        public FileResult Download(int projectID)
        {
            int progID = projectID;
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == progID).First();

            return File(dProject.projectFile, "application/" + Path.GetExtension(dProject.projectFile), dProject.pName + "-ExpenditureFile" + Path.GetExtension(dProject.projectFile));
        }

    }
}