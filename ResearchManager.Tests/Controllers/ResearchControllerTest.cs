using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResearchManager;
using ResearchManager.Controllers;
using System.Data.Entity;
using System.IO;

namespace ResearchManager.Tests.Controllers
{
    [TestClass]
    public class ResearchControllerTest
    {
        /////////////////////////////////INDEX TESTS/////////////////////////////////
        [TestMethod]
        public void ResearcherIndexTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var researcherToDel = DatabaseInsert.AddTestUser("Researcher", db);

            var projectToDel = DatabaseInsert.AddTestProject(researcherToDel, db);
            tempData["ActiveUser"] = researcherToDel;

            ResearchController controller = new ResearchController();
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
            db.users.Remove(researcherToDel);
            db.SaveChanges();
        }

        [TestMethod]
        public void ResearcherIndexRedirectTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempDataRIS = new TempDataDictionary();
            TempDataDictionary tempDataResearcher = new TempDataDictionary();

            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var ResearcherToDel = DatabaseInsert.AddTestUser("Researcher", db);


            tempDataRIS["ActiveUser"] = RISToDel;
            tempDataResearcher["ActiveUser"] = ResearcherToDel;

            ResearchController RISResearchController = new ResearchController();
            RISResearchController.TempData = tempDataRIS;

            ResearchController ResearchController = new ResearchController();
            ResearchController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultRIS = (RedirectToRouteResult)RISResearchController.Index() as RedirectToRouteResult;
            ViewResult resultResearcher = (ViewResult)ResearchController.Index() as ViewResult;


            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultRIS);
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultRIS.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultRIS.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultResearcher);
            Assert.AreEqual("Index", resultResearcher.ViewName);
        }

        /////////////////////////////////*INDEX TESTS*/////////////////////////////////

        /////////////////////////////////Details TESTS/////////////////////////////////
        [TestMethod]
        public void ResearcherDetailsTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var researcherToDel = DatabaseInsert.AddTestUser("Researcher", db);

            var projectToDel = DatabaseInsert.AddTestProject(researcherToDel, db);
            tempData["ActiveUser"] = researcherToDel;

            ResearchController controller = new ResearchController();
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
            db.users.Remove(researcherToDel);
            db.SaveChanges();
        }

        [TestMethod]
        public void ResearcherDetailsRedirectTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempDataRIS = new TempDataDictionary();
            TempDataDictionary tempDataResearcher = new TempDataDictionary();

            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var ResearcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            var projectToDelRIS = DatabaseInsert.AddTestProject(RISToDel, db);
            var projectToDel = DatabaseInsert.AddTestProject(ResearcherToDel, db);

            tempDataRIS["ActiveUser"] = RISToDel;
            tempDataResearcher["ActiveUser"] = ResearcherToDel;

            ResearchController RISResearchController = new ResearchController();
            RISResearchController.TempData = tempDataRIS;

            ResearchController ResearchController = new ResearchController();
            ResearchController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultRIS = (RedirectToRouteResult)RISResearchController.Details(projectToDelRIS.projectID) as RedirectToRouteResult;
            ViewResult resultResearcher = (ViewResult)ResearchController.Details(projectToDel.projectID) as ViewResult;

            db.projects.Remove(projectToDelRIS);
            db.projects.Remove(projectToDel);
            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultRIS);
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultRIS.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultRIS.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultResearcher);
            Assert.AreEqual("Details", resultResearcher.ViewName);
        }

        /////////////////////////////////*Details TESTS*/////////////////////////////////

        /////////////////////////////////CreateProject TESTS/////////////////////////////////
        [TestMethod]
        public void CreateProjectTest()
        {
            Entities db = new Entities();
            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var researcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            tempData["ActiveUser"] = researcherToDel;
            ResearchController controller = new ResearchController
            {
                TempData = tempData
            };

            // Act
            ViewResult result = controller.CreateProject() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("CreateProject", result.ViewName);
            Assert.IsNotNull(result.TempData["ActiveUser"]);
            db.users.Remove(researcherToDel);
        }

        [TestMethod]
        public void CreateProjectRedirectTest()
        {
            Entities db = new Entities();

            // Arrange
            TempDataDictionary tempDataRIS = new TempDataDictionary();
            TempDataDictionary tempDataResearcher = new TempDataDictionary();

            var RISToDel = DatabaseInsert.AddTestUser("RIS", db);
            var ResearcherToDel = DatabaseInsert.AddTestUser("Researcher", db);


            tempDataRIS["ActiveUser"] = RISToDel;
            tempDataResearcher["ActiveUser"] = ResearcherToDel;

            ResearchController RISResearchController = new ResearchController();
            RISResearchController.TempData = tempDataRIS;

            ResearchController ResearchController = new ResearchController();
            ResearchController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultRIS = (RedirectToRouteResult)RISResearchController.CreateProject() as RedirectToRouteResult;
            ViewResult resultResearcher = (ViewResult)ResearchController.CreateProject() as ViewResult;


            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultRIS);
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultRIS.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultRIS.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultResearcher);
            Assert.AreEqual("CreateProject", resultResearcher.ViewName);
        }

        /* [TestMethod]
         public void CreateProjectPOSTTest()
         {
             Entities db = new Entities();
             ResearchController controller = new ResearchController();
             user uToDel = DatabaseInsert.AddTestUser("Researcher", db);
             var p = new project
             {
                 userID = uToDel.userID,
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
         */
        /////////////////////////////////*CreateProject TESTS*/////////////////////////////////

        /////////////////////////////////EditProject TESTS/////////////////////////////////
        [TestMethod]
        public void EditProjectTest()
        {
            Entities db = new Entities();
            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var researcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            tempData["ActiveUser"] = researcherToDel;
            var pToDel = DatabaseInsert.AddTestProject(researcherToDel, db);
            ResearchController controller = new ResearchController
            {
                TempData = tempData
            };

            // Act
            ViewResult result = controller.EditProject(pToDel.projectID) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("EditProject", result.ViewName);
            Assert.IsNotNull(result.TempData["ActiveUser"]);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(((project)result.Model).projectID, pToDel.projectID);

            db.projects.Remove(pToDel);
            db.users.Remove(researcherToDel);
            db.SaveChanges();
        }

        [TestMethod]
        public void EditProjectRedirectTest()
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

            ResearchController RISResearchController = new ResearchController();
            RISResearchController.TempData = tempDataRIS;

            ResearchController ResearchController = new ResearchController();
            ResearchController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultRIS = (RedirectToRouteResult)RISResearchController.EditProject(pToDelRIS.projectID) as RedirectToRouteResult;
            ViewResult resultResearcher = (ViewResult)ResearchController.EditProject(pToDel.projectID) as ViewResult;

            db.projects.Remove(pToDelRIS);
            db.projects.Remove(pToDel);
            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultRIS);
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultRIS.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultRIS.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultResearcher);
            Assert.AreEqual("EditProject", resultResearcher.ViewName);
        }

        /*[TestMethod]
        public void EditProjectPOSTTest()
        {
            Entities db = new Entities();
            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var researcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            tempData["ActiveUser"] = researcherToDel;
            var pToDel = DatabaseInsert.AddTestProject(researcherToDel, db);
            ResearchController controller = new ResearchController
            {
                TempData = tempData
            };

            // Act
            ViewResult result = controller.EditProject(pToDel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsNotNull(result.TempData["ActiveUser"]);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(((List<project>)result.Model).First().projectID, pToDel.projectID);

            db.projects.Remove(pToDel);
            db.users.Remove(researcherToDel);
            db.SaveChanges();
        }

       /* [TestMethod]
        public void EditProjectPOSTRedirectTest()
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

            ResearchController RISResearchController = new ResearchController();
            RISResearchController.TempData = tempDataRIS;

            ResearchController ResearchController = new ResearchController();
            ResearchController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultRIS = (RedirectToRouteResult)RISResearchController.EditProject(pToDelRIS.projectID) as RedirectToRouteResult;
            ViewResult resultResearcher = (ViewResult)ResearchController.EditProject(pToDel.projectID) as ViewResult;

            db.projects.Remove(pToDelRIS);
            db.projects.Remove(pToDel);
            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultRIS);
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultRIS.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultRIS.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultResearcher);
            Assert.AreEqual("EditProject", resultResearcher.ViewName);
        }
        */
        /////////////////////////////////*EditProject TESTS*/////////////////////////////////


        //////////////////////////////////Download TESTS//////////////////////////////////

        [TestMethod]
        public void DownloadProjectTest()
        {
            Entities db = new Entities();
            // Arrange
            TempDataDictionary tempData = new TempDataDictionary();
            var researcherToDel = DatabaseInsert.AddTestUser("Researcher", db);
            tempData["ActiveUser"] = researcherToDel;
            var pToDel = DatabaseInsert.AddTestProject(researcherToDel, db);
            ResearchController controller = new ResearchController
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
            db.users.Remove(researcherToDel);
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

            ResearchController RISResearchController = new ResearchController();
            RISResearchController.TempData = tempDataRIS;

            ResearchController ResearchController = new ResearchController();
            ResearchController.TempData = tempDataResearcher;

            // Act
            RedirectToRouteResult resultRIS = (RedirectToRouteResult)RISResearchController.EditProject(pToDelRIS.projectID) as RedirectToRouteResult;
            FileResult resultResearcher = ResearchController.Download(pToDel.projectID) as FileResult;

            db.projects.Remove(pToDelRIS);
            db.projects.Remove(pToDel);
            db.users.Remove(RISToDel);
            db.users.Remove(ResearcherToDel);
            db.SaveChanges();

            // Assert 'Other User'
            Assert.IsNotNull(resultRIS);
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("action"));
            Assert.IsTrue(resultRIS.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", resultRIS.RouteValues["action"].ToString());
            Assert.AreEqual("Home", resultRIS.RouteValues["controller"].ToString());

            //Assert Researcher
            Assert.IsNotNull(resultResearcher);
            Assert.AreEqual(pToDel.pName + "-ExpenditureFile" + Path.GetExtension(pToDel.projectFile), resultResearcher.FileDownloadName);
            Assert.AreEqual("application/" + Path.GetExtension(pToDel.projectFile), resultResearcher.ContentType);
        }


        /////////////////////////////////*Download TESTS*/////////////////////////////////


        //////////////////////////////////addToHistory TESTS//////////////////////////////////
        [TestMethod]
        public void addToHistoryTest()
        {
            //Arrange
            Entities db = new Entities();
            var tempUser = DatabaseInsert.AddTestUser("Researcher", db);
            var tempProject = DatabaseInsert.AddTestProject(tempUser, db);
            string changeSum = "Test Change Sum";

            //ACT
            HelperClasses.SharedControllerMethods.addToHistory(tempUser.userID, tempProject.projectID, changeSum);
            Entities db2 = new Entities();

            var found = db2.projects.Find(tempProject.projectID).changes.First().changeSummary;

            //Assert
            Assert.AreEqual(changeSum, found);

            //Clean Up

            db2.changes.Remove(db2.projects.Find(tempProject.projectID).changes.First());
            db2.projects.Remove(db2.projects.Find(tempProject.projectID));
            db2.users.Remove(db2.users.Find(tempUser.userID));
            db2.SaveChanges();
        }
        /////////////////////////////////*addToHistory TESTS*/////////////////////////////////
    }
}
