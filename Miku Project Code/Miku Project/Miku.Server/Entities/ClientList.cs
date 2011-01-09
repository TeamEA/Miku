using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miku.Server.Entities
{
    public class ClientList
    {
        public static Dictionary<string, Client> userList = new Dictionary<string, Client>();

        public static Dictionary<string, Client> GetUserList()
        {
            return userList;
        }

    }
}
