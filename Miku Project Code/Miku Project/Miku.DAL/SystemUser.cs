using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miku.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SystemUser
    {
        private static MikuDataContext mikuDataContext = 
            new MikuDataContext();

        /// <summary>
        /// Adds the system user.
        /// </summary>
        /// <param name="sysUser">The sys user.</param>
        /// <returns></returns>
        public static bool AddSystemUser(SystemUser sysUser)
        {
            try
            {
                mikuDataContext.SystemUsers.InsertOnSubmit(sysUser);
                mikuDataContext.SubmitChanges();
                return  true;
            }
            catch (Exception e)
            {
                return false;
            }
           
        }

        /// <summary>
        /// Deletes the system user.
        /// </summary>
        /// <param name="sysUser">The sys user.</param>
        /// <returns></returns>
        public static bool DeleteSystemUser(SystemUser sysUser)
        {
            try
            {
                mikuDataContext.SystemUsers.DeleteOnSubmit(sysUser);
                mikuDataContext.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }

        /// <summary>
        /// Gets the name of the system user by login.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns></returns>
        public static SystemUser GetSystemUserByLoginName(string loginName)
        {
            try
            {
                return mikuDataContext.SystemUsers.First(u => u.LoginName == loginName);
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
        /// <summary>
        /// Updates the system user.
        /// </summary>
        /// <returns></returns>
        public static bool UpdateSystemUser()
        {
            try
            {
                mikuDataContext.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
            
        }
    }
}
