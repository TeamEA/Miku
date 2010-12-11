using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Miku.DAL.Entities;
using System.Data.Linq;

namespace Miku.DAL.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Guid userID = Guid.NewGuid();
            string connectionStrings =
                ConfigurationManager.ConnectionStrings["MikuDB"].ConnectionString;
            MikuDataContext dc = new MikuDataContext(connectionStrings);
            dc.SystemUsers.InsertOnSubmit(new Entities.SystemUser
            {
                UserID = userID,
                IPAddress = "",
                LoginName = "gh",
                Port = 1,
                UserState = 1
            });
            dc.SubmitChanges();
            IExecuteResult user = dc.GetUserByName("gh");
            object o= user.ReturnValue;
            //Assert.AreEqual("gh", );
            //dc.SystemUsers.DeleteOnSubmit(user);
            //dc.SubmitChanges();
        }
    }
}
