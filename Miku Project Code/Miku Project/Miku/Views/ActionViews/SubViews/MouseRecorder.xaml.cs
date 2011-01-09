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
using System.Windows.Interop;
using System.Windows.Forms;
using Miku.Client.Controllers;


namespace Miku.Client.Views.ActionViews
{
    /// <summary>
    /// Interaction logic for MouseRecorder.xaml
    /// </summary>
    public partial class MouseRecorder : StackPanel
    {
        private ActionView parentView;
        public MouseRecorder()
        {
            InitializeComponent();
        }
        internal void ParentView(ActionView actionView)
        {
            this.parentView = actionView;
        }

        private void BackHome_Click(object sender, RoutedEventArgs e)
        {
            parentView.NavegateToSelect();
        }

        #region 功能按钮点击事件
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {            
            this.parentView.actionController = new ActionController(this.parentView, this.parentView.recordStrategy);
            this.parentView.actionController.RequestStartRecordActions();
        }

        private void PlaybackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.parentView.HasActions)            
                this.parentView.actionController.RequestStartPlayback();
        }
        private void LoadfileButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void SavefileButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentView.actionController.RequestSaveActions();
        }
        #endregion       

    }
}
