using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miku.DAL
{
    public class DataAccess
    {
        /// <summary>
        /// Adds the system user.
        /// </summary>
        /// <param name="sysUser">The sys user.</param>
        /// <returns></returns>
        public  bool AddSystemUser(SystemUser sysUser)
        {
            return SystemUser.AddSystemUser(sysUser);
        }
        /// <summary>
        /// Deletes the system user.
        /// </summary>
        /// <param name="sysUser">The sys user.</param>
        /// <returns></returns>
        public  bool DeleteSystemUser(SystemUser sysUser)
        {
            return SystemUser.DeleteSystemUser(sysUser);
        }
        /// <summary>
        /// Gets the name of the system user by login.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns></returns>
        public  SystemUser GetSystemUserByLoginName(string loginName)
        {
            return SystemUser.GetSystemUserByLoginName(loginName);
        }
        /// <summary>
        /// Updates the system user.
        /// </summary>
        /// <returns></returns>
        public  bool UpdateSystemUser()
        {
            return SystemUser.UpdateSystemUser();
        }
    }
}
