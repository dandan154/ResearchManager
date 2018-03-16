using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResearchManager;
using ResearchManager.Controllers;
using System.Data.Entity;
namespace ResearchManager.Tests.Controllers
{
    [TestClass]
    public class ResearchControllerTest
    {
        [TestMethod]
        public void IndexTest()
        {
            Entities db = new Entities();


            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            user tempUser = new user();
            tempUser.Email = "test@test.com";
            tempUser.forename = "Testf";
            tempUser.surname = "Tests";
            tempUser.hash = "null";
            tempUser.salt = "null";
            tempUser.staffPosition = "Researcher";
            tempUser.Matric = "999999";
            var userToDel = db.users.Add(tempUser);

            db.SaveChanges();
            var tempProject = new project();
            tempProject.userID = userToDel.userID;
            tempProject.pName = "test";
            tempProject.pDesc = "test";
            tempProject.pAbstract = "test";
            tempProject.dateCreated = DateTime.UtcNow;
            tempProject.projectFile = "none";
            tempProject.projectStage = "Test";
            var projectToDel = db.projects.Add(tempProject);
            db.SaveChanges();
            tempData["ActiveUser"] = userToDel;

            ResearchController controller = new ResearchController();
            controller.TempData = tempData;

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsNotNull(result.TempData["ActiveUser"]);
            Assert.IsNotNull(result.Model);

            db.projects.Remove(projectToDel);
            db.users.Remove(userToDel);
            db.SaveChanges();
        }

        [TestMethod]
        public void CreateProjectTest()
        {
            // Arrange
            ResearchController controller = new ResearchController();

            // Act
            ViewResult result = controller.CreateProject() as ViewResult;

            // Assert
            Assert.AreEqual("Form for creating new research projects into the management system", result.ViewBag.Message);
        }

        [TestMethod]
        public void CreateProjectPOSTTest()
        {
            // ASK ABOUT IN CLASS 
            // Arrange
            ResearchController controller = new ResearchController();
            var p = new project
            {
                userID = 1,
                dateCreated = DateTime.Now.ToUniversalTime(),
                projectStage = "Created",
                pName = "Test Name",
                pAbstract = "Test Abstract",
                pDesc = "Test Description",
                projectFile = "Test Path"
            };

            // Act    
           //HttpActionResult actionResult = controller.createProject(p,file);
           // var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<MovieDTO>;

            // Assert
           // Assert.AreEqual("Form for creating new research projects into the management system", result.ViewBag.Message);
        }

        /*[TestMethod]
        public void Contact()
        {
            // Arrange
            ResearchController controller = new ResearchController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        */
    }
}
