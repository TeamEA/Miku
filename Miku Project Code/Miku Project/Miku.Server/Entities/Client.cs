using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Miku.Server.Entities
{
    public class Client
    {
        public string UserName { get; set; }
        public Uri PhoneUri { get; set; }
        public IPEndPoint ClientIPEndPoint { get; set; }
    }
}
