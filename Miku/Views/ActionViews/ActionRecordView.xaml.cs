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
using System.Windows.Forms;
using System.Windows.Interop;
using Miku.Client.Views.UserControl;

namespace Miku.Client.Views.ActionViews
{
    /// <summary>
    /// Interaction logic for ActionRecordView.xaml
    /// </summary>
    public partial class ActionRecordView : Window,IActionView
    {
        private IActionController actionController;
        private RecordStrategies recordStrategy;
        private System.Windows.Forms.NotifyIcon notifyIcon;

        public ActionRecordView()
        {
            InitializeComponent();
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            AddNotifyIcon(this.notifyIcon, "点控动作录制", "Actions.ico");
        }

        private void btnStartRecord_Click(object sender, RoutedEventArgs e)
        {
            string strTipText = notifyIcon.BalloonTipText;
            notifyIcon.BalloonTipText = "开始动作录制。按F10键停止录制！";
            notifyIcon.ShowBalloonTip(5000);
            actionController = new ActionController(this, this.recordStrategy);            
            this.actionController.RequestStartRecordActions();
            notifyIcon.BalloonTipText = strTipText;
        }

        private void btnPlayBack_Click(object sender, RoutedEventArgs e)
        {
            string strTipText = notifyIcon.BalloonTipText;
            notifyIcon.BalloonTipText = "开始动作录制回放.";
            notifyIcon.ShowBalloonTip(5000);
            this.actionController.RequestStartPlayback();
            notifyIcon.BalloonTipText = strTipText;
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            this.actionController.RequestSaveActions();
        }

        #region IActionView
        public void ResponseStartRecording()
        {           
            this.Hide();
        }

        public void ResponseStopRecording()
        {
            this.Show();
        }

        public void ResponseKeepRecording(string recordInfo)
        {
            
        }

        public void ResponseStartPlayback()
        {           
            this.Hide();
        }

        public void ResponseSaveActions(ref string filepath)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "MikuActionScript(*.mact)|*.mact";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filepath = dialog.FileName;
            }
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            recordStrategy = RecordStrategies.MouseAndKeyboard;

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd != IntPtr.Zero)
            {
                HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(this.MessageHookHandler));
            }
            Win32API.RegisterHotKey((new WindowInteropHelper(this)).Handle, 247696411, 0, (UInt32)Keys.F10); //注册热键 
        }

        private IntPtr MessageHookHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //window消息定义的注册的按键消息
            if (msg.Equals(0x0312) && (wParam.ToInt32() == 247696411))
            {
                this.actionController.RequestStopRecordActions();
            }
            return IntPtr.Zero;
        }

        private void rdb_Click(object sender, RoutedEventArgs e)
        {
            if (rdbMouseAndKeyboard.IsChecked == true)
            {
                this.recordStrategy = RecordStrategies.MouseAndKeyboard;
            }
            else if (rdbMouse.IsChecked == true)
            {
                this.recordStrategy = RecordStrategies.Mouse;
            }
            else if (rdbKeyboard.IsChecked == true)
            {
                this.recordStrategy = RecordStrategies.Keyboard;
            }
        }

        #region 最小、最大、关闭、系统控制
        protected override void OnClosed(EventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosed(e);
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnMinSize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnMaxSize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            YesNoMessageBox messageBox = new YesNoMessageBox("是否这的要关闭系统？");
            messageBox.ShowDialog();
            if (messageBox.YesOrNo == YesNoMessageBox.YesAndNo.Yes)
            {
                this.WindowClose();
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            this.Help();
        }

        private void btnContrct_Click(object sender, RoutedEventArgs e)
        {
            this.Contract();
        }
        
        #endregion

        #region 自定义函数
        private void AddNotifyIcon(System.Windows.Forms.NotifyIcon notifyIcon, string tooltip, string icoName)
        {
            notifyIcon.BalloonTipText = tooltip;
            notifyIcon.Text = tooltip;
            notifyIcon.Icon = new System.Drawing.Icon(icoName);
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);
            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem closeItem = new System.Windows.Forms.MenuItem();
            closeItem.Text = "退出";
            closeItem.Click += new EventHandler(delegate { this.WindowClose(); });

            System.Windows.Forms.MenuItem helpItem = new System.Windows.Forms.MenuItem();
            helpItem.Text = "帮助";
            helpItem.Click += new EventHandler(delegate { this.Help(); });
            menu.MenuItems.Add(helpItem);
            menu.MenuItems.Add(closeItem);
            notifyIcon.ContextMenu = menu;
        }

        private void Help()
        {
            //throw new NotImplementedException();
            AboutSystem aboutSystem = new AboutSystem();
            aboutSystem.ShowDialog();
        }

        private void Contract()
        {
            //throw new NotImplementedException();
        }

        private void WindowClose()
        {
            this.notifyIcon.Visible = false;
            this.notifyIcon.Dispose();
            this.notifyIcon = null;
            this.Close();
        }
        #endregion

    }
}
