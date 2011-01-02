using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Miku.Client.Controllers;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using Miku.Client.Views.UserControl;
using Miku.Client.Helpers;
using System.Collections.Generic;

namespace Miku.Client.Views.ActionViews
{
    /// <summary>
    /// Interaction logic for ActionView.xaml
    /// </summary>
    public partial class ActionView : Window,IActionView
    {       
        public Controllers.IActionController actionController;
        public RecordStrategies recordStrategy;
        private System.Windows.Forms.NotifyIcon notifyIcon;        
        public RecordStrategies RecordStrategie
        {
            private get { return this.recordStrategy; }
            set { this.recordStrategy = value; }
        }
        public ActionView()
        {
            InitializeComponent();
            InitializeActionView();
        }

        #region  初始化ActionView
        private void InitializeActionView()
        {            
            VisualStateManager.GoToElementState(this.LayoutRoot, "Select", true);
            RegisterHotKeys();
            BuildANotifyForActionView("点控动作录制", Miku.Client.Properties.Resources.Actions);
        }
        
        private void RegisterHotKeys()
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd != IntPtr.Zero)
            {
                HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(this.MessageHookHandler));
            }
            Win32API.RegisterHotKey((new WindowInteropHelper(this)).Handle, 247696411, 0, (UInt32)Keys.F10);
        }
        private IntPtr MessageHookHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg.Equals(0x0312) && (wParam.ToInt32() == 247696411))
            {
                this.actionController.RequestStopRecordActions();
                handled = true;
            }
            return IntPtr.Zero;
        }
        #endregion
        
        #region IActionView 实现方法
        public void ResponseStartRecording()
        {
            this.Hide();
            this.UpdateNoyifyIconBalloonTipText("开始动作录制。按F10键停止录制！", 5000, this.notifyIcon);
            
        }

        public void ResponseStopRecording()
        {
            this.Show();
        }

        public void ResponseKeepRecording(string recordInfo)
        {
            RecordWindow.lbxRecordList.Items.Add(recordInfo);
        }


        public void ResponseStartPlayback()
        {
            this.Hide();
            this.UpdateNoyifyIconBalloonTipText("开始动作录制回放.", 5000, this.notifyIcon);           
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

        public void ResponsePlaybackExistFile(ref string filepath)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "MikuActionScript(*.mact)|*.mact";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filepath = dialog.FileName;
            }
        }
        #endregion

        #region 导航
        internal void GotoRecorderView()
        {
            if (this.recordStrategy == RecordStrategies.Mouse)
            {

            }
            else if (this.recordStrategy == RecordStrategies.Keyboard)
            {
            }
            else if (this.recordStrategy == RecordStrategies.MouseAndKeyboard)
            {

            }
            NavegateToRecord();
        }
        internal void NavegateToSelect()
        {
            this.mainTipPanel.Initialize();
            VisualStateManager.GoToElementState(this.LayoutRoot, "Select", true);

        }

        internal void NavegateToRecord()
        {            
            VisualStateManager.GoToElementState(this.LayoutRoot, "Record", true);
        }
        #endregion

        #region 托盘图标
        private void BuildANotifyForActionView( string tipText, System.Drawing.Icon icon)
        {
            BuildANotify(icon);
            UpdateNoyifyIconBalloonTipText(tipText,5000,notifyIcon);
            AddAMenuToNotify();
        }
        
        private void AddAMenuToNotify()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem closeitem = new MenuItem("退出", new EventHandler(delegate { this.WindowClose(); }));
            menu.MenuItems.Add(closeitem);
            MenuItem aboutitem = new MenuItem("关于", new EventHandler(delegate { this.WindowAbout(); }));
            menu.MenuItems.Add(aboutitem);
            notifyIcon.ContextMenu = menu;
        }

        private MenuItem BuildAMenuItem(string menuText, EventHandler handle)
        {
            
            MenuItem closeItem = new System.Windows.Forms.MenuItem();
            closeItem.Text = menuText;
            closeItem.Click += handle;
            return closeItem;
        }

        private void BuildANotify(System.Drawing.Icon icon)
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = icon;
            notifyIcon.Visible = true;
        }

        private void UpdateNoyifyIconBalloonTipText(string tipText, int time, NotifyIcon notifyIcon)
        {
            string strTipText = notifyIcon.BalloonTipText;
            notifyIcon.BalloonTipText = tipText;
            notifyIcon.Text = tipText;
            notifyIcon.ShowBalloonTip(time);
            notifyIcon.BalloonTipText = strTipText;
            notifyIcon.Text = strTipText;
        }

        #endregion

        #region 系统方法
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainTipPanel.ParentView(this);
            RecordWindow.ParentView(this);
        }
        private void WindowAbout()
        {
            AboutSystem aboutSystem = new AboutSystem();
            aboutSystem.ShowDialog();
        }
        private void WindowClose()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            notifyIcon = null;
            this.Close();
        }
        #endregion
    }
}
