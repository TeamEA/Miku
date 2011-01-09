using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Miku.Client.Views
{
    public delegate void OnResponseSendingActionEventHandler(object action,string targetUserName);

    /// <summary>
    /// The interface that the ActionView need to implement
    /// </summary>
    public interface ICommunicationView
    {
        void ResponseRecieveMsg(string msg);
        void ResponseRecieveFile(out string filePath,string fileName);
        void ResponseSendFile();
        void OnResponseRecievedControlImage(MemoryStream screenImageStream);
        void OnResponseRecievedControlCommand(object command);
        event OnResponseSendingActionEventHandler OnResponseSendingActionEvent;
        void ResponseEndRecieveFile();
        string SendingMsg{get;}
        string FilePath { get; set; }
        IPEndPoint ipEndPoint { get;}
        int ListeningPort { get; }

        string SenderName { get; }
        string RecieverName { get; }
    }
}
