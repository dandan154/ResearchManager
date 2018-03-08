using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResearchManager.Tests.Model_Tests
{
    [TestClass]
    public class userUnitTest
    {
        [TestMethod]
        public void testSetGetName()
        {
            user u = new user();
            u.userID = 999;
            Assert.AreEqual(999, u.userID);
        }

        [TestMethod]
        public void testSetGetFName()
        {
            user u = new user();
            u.forename = "Test";
            Assert.AreEqual("Test", u.forename);
        }

        [TestMethod]
        public void testSetGetSName()
        {
            user u = new user();
            u.surname = "Test";
            Assert.AreEqual("Test", u.surname);
        }

        [TestMethod]
        public void testSetGetEmail()
        {
            user u = new user();
            u.Email = "test@test.com";
            Assert.AreEqual("test@test.com", u.Email);
        }

        [TestMethod]
        public void testSetGetStaffPos()
        {
            user u = new user();
            u.staffPosition = "TEST";
            Assert.AreEqual("TEST", u.staffPosition);
        }

        [TestMethod]
        public void testSetGetStaffHash()
        {
            user u = new user();
            u.hash = "TEST";
            Assert.AreEqual("TEST", u.hash);
        }

        [TestMethod]
        public void testSetGetStaffPOs()
        {
            user u = new user();
            project p = new project();
            u.projects.Add(p);
            Assert.AreEqual(u.projects.Contains(p), true);
        }
    }
}
