using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            var usr = new ResearchManager.Models.SignInUser(); 
            return View(usr); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(ResearchManager.Models.SignInUser model)
        {
            var db = new Entities();

            if (!ModelState.IsValid)
            {
                var usr = db.users.Where(u => u.userID == model.userID).First();

                if (usr != null)
                {
                    Session["UserID"] = usr.userID;

                    if (usr.staffPosition == 1)
                    {
                        return RedirectToAction("Index", "Research");

                    }
                    else if(usr.staffPosition == 2)
                    {
                        return RedirectToAction("Index", "RIS");
                    }
                    else if(usr.staffPosition > 2)
                    {
                        return RedirectToAction("Index", "Dean"); 
                    }
                }

            }
            return View();
        }
    }
}