using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.Server.Entities;

namespace Miku.Server.WCFServices
{
    public class RegistrationService : IRegistrationService
    {
        public static event EventHandler<SubscriptionEventArgs> Subscribed;

        private static Dictionary<string, Client> clientList = ClientList.GetUserList();

        private static object obj = new object();

        public void Register(string userName,string uri)
        {
            Uri channelUri = new Uri(uri, UriKind.Absolute);
            Subscribe(userName,channelUri);
        }

        public void Unregister(string userName)
        {
            Unsubscribe(userName);
        }

        #region Subscription/Unsubscribing logic
        private void Subscribe(string userName, Uri channelUri)
        {
            lock (obj)
            {
                if (!clientList.Keys.Contains(userName))
                {
                    clientList.Add(userName,new Client
                    {
                        UserName = userName,
                        ClientIPEndPoint = null,
                        PhoneUri = channelUri
                    });
                }
                else if (clientList[userName].PhoneUri != channelUri)
                {
                    clientList[userName].PhoneUri = channelUri;
                }
            }
            OnSubscribed(clientList[userName]);
        }

        public static void Unsubscribe(string userName)
        {
            Client removedClient = clientList[userName];
            lock (obj)
            {
                clientList.Remove(userName);
            }
            OnSubscribed(removedClient);
        }
        #endregion

        #region Helper private functionality
        private static void OnSubscribed(Client client)
        {
            EventHandler<SubscriptionEventArgs> handler = Subscribed;
            if (handler != null)
            {
                handler(null,
                  new SubscriptionEventArgs( client));
            }
        }
        #endregion

        #region Internal SubscriptionEventArgs class definition
        public class SubscriptionEventArgs : EventArgs
        {
            public SubscriptionEventArgs(Client client)
            {
                this.client = client; 
            }

            public Client client { get; set; }
        }
        #endregion

    }
}