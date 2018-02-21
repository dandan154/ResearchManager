using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers; 

namespace ResearchManager.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            //Session variable test 
            Session["UserID"] = 0;

            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(ResearchManager.Models.SignInUser model)
        {
            var db = new Entities();

            try
            {
                if (!ModelState.IsValid)
                {
                    //WARNING - method needs try-catch for when db cannot be queried
                    var usr = db.users.Where(u => u.userID == model.userID).First();

                    if (usr != null)
                    {
                        string ps = model.plntxtPass + usr.salt;
                        ps = Crypto.HashPassword(ps);

                        if (ps == usr.hash)
                        {
                            Session["UserID"] = usr.userID;
                            Session["UserPosition"] = usr.staffPosition;

                            //Redirect user to appropriate page
                            if (usr.staffPosition == 1)
                            {
                                return RedirectToAction("Index", "Research");

                            }
                            else if (usr.staffPosition == 2)
                            {
                                return RedirectToAction("Index", "RIS");
                            }
                            else if (usr.staffPosition > 2)
                            {
                                return RedirectToAction("Index", "Dean");
                            }
                        }
                    }
                }
            }
            ViewBag.Message = "Login Failed, Please Try Again";
            return View();
        }
    }
}