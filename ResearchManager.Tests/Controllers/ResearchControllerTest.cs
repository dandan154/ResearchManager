using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResearchManager;
using ResearchManager.Controllers;
using System.Web;

namespace ResearchManager.Tests.Controllers
{
    [TestClass]
    public class ResearchControllerTest
    {
       /* [TestMethod]
        public void ResearchIndexTest()
        {
            // Arrange
            ResearchController controller = new ResearchController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        */
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

    }
}
