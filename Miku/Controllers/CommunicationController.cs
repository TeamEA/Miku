using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.Client.Models.Communication;
using Miku.Client.Views;
using Miku.Client.Helpers.ExceptionHandler;
using Miku.Client.Models.Hooks;
using System.Runtime.InteropServices;
using System.Reflection;


namespace Miku.Client.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CommunicationController : ICommunicationController
    {
        private CommunicationClient communicationClient;
        private ICommunicationView communicationView;


        public CommunicationController(ICommunicationView communicationClient)
        {
            this.communicationView = communicationClient;
        }


        /// <summary>
        /// Request  send a MSG.
        /// </summary>
        public void RequestSendMsg()
        {
            try
            {
                communicationClient.SendMsgToOneThroughTargetUserName(this.communicationView.SendingMsg,this.communicationView.RecieverName);
            }
            catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }

        /// <summary>
        /// Requests send a file.
        /// </summary>
        public void RequestSendFile()
        {
            try
            {
                this.communicationView.ResponseSendFile();
                string path = this.communicationView.FilePath;
                this.communicationView.FilePath = String.Empty;

                communicationClient.RequestSendFileToOneThroughTargetUserName(path, this.communicationView.RecieverName,this.communicationView.SenderName);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }


        public void RequestRegisterToServer()
        {
            this.communicationClient = new CommunicationClient(this.communicationView.ipEndPoint,this.communicationView.ListeningPort);

            this.communicationClient.RecieveMsgEvent += this.communicationView.ResponseRecieveMsg;

            this.communicationClient.RecieveFileEvent += this.communicationView.ResponseRecieveFile;
            this.communicationClient.EndRecieveFileEvent += this.communicationView.ResponseEndRecieveFile;

            this.communicationClient.OnRecievedControlImageEvent += this.communicationView.OnResponseRecievedControlImage;
            this.communicationClient.OnRecievedControlCommandEvent += this.communicationView.OnResponseRecievedControlCommand;

            this.communicationClient.Register(this.communicationView.SenderName,new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"),this.communicationView.ListeningPort));
        }


        public void RequestRemoteControl()
        {
            this.communicationView.OnResponseSendingActionEvent += 
                this.communicationClient.SendControlCommandToOneThroughTargetUserName;
        }

        public void RequestAcceptRemoteControl()
        {
            this.communicationClient.BeginSendBackgroundImageToOneThroughTargetUserName( this.communicationView.RecieverName);
        }

        public void RequestPlayRemoteScript()
        {
            throw new NotImplementedException();
        }


        
    }
}
