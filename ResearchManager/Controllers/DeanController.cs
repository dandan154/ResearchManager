using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchManager.Controllers
{
    public class DeanController : Controller
    {
        // GET: Dean
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Home", "SignIn");
            return View();
        }
    }
}