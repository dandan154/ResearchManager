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
    public class DeanControllerTest

    {
        [TestMethod]
        public void DetailsRedirectTest()
        {
            DeanController dean = new DeanController();


            int m = int.MaxValue;
            ActionResult action = dean.Details(m) as ActionResult;

            Assert.IsNotNull(action);
        }

        [TestMethod]
        public void DetailsStandardTest()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser; 
            DeanController dean = new DeanController();
            dean.TempData = tempData;

            ViewResult action = (ViewResult)dean.Details(testProject.projectID);

            //Remove testing models from database
            db.projects.Remove(testProject);
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main tests 
            Assert.IsNotNull(action);
            Assert.IsNotNull(action.ViewData.Model);
            Assert.AreEqual(action.TempData["ActiveUser"], testUser);

        }

        public void Index()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser;

            DeanController dean = new DeanController();
            dean.TempData = tempData; 
            ViewResult action = (ViewResult)dean.Index();
           
            //Remove testing models from database
            db.projects.Remove(testProject);
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main Tests
            Assert.IsNotNull(action);
            Assert.IsNotNull(action.ViewData.Model);
            Assert.AreEqual(action.TempData["ActiveUser"], testUser);

        }
    }
}
