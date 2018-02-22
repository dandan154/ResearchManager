using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            Entities db = new Entities();
            var projects = db.projects;
            return View(projects.ToList());
        }

        public ActionResult Details()
        {
            String i = RouteData.Values["id"].ToString();
            int searchTerm = Convert.ToInt32(i); 
 
            Entities db = new Entities();
            var project = db.projects.Where(p => p.projectID == searchTerm).First(); 
            return View(project); 
        }

    }
}