

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Miku.Server.WCFServices
{
    [ServiceContract]
    public interface IRegistrationService
    {
        [OperationContract, WebGet]
        void Register(string userName, string uri);

        [OperationContract, WebGet]
        void Unregister(string userName);
    }
}