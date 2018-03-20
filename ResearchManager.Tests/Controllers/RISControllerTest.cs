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
using System.IO;

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

            //Assert RIS
            Assert.IsNotNull(resultRIS);
            Assert.AreEqual("Index", resultRIS.ViewName);
        }

        /////////////////////////////////*INDEX TESTS*/////////////////////////////////

        /////////////////////////////////ReUpload TESTS/////////////////////////////////
        [TestMethod]
        public void ReUploadTest()
        {
            Entities db = new Entities();
            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var projectToDel = DatabaseInsert.AddTestProject(RISToDel, db);
            tempData["ActiveUser"] = RISToDel;
            RISController controller = new RISController
            {
                TempData = tempData
            };

            // Act
            ViewResult result = controller.ReuploadExpend(projectToDel.projectID) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ReuploadExpend", result.ViewName);
            Assert.IsNotNull(result.TempData["ActiveUser"]);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(((project)result.Model).projectID, projectToDel.projectID);
            db.projects.Remove(projectToDel);
            db.users.Remove(RISToDel);
        }

        [TestMethod]
        public void ReUploadRedirectTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempDataRIS = new TempDataDictionary();
            TempDataDictionary tempDataResearcher = new TempDataDictionary();

            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var ResearcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            var RISprojectToDel = DatabaseInsert.AddTestProject(RISToDel, db);
            var ResearcherprojectToDel = DatabaseInsert.AddTestProject(ResearcherToDel, db);


            tempDataRIS["ActiveUser"] = RISToDel;
            tempDataResearcher["ActiveUser"] = ResearcherToDel;

            RISController RISController = new RISController();
            RISController.TempData = tempDataRIS;

            RISController ResearchController = new RISController();
            ResearchController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultResearcher = (RedirectToRouteResult)ResearchController.ReuploadExpend(ResearcherprojectToDel.projectID) as RedirectToRouteResult;
            ViewResult resultRIS = (ViewResult)RISController.ReuploadExpend(RISprojectToDel.projectID) as ViewResult;

            db.projects.Remove(ResearcherprojectToDel);
            db.projects.Remove(RISprojectToDel);
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
            Assert.AreEqual("ReuploadExpend", resultRIS.ViewName);
        }
        /////////////////////////////////*ReUpload TESTS*/////////////////////////////////

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

            //Assert RIS
            Assert.IsNotNull(resultRIS);
            Assert.AreEqual("Details", resultRIS.ViewName);
        }

        /////////////////////////////////*Details TESTS*/////////////////////////////////

        //////////////////////////////////Download TESTS//////////////////////////////////

        [TestMethod]
        public void DownloadProjectTest()
        {
            Entities db = new Entities();
            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            tempData["ActiveUser"] = RISToDel;
            var pToDel = DatabaseInsert.AddTestProject(RISToDel, db);
            RISController controller = new RISController
            {
                TempData = tempData
            };

            // Act
            FileResult result = controller.Download(pToDel.projectID) as FileResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pToDel.pName + "-ExpenditureFile" + Path.GetExtension(pToDel.projectFile), result.FileDownloadName);
            Assert.AreEqual("application/" + Path.GetExtension(pToDel.projectFile), result.ContentType);

            db.projects.Remove(pToDel);
            db.users.Remove(RISToDel);
            db.SaveChanges();
        }

        [TestMethod]
        public void DownloadProjectRedirectTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempDataRIS = new TempDataDictionary();
            TempDataDictionary tempDataResearcher = new TempDataDictionary();

            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var ResearcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            var pToDel = DatabaseInsert.AddTestProject(ResearcherToDel, db);
            var pToDelRIS = DatabaseInsert.AddTestProject(RISToDel, db);


            tempDataRIS["ActiveUser"] = RISToDel;
            tempDataResearcher["ActiveUser"] = ResearcherToDel;

            RISController ResearchRISController = new RISController();
            ResearchRISController.TempData = tempDataResearcher;

            RISController RISController = new RISController();
            RISController.TempData = tempDataRIS;

            // Act
            FileResult resultResearcher = (FileResult)ResearchRISController.Download(pToDel.projectID) as FileResult;
            FileResult resultRIS = RISController.Download(pToDel.projectID) as FileResult;

            db.projects.Remove(pToDelRIS);
            db.projects.Remove(pToDel);
            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNull(resultResearcher);

            //Assert RIS
            Assert.IsNotNull(resultRIS);
            Assert.AreEqual(pToDelRIS.pName + "-ExpenditureFile" + Path.GetExtension(pToDelRIS.projectFile), resultRIS.FileDownloadName);
            Assert.AreEqual("application/" + Path.GetExtension(pToDelRIS.projectFile), resultRIS.ContentType);
        }


        /////////////////////////////////*Download TESTS*/////////////////////////////////
    }
}
