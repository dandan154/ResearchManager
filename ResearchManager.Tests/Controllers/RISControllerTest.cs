using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResearchManager;
using ResearchManager.Controllers;
using System.Web;
using Moq;

namespace ResearchManager.Tests.Controllers
{
    [TestClass]
    public class RISControllerTest

    {
        //Unable to Test due to limitations of ASP MVC
        /*[TestMethod]
        public void SignIn()
        {
            // Arrange
            RISController controller = new RISController();
            Entities db = new Entities();
            db.users.Add(new user
                {
                    Email="test@test.com",
                    forename="test",
                    surname="test",
                    staffPosition="Dean",
                    hash="test",
                    salt="test",
                    Matric="150014251"
                });
            db.SaveChanges();
            // Act
            var addedUser = db.users.Where(u => u.Email == "test@test.com").First();
            RedirectResult result = controller.viewSignIn(addedUser.staffPosition) as RedirectResult;
            db.users.Remove(addedUser);
            db.SaveChanges();

            // Assert
            Assert.IsNotNull(result);
        }
        */

        /*[TestMethod]
        public void RISIndex()
        {
            // Arrange
            RISController controller = new RISController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        */

        public void RISIndexTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);

            var projectToDel = DatabaseInsert.AddTestProject(RISToDel, db);
            tempData["ActiveUser"] = RISToDel;

            RISController controller = new RISController();
            controller.TempData = tempData;

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsNotNull(result.TempData["ActiveUser"]);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(((List<project>)result.Model).First().projectID, projectToDel.projectID);

            db.projects.Remove(projectToDel);
            db.users.Remove(RISToDel);
            db.SaveChanges();
        }

        [TestMethod]
        public void RISIndexRedirectTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempDataRIS = new TempDataDictionary();
            TempDataDictionary tempDataResearcher = new TempDataDictionary();

            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var ResearcherToDel = DatabaseInsert.AddTestUser("Researcher", db);


            tempDataRIS["ActiveUser"] = RISToDel;
            tempDataResearcher["ActiveUser"] = ResearcherToDel;

            RISController RISController = new RISController();
            RISController.TempData = tempDataRIS;

            RISController ResearchRISController = new RISController();
            ResearchRISController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultResearcher = (RedirectToRouteResult)ResearchRISController.Index() as RedirectToRouteResult;
            ViewResult resultRIS = (ViewResult)RISController.Index() as ViewResult;


            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultResearcher);
            Assert.IsTrue(resultResearcher.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultResearcher.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultResearcher.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultResearcher.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultRIS);
            Assert.AreEqual("Index", resultRIS.ViewName);
        }

        /////////////////////////////////*INDEX TESTS*/////////////////////////////////

        /////////////////////////////////Details TESTS/////////////////////////////////
        [TestMethod]
        public void RISDetailsTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);

            var projectToDel = DatabaseInsert.AddTestProject(RISToDel, db);
            tempData["ActiveUser"] = RISToDel;

            RISController controller = new RISController();
            controller.TempData = tempData;

            // Act
            ViewResult result = controller.Details(projectToDel.projectID) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ViewName);
            Assert.IsNotNull(result.TempData["ActiveUser"]);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(((project)result.Model).projectID, projectToDel.projectID);
            Assert.AreEqual(((project)result.Model).pName, projectToDel.pName);
            Assert.AreEqual(((project)result.Model).pDesc, projectToDel.pDesc);
            Assert.AreEqual(((project)result.Model).pAbstract, projectToDel.pAbstract);
            Assert.AreEqual(((project)result.Model).projectFile, projectToDel.projectFile);
            Assert.AreEqual(((project)result.Model).projectStage, projectToDel.projectStage);

            db.projects.Remove(projectToDel);
            db.users.Remove(RISToDel);
            db.SaveChanges();
        }

        [TestMethod]
        public void RISDetailsRedirectTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempDataRIS = new TempDataDictionary();
            TempDataDictionary tempDataResearcher = new TempDataDictionary();

            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var ResearcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            var projectToDelRIS = DatabaseInsert.AddTestProject(RISToDel, db);
            var projectToDelResearcher = DatabaseInsert.AddTestProject(ResearcherToDel, db);

            tempDataRIS["ActiveUser"] = RISToDel;
            tempDataResearcher["ActiveUser"] = ResearcherToDel;

            RISController ResearchRISController = new RISController();
            ResearchRISController.TempData = tempDataResearcher;

            RISController RISController = new RISController();
            RISController.TempData = tempDataRIS;

            // Act
            RedirectToRouteResult resultResearcher = (RedirectToRouteResult)ResearchRISController.Details(projectToDelRIS.projectID) as RedirectToRouteResult;
            ViewResult resultRIS = (ViewResult)RISController.Details(projectToDelResearcher.projectID) as ViewResult;

            db.projects.Remove(projectToDelRIS);
            db.projects.Remove(projectToDelResearcher);
            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultResearcher);
            Assert.IsTrue(resultResearcher.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultResearcher.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultResearcher.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultResearcher.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultRIS);
            Assert.AreEqual("Details", resultRIS.ViewName);
        }

        /////////////////////////////////*Details TESTS*/////////////////////////////////

    }
}
