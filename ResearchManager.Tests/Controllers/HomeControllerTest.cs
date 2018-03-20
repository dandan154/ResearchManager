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
       [TestMethod]  
        public void HomeSignIn()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.SignIn() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("SignIn", result.ViewName); 
        }
        

        [TestMethod]
        public void HomeContact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Contact", result.ViewName); 
        }

        [TestMethod]
        public void HomeSignOut()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Create new user 
            user testUser = DatabaseInsert.AddTestUser("Dean", db);
            tempData["ActiveUser"] = testUser;

            HomeController controller = new HomeController();
            controller.TempData = tempData;

            db.users.Remove(testUser);
            db.SaveChanges();

            ViewResult view = controller.SignOut() as ViewResult;

            //Main Tests
            Assert.IsNotNull(testUser);
            Assert.IsNotNull(view);
            Assert.AreEqual(null, view.TempData["ActiveUser"]);
            Assert.AreEqual("SignIn", view.ViewName); 


        }
    }
}
