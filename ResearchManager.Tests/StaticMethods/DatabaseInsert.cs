using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchManager.Tests
{
    static class DatabaseInsert
    {
        static public user AddTestUser(string pos, Entities db)
        {
            user testUser = new user();
            testUser.Email = "john@smith.com";
            testUser.forename = "John";
            testUser.surname = "Smith";
            testUser.hash = "null";
            testUser.salt = "null";
            testUser.staffPosition = pos;
            testUser.Matric = "1";

            var userToDel = db.users.Add(testUser);
            System.Diagnostics.Debug.WriteLine(userToDel);
            System.Diagnostics.Debug.WriteLine(testUser);
            db.SaveChanges();

            return userToDel;
        }

        static public project AddTestProject(user user, Entities db)
        {
            var tempProject = new project();
            tempProject.userID = user.userID;
            tempProject.pName = "test";
            tempProject.pDesc = "test";
            tempProject.pAbstract = "test";
            tempProject.dateCreated = DateTime.UtcNow;
            tempProject.projectFile = "none.xlsx";
            tempProject.projectStage = "Test";

            var projectToDel = db.projects.Add(tempProject);
            db.SaveChanges();

            return projectToDel;

        }
    }
}
