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
        }

        void server_OnRegistringEvent(string regMsg)
        {
            Dispatcher.BeginInvoke((Action)(() => { listBoxRecieved.Items.Add(regMsg); }));
        }
    }
}
