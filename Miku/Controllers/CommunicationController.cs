using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.Client.Models.Communication;
using Miku.Client.Views;

namespace Miku.Client.Controllers
{
    public class CommunicationController : ICommunicationController
    {
        private CommunicationClient communicationClient;
        private ICommunicationView communicationView;

        public CommunicationController(ICommunicationView communicationClient)
        {
            this.communicationClient = new CommunicationClient();
            this.communicationView = communicationClient;

            this.communicationClient.RecieveMsgEvent += this.communicationView.ResponseRecieveMsg;

        }

        public void RequestRecieveMsg()
        {
            communicationClient.SendMsgToOneThroughIPEndPoint(this.communicationView.SendingMsg,new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 11002));
        }

        public void RequestRecieveFile()
        {
            throw new NotImplementedException();
        }
    }
}
