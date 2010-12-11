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
using System.Windows.Shapes;
using Miku.Client.Models.Communication;
using Miku.Client.Controllers;
using System.Threading;

namespace Miku.Client.Views
{
    /// <summary>
    /// Interaction logic for TestRemoteCommunicationView.xaml
    /// </summary>
    public partial class TestRemoteCommunicationView : Window, ICommunicationView
    {

        private CommunicationController communicationController;
        private SynchronizationContext synchronizationContext;

        public TestRemoteCommunicationView()
        {
            InitializeComponent();
            communicationController =   new CommunicationController(this);
            synchronizationContext = SynchronizationContext.Current;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            communicationController.RequestRecieveMsg();
        }

        public void ResponseRecieveMsg(string msg)
        {
            SynchronizationContext.SetSynchronizationContext((SynchronizationContext)synchronizationContext);
            SynchronizationContext.Current.Post(delegate
                 {
                     textBox2.Text += msg;
                 }, null);
        }






        public string SendingMsg
        {
            get { return textBox1.Text; }
        }
    }
}
