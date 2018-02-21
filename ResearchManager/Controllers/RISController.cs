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
        private SqlConnection conn;

        // GET: RIS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult viewProject()
        {
            return View();
        }
    }
}