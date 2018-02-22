using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearchManager.Controllers
{
    public class OngoingProjectsController : Controller
    {
        // user id corresponds to...
        int user_id = 1; // 1 is a temp id, the intention is to capture a cookie created at login that stores user id
        // ... 
        // 1 = RIS
        // 2 = Researcher
        // 3 = Associate dean
        // 4 = Dean

        // Each respective user id relates to a particular project completion label
        // 1 = Created
        // 2 = Researcher_Signed
        // 3 = Associate_Dean_Signed
        // 4 = Dean_Signed

        // Convert user id to project label
        string IdToLabel(int id)
        {
            try {
                if (id == 1)
                    return "Created";

                if (id == 2)
                    return "Researcher_Signed";

                if (id == 3)
                    return "Associate_Dean_Signed";

                if (id == 4)
                    return "Dean_Signed";
            }
            catch (Exception)
            {
                return "Invalid user ID - Caught in try/catch";
            }
            return "Invalid user ID";
        }

        // On load we capture the cookie from sign in
        protected void Page_Load(object sender, EventArgs e)
        {/*
            //Grab the cookie
             HttpCookie cookie = Request.Cookies[“strCookieName”];

            //Check to make sure the cookie exists
            if (cookie != null)
            {
                //     ReadCookie();
            }
            else
            {
                lblCookie.Text = "Cookie not found. ";
            }
            */
        }

        // GET: OngoingProjects
        public ActionResult Index()
        {
            // convert user id to project label
            //string projectLabel = IdToLabel(user_id); // = valid, yet needs fixing. See comment below.

            // return projects that relate to the current users level
            Entities db = new Entities();
            // requires changing the current int representation of projectstage in project.cs to string, so as to match the database
            var projects = db.projects.Where(p => p.projectStage == user_id);
            return View(projects.ToList());
        }
    }
}