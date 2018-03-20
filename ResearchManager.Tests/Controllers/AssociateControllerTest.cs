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
    public class AssociateControllerTest

    {
        [TestMethod]
        public void AssociateDetailsRedirect()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Associate Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser;
            DeanController associateNullProject = new DeanController();
            DeanController associateNullUser = new DeanController();
            associateNullProject.TempData = tempData;

            //remove test project before usage
            db.projects.Remove(testProject);
            db.SaveChanges();

            //Return view with invalid projectID
            RedirectToRouteResult result = (RedirectToRouteResult)associateNullProject.Details(testProject.projectID);
            RedirectToRouteResult result2 = (RedirectToRouteResult)associateNullUser.Details(testProject.projectID);
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main Tests
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("ControllerChange", result.RouteValues["action"].ToString());
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());

            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result2.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("SignIn", result2.RouteValues["action"].ToString());
            Assert.AreEqual("Home", result2.RouteValues["controller"].ToString());
        }

        [TestMethod]
        public void AssociateDetailsStandard()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Associate Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser;
            AssociateController associate = new AssociateController();
            associate.TempData = tempData;

            ViewResult action = (ViewResult)associate.Details(testProject.projectID);

            //Remove testing models from database
            db.projects.Remove(testProject);
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main tests 
            Assert.IsNotNull(action);
            Assert.IsNotNull(action.ViewData.Model);
            Assert.AreEqual(action.TempData["ActiveUser"], testUser);

        }

        [TestMethod]
        public void AssociateIndexRedirect()
        {
            //Connect to database
            Entities db = new Entities();
            AssociateController associateNullUser = new AssociateController();

            //Return view with invalid projectID
            RedirectToRouteResult result = (RedirectToRouteResult)associateNullUser.Index();

            //Main Tests
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("SignIn", result.RouteValues["action"].ToString());
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
        }

        [TestMethod]
        public void AssociateIndexStandard()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Associate Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser;

            AssociateController associate = new AssociateController();
            associate.TempData = tempData;

            ViewResult action = (ViewResult)associate.Index();

            //Remove testing models from database
            db.projects.Remove(testProject);
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main Tests
            Assert.IsNotNull(action);
            Assert.IsNotNull(testUser);
            Assert.IsNotNull(action.ViewData.Model);
            Assert.IsNotNull(action.TempData["ActiveUser"]);
            Assert.AreEqual(action.TempData["ActiveUser"], testUser);
        }
    }
}
