using System;
using System.Collections.Generic;
using System.Text;

namespace Miku.Client.Views
{
    /// <summary>
    /// The interface that the ActionView need to implement
    /// </summary>
    public interface ICommunicationView
    {
        void ResponseRecieveMsg(string msg);
        string SendingMsg{get;}
    }
}
