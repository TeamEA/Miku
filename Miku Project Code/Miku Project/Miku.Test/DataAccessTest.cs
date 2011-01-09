using Miku.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Rhino.Mocks;

namespace Miku.DAL.Test
{
    
    
    /// <summary>
    ///This is a test class for DataAccessTest and is intended
    ///to contain all DataAccessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataAccessTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AddSystemUser
        ///</summary>
        [TestMethod()]
        public void AddSystemUserTest()
        {
            DataAccess target = new DataAccess();
            SystemUser sysUser = null;
            bool expected = false;
            bool actual;
            actual = target.AddSystemUser(sysUser);
            Assert.AreEqual(expected, actual);

            sysUser = new SystemUser { LoginName = "gh", UserID = Guid.NewGuid(), Port = 11, UserState = 1, IPAddress = "127.0.0.1" };
            expected = true;
            actual = target.AddSystemUser(sysUser);
            Assert.AreEqual(expected, actual);

            actual = SystemUser.DeleteSystemUser(sysUser);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DeleteSystemUser
        ///</summary>
        [TestMethod()]
        public void DeleteSystemUserTest()
        {
            DataAccess target = new DataAccess();
            SystemUser sysUser = null;
            bool expected = false;
            bool actual;
            actual = target.DeleteSystemUser(sysUser);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetSystemUserByLoginName
        ///</summary>
        [TestMethod()]
        public void GetSystemUserByLoginNameTest()
        {
            DataAccess target = new DataAccess();
            SystemUser sysUser = new SystemUser { LoginName = "gh", UserID = Guid.NewGuid(), Port = 11, UserState = 1, IPAddress = "127.0.0.1" };
            target.AddSystemUser(sysUser);

            string loginName = "gh";
            SystemUser expected = sysUser;
            SystemUser actual;
            actual = target.GetSystemUserByLoginName(loginName);
            Assert.AreEqual(expected, actual);

            target.DeleteSystemUser(sysUser);
        }

        /// <summary>
        ///A test for UpdateSystemUser
        ///</summary>
        [TestMethod()]
        public void UpdateSystemUserTest()
        {
            DataAccess target = new DataAccess();

            SystemUser sysUser = new SystemUser { LoginName = "gh", UserID = Guid.NewGuid(), Port = 11, UserState = 1, IPAddress = "127.0.0.1" };
            target.AddSystemUser(sysUser);

            bool expected = true;
            bool actual;

            sysUser.Port = 12;
            actual = target.UpdateSystemUser();
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(12, SystemUser.GetSystemUserByLoginName("gh").Port);

            target.DeleteSystemUser(sysUser);
        }
    }
}
