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
using System.Windows.Shell;

namespace Miku.Client.Views.ActionViews
{
    public enum ActionViewState { Idle,Busy};
    /// <summary>
    /// Interaction logic for ActionView.xaml
    /// </summary>
    public partial class ActionView : Window,IActionView
    {
        internal bool HasActions = false;
        public Controllers.IActionController actionController;
        public RecordStrategies recordStrategy;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private ActionViewState actionViewState;
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
            //RegisterHotKeys();
            BuildANotifyForActionView("点控动作录制", Miku.Client.Properties.Resources.Actions);
        }
        
        //private void RegisterHotKeys()
        //{
        //    IntPtr hwnd = new WindowInteropHelper(this).Handle;
        //    if (hwnd != IntPtr.Zero)
        //    {
        //        HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(this.MessageHookHandler));
        //    }
        //    Win32API.RegisterHotKey((new WindowInteropHelper(this)).Handle, 247696411, 0, (UInt32)Keys.F10);
        //}
        //private IntPtr MessageHookHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    if (msg.Equals(0x0312) && (wParam.ToInt32() == 247696411))
        //    {
        //        this.actionController.RequestStopRecordActions();
        //        handled = true;
        //    }
        //    return IntPtr.Zero;
        //}

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            Win32API.RegisterHotKey(hWnd, 247696411, 0, (UInt32)System.Windows.Forms.Keys.Escape);

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }
        public static int WM_HOTKEY = 0x0312;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg.Equals(WM_HOTKEY)&&wParam.ToInt32() == 247696411)
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
            this.UpdateNoyifyIconBalloonTipText("开始动作录制。按F10键停止录制！", 5000, this.notifyIcon);
            this.actionViewState = ActionViewState.Busy;
            this.WindowState = WindowState.Minimized;
            taskbarInfo.ProgressState = TaskbarItemProgressState.Paused;
            taskbarInfo.ProgressValue = 1.0;            
        }

        public void ResponseStopRecording()
        {
            this.actionViewState = ActionViewState.Idle;
            this.WindowState = WindowState.Normal;
            taskbarInfo.ProgressState = TaskbarItemProgressState.Normal;
            taskbarInfo.ProgressValue = 1;            
            this.HasActions = true;
        }

        public void ResponseKeepRecording(string recordInfo)
        {
            RecordWindow.lbxRecordList.Items.Add(recordInfo);
        }


        public void ResponseStartPlayback()
        {
            this.UpdateNoyifyIconBalloonTipText("开始动作录制回放.", 5000, this.notifyIcon);
            this.actionViewState = ActionViewState.Busy;
            this.WindowState = WindowState.Minimized;
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
            OpenFileDialog dialog = new OpenFileDialog();
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
            MenuItem showitem = new MenuItem("显示",  new EventHandler(showitem_Click));           
            menu.MenuItems.Add(showitem);
            notifyIcon.ContextMenu = menu;
        }

        void showitem_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
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

        #region 最小化、关闭
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            YesNoMessageBox messageBox = new YesNoMessageBox("是否要关闭系统？");
            messageBox.ShowDialog();
            if (messageBox.YesOrNo == YesNoMessageBox.YesAndNo.Yes)
            {
                this.WindowClose();
            }
        }

        private void btnMinSize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.actionViewState == ActionViewState.Busy)
                this.WindowState = WindowState.Minimized;
        }
    }
}
