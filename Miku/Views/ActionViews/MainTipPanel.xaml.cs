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
using Miku.Client.Controllers;

namespace Miku.Client.Views.ActionViews
{
    /// <summary>
    /// Interaction logic for MainTipPanel.xaml
    /// </summary>
    public partial class MainTipPanel : StackPanel
    {
        private RecordStrategies recordStrategy;
        private ActionView parentView;
        public RecordStrategies RecordStrategy
        {
            private set { this.recordStrategy = value; }
            get { return this.recordStrategy; }
        }
        
        public MainTipPanel()
        {
            InitializeComponent();            
        }

        private void rdb_Click(object sender, RoutedEventArgs e)
        {
            if (rdbMouseAndKeyboard.IsChecked == true)
            {
                this.parentView.RecordStrategie = RecordStrategies.MouseAndKeyboard;
            }
            else if (rdbMouse.IsChecked == true)
            {
                this.parentView.RecordStrategie = RecordStrategies.Mouse;
            }
            else if (rdbKeyboard.IsChecked == true)
            {
                this.parentView.RecordStrategie = RecordStrategies.Keyboard;
            }
            this.parentView.GotoRecorderView();
        }
        internal void ParentView(ActionView actionView)
        {
            this.parentView = actionView;
        }

        internal void Initialize()
        {
            rdbKeyboard.IsChecked = false;
            rdbMouse.IsChecked = false;
            rdbMouseAndKeyboard.IsChecked = false;
        }
    }
}
