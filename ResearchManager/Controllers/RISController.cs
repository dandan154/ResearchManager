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

        public ActionResult ViewProject()
        {
            return View();
        }

    }
}