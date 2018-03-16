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
    public class HomeControllerTest

    {
    /*  [TestMethod]  
        public void SignIn()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.SignIn() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        */

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
