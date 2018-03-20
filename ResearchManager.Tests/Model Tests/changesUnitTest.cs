using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResearchManager.Tests.Model_Tests
{
    [TestClass]
    class changesUnitTest
    {
        [TestMethod]
        public void testChangeID()
        {
            change c = new change();
            c.changeID = 999;
            Assert.AreEqual(999, c.changeID); 
        }
        [TestMethod]
        public void testUserID()
        {
            change c = new change();
            c.userID = 999;
            Assert.AreEqual(999, c.userID);
        }
        [TestMethod]
        public void testProjectID()
        {
            change c = new change();
            c.projectID = 999;
            Assert.AreEqual(999, c.projectID);
        }

        public void testDateCreated()
        {
            change c = new change();
            DateTime dt = DateTime.Now;
            c.dateCreated = dt;
            Assert.IsNotNull(dt); 
            Assert.AreEqual(dt, c.dateCreated); 
        }

        public void testChangeSummary()
        {
            change c = new change();
            c.changeSummary = "test";
            Assert.AreEqual("test", c.changeSummary); 
        }
    }
}
