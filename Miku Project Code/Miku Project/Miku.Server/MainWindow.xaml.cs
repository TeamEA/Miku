using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Miku.Server.Communication;
using Miku.Server.Entities;
using Miku.Server.WCFServices;

namespace Miku.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommunicationServer server;
        public MainWindow()
        {
            InitializeComponent();
            server = new CommunicationServer(11002);
            server.OnRegistringEvent += new CommunicationServer.OnRegistring(server_OnRegistringEvent);
            RegistrationService.Subscribed += new EventHandler<RegistrationService.SubscriptionEventArgs>(RegistrationService_Subscribed);
        }

        void RegistrationService_Subscribed(object sender, RegistrationService.SubscriptionEventArgs e)
        {
            string regMsg = String.Format("User:{0}\nPC@{1}\nPhone@{2}", e.client.UserName, e.client.ClientIPEndPoint, e.client.PhoneUri);
            Dispatcher.BeginInvoke((Action)(() => 
            {
                bool find = false;

                foreach (ListBoxItem item in listBoxRecieved.Items)
                {
                    if (item.Name == e.client.UserName)
                    {
                        item.Content = regMsg;
                    }
                    find = true;
                    break;
                }

                if (find == false)
                {
                    listBoxRecieved.Items.Add(new ListBoxItem { Name = e.client.UserName, Content = regMsg });
                }
            }));
            
            
            
        }

        void server_OnRegistringEvent(Client clientInfo)
        {
            string regMsg = String.Format("User:{0}\nPC@{1}\nPhone@{2}",clientInfo.UserName,clientInfo.ClientIPEndPoint,clientInfo.PhoneUri);
            Dispatcher.BeginInvoke((Action)(() =>
            {
                bool find = false;
                foreach (ListBoxItem item in listBoxRecieved.Items)
                {
                    if (item.Name == clientInfo.UserName)
                    {
                        item.Content = regMsg;
                    }
                    find = true;
                    break;
                }

                if (find == false)
                {
                    listBoxRecieved.Items.Add(new ListBoxItem { Name = clientInfo.UserName, Content = regMsg });
                }
            }));
        }
    }
}
