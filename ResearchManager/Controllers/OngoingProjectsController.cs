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
        string user_position = "RIS";
        string projectLabel = "";
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
        // 1: RIS Staff can request researcher for signature            = Completion label id = Created (1)                 = User Id = 1   RIS has signed
        // 2: Researcher sends project to Assoc. Dean for signature     = Completion label id = Researcher_Signed (2)       = User Id = 2   Researcher has signed
        // 3: Associate Dean can sign once complete                     = Completion label id = Associate_Dean_Signed (3)   = User Id = 3   Associate Dean has signed
        // 4: Dean can sign once complete                               = Completion label id = Dean_Signed (4)             = User Id = 4   Dean has signed

        // What happens here is;
        // 1:   Load user position
        // 2:   Convert user position to its corresponding project completion level label
        // 3:   Return all projects that has a projectStage equal to the project completion label (see mapping comment above)
        // --Not added yet--
        // 4:   Each project will have a button associated with it.
        // 5:   Depending on the current user the button will perform one of the aforementioned tasks
        // 6:   When a project is signed it no longer falls under the current users remit, force refresh of page and the project will no longer be shown

        // Convert user id to project label
        string IdToLabel(string id)
        {
            if (id != "") {
                if (id == "RIS")
                    return "Created";

                if (id == "Researcher")
                    return "Researcher_Signed";

                if (id == "Associate_Dean")
                    return "Associate_Dean_Signed";

                if (id == "Dean")
                    return "Dean_Signed";
            }
            else
            {
                return "Invalid user ID - Caught in try/catch";
            }
            return "Error";
        }

        // On load we capture the sign in session
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        // GET: OngoingProjects
        public ActionResult Index()
        {
            // return projects that relate to the current users level
            Entities db = new Entities();
            // requires changing the current int representation of projectstage in project.cs to string, so as to match the database

            string label = IdToLabel(user_position);
            System.Diagnostics.Debug.Write("\n");
            System.Diagnostics.Debug.Write("\n user id = " + user_position + + " " + projectLabel +  "\n");
            System.Diagnostics.Debug.Write("\n");
            var projects = db.projects.Where(p => p.projectStage == projectLabel);
            return View(projects.ToList());
            return View();
        }
    }
}