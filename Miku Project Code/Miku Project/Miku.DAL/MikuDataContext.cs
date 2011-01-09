using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Miku.DAL.Entities;
using System.Configuration;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace Miku.DAL
{
    public class MikuDataContext:DataContext
    {
        public Table<SystemUser> SystemUsers;

        public MikuDataContext(string connection )
            :base(connection)
        {
            //string connectionStrings =
            //    ConfigurationManager.ConnectionStrings["MikuDB"].ConnectionString;
        }

        [Function(Name = "P_USER_BYLOGINNAME")]
        public IExecuteResult GetUserByName
        ([Parameter(DbType="nvarchar(20)")]string userName)
        {
            return this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                userName);
              
        }
    }
}
