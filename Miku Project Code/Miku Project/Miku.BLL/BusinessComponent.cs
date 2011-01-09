using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.DAL;

namespace Miku.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class BusinessComponent
    {
        private DataAccess da = new DataAccess();

        /// <summary>
        /// Adds the system user.
        /// </summary>
        /// <param name="sysUser">The sys user.</param>
        /// <returns></returns>
        public bool AddSystemUser(SystemUser sysUser)
        {
            return da.AddSystemUser(sysUser);
        }

        /// <summary>
        /// Deletes the system user.
        /// </summary>
        /// <param name="sysUser">The sys user.</param>
        /// <returns></returns>
        public bool DeleteSystemUser(SystemUser sysUser)
        {
            return da.DeleteSystemUser(sysUser);
        }


        /// <summary>
        /// Gets the name of the system user by login.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns></returns>
        public SystemUser GetSystemUserByLoginName(string loginName)
        {
            return da.GetSystemUserByLoginName(loginName);
        }

        /// <summary>
        /// Updates the system user.
        /// </summary>
        /// <returns></returns>
        public bool UpdateSystemUser()
        {
            return da.UpdateSystemUser();
        }
    }
}
