using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miku.Client.Controllers
{
    public interface ICommunicationController
    {
        void RequestRecieveMsg();
        void RequestRecieveFile();
    }
}
