using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResearchManager;
using ResearchManager.Controllers;
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
            tempUser.staffPosition = "Researcher";
            tempUser.Matric = "999999";
            var userToDel = db.users.Add(tempUser);
            var tempProject = new project();
            tempProject.userID = userToDel.userID;
            tempProject.pName = "test";
            tempProject.pDesc = "test";
            tempProject.pAbstract = "test";
            tempProject.dateCreated = DateTime.UtcNow;
            tempProject.projectFile = "none";
            var projectToDel = db.projects.Add(tempProject);
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
        }

        [TestMethod]
        public void createProjectTest()
        {
            // Arrange
            ResearchController controller = new ResearchController();

            // Act
            ViewResult result = controller.createProject() as ViewResult;

            // Assert
            Assert.AreEqual("Form for creating new research projects into the management system", result.ViewBag.Message);
        }

        [TestMethod]
        public void createProjectPOSTTest()
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
