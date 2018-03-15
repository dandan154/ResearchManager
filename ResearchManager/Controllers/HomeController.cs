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
        public ActionResult SignIn(user user)
        {
            if (user.staffPosition != null)
                return ControllerChange();
            return View("SignIn");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(Models.SignInData model)
        {
            var db = new Entities();

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                else
                {
                    var usr = db.users.Where(u => u.Matric == model.userID.Trim()).First();

                    if (usr != null)
                    {
                        string ps = model.plntxtPass + usr.salt;
                        bool isCorrect = Crypto.VerifyHashedPassword(usr.hash, ps);

                        if (isCorrect)
                        {
                            user active = new user();

                            TempData["ActiveUser"] = active; 
                            
                            active.staffPosition = usr.staffPosition;
                            active.forename = usr.forename;
                            active.surname = usr.surname;
                            active.Matric = usr.Matric;
                            active.Email = usr.Email;
                            active.userID = usr.userID; 

                            return ControllerChange();
                        }
                    }
                }
            }
            catch
            {
            }
            ViewBag.Message = "Login Failed, Please Try Again";
            return View();
        }

        public RedirectToRouteResult ControllerChange()
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            try
            {
                //Redirect user to appropriate page
                if (active.staffPosition == "Researcher")
                {
                    return RedirectToAction("Index", "Research");
                }
                else if (active.staffPosition == "RIS")
                {
                    return RedirectToAction("Index", "RIS");
                }
                else if (active.staffPosition == "Dean")
                {
                    return RedirectToAction("Index", "Dean");
                }
                else if (active.staffPosition == "AssociateDean")
                {
                    return RedirectToAction("Index", "Associate");
                }
                else
                {
                    return RedirectToAction("SignIn");
                }
            }
            catch
            {
                return RedirectToAction("SignIn"); 
            }
        } 
    }
    
}
