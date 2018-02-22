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
        int user_id;
        string projectLabel;
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

        // The following shows the relationship between tasks, completion id, and user id
        // 1: RIS Staff can request researcher for signature            = Completion label id = Created (1)                 = User Id = 1
        // 2: Researcher sends project to Assoc. Dean for signature     = Completion label id = Researcher_Signed (2)       = User Id = 2
        // 3: Associate Dean can sign once complete                     = Completion label id = Associate_Dean_Signed (3)   = User Id = 3
        // 4: Dean can sign once complete                               = Completion label id = Dean_Signed (4)             = User Id = 4

        // What happens here is;
        // 1:   Load cookie containig user ID (for now we also create the cookie here, on rleease it will be created on login)
        // 2:   Convert user ID to its corresponding project completion level label
        // 3:   Return all projects that has a projectStage equal to the project completion label (see mapping comment above)
        // --Not added yet--
        // 4:   Each project will have a button associated with it.
        // 5:   Depending on the current user the button will perform one of the aforementioned tasks
        // 6:   When a project is signed it no longer falls under the current users remit, force refresh of page and the project will no longer be shown

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
        protected void Session_Start(object sender, EventArgs e)
        {
            // Start creating cookie
            HttpCookie new_cookie = new HttpCookie("user_id");

            //Set the cookies value
            new_cookie.Value = "Test File 2";

            //Set the cookie to expire in 5 minute
            DateTime dtNow = DateTime.Now;
            TimeSpan tsMinute = new TimeSpan(0, 0, 5, 0);
            new_cookie.Expires = dtNow + tsMinute;

            Response.Cookies.Add(new_cookie);
  
            Response.Write("Cookie written");
            // end creating cookie

            // grab the cookie
            HttpCookie cookie = Request.Cookies["user_id"];

            // check to make sure the cookie exists
            if (cookie != null)
            {
                //     ReadCookie();
            }
            else
            {
                Response.Write("Cookie not found");
            }

        }

        protected void ReadCookie()
        {
            // get the cookie name the user entered

            // grab the cookie
            HttpCookie cookie = Request.Cookies["user_id"];

            // check to make sure the cookie exists
            if (cookie == null)
            {
                Response.Write("Cookie not found");
            }
            else
            {
                // write the cookie value
                String strCookieValue = cookie.Value.ToString();
                Response.Write("The cookie contains: " + strCookieValue);
                user_id = Convert.ToInt32(strCookieValue);
                projectLabel = strCookieValue; // temp, delete later
                // convert user id to project label
                //projectLabel = IdToLabel(user_id);
            }
        }

        // GET: OngoingProjects
        public ActionResult Index()
        {
            // return projects that relate to the current users level
            Entities db = new Entities();
            // requires changing the current int representation of projectstage in project.cs to string, so as to match the database
            var projects = db.projects.Where(p => p.projectFile == "Test File 2");
            return View(projects.ToList());
        }
    }
}