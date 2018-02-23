using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResearchManager.Tests.Model_Tests
{
    [TestClass]
    public class projectUnitTest
    {
        [TestMethod]
        public void testSetGetName()
        {
            project p = new project();
            p.pName = "Test";
            Assert.AreEqual("Test", p.pName);
        }

        [TestMethod]
        public void testSetGetDesc()
        {
            project p = new project();
            p.pDesc = "Test";
            Assert.AreEqual("Test", p.pDesc);
        }

        [TestMethod]
        public void testSetGetAbstract()
        {
            project p = new project();
            p.pAbstract = "Test";
            Assert.AreEqual("Test", p.pAbstract);
        }

        [TestMethod]
        public void testSetGetProjectFile()
        {
            project p = new project();
            p.projectFile = "Test";
            Assert.AreEqual("Test", p.projectFile);
        }

        [TestMethod]
        public void testSetGetProjectStage()
        {
            project p = new project();
            p.projectStage = "Created";
            Assert.AreEqual("Created", p.projectStage);
        }

        [TestMethod]
        public void testSetGetProjectID()
        {
            project p = new project();
            p.projectID = 999;
            Assert.AreEqual(999, p.projectID);
        }
    }
}
