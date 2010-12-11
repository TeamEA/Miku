using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.DAL.Entities;
using System.Data.Linq;

namespace Miku.DAL
{
    public class CompiledQueries
    {
        public static Func<MikuDataContext, Guid, SystemUser>
            UserByUserID =
            CompiledQuery.Compile((MikuDataContext dc, Guid userID) =>
                                    dc.SystemUsers.Single(u => u.UserID == userID)
                        );

        public static Func<MikuDataContext, string, SystemUser>
           UserByUserLoginName =
           CompiledQuery.Compile((MikuDataContext dc, string userLoginName) =>
                                   dc.SystemUsers.Single(u => u.LoginName == userLoginName)
                       );
    }
}
