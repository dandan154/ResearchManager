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
            // Arrange
            ResearchController controller = new ResearchController();

            // Act
            ViewResult result = controller.viewIndexPage(999) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
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
