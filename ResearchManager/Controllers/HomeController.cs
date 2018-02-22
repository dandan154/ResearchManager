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
            if(Session["UserPosition"] != null)
                return RedirectToAction("SignIn");

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
                    var usr = db.users.Where(u => u.userID == model.userID).First();

                    if (usr != null)
                    {
                        string ps = model.plntxtPass + usr.salt;
                        ps = Crypto.HashPassword(ps);


                        if (ps == usr.hash)
                        {
                            Session["UserID"] = usr.userID;
                            Session["StaffPosition"] = usr.staffPosition;

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
            try
            {
                //Redirect user to appropriate page
                if (Session["StaffPosition"].ToString() == "Research")
                {
                    return RedirectToAction("Index", "Research");

                }
                else if (Session["StaffPosition"].ToString() == "RIS")
                {
                    return RedirectToAction("viewProject", "RIS");
                }
                else if (Session["StaffPosition"].ToString() == "Dean" || Session["StaffPosition"].ToString() == "AssociateDean")
                {
                    return RedirectToAction("Index", "Dean");
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
