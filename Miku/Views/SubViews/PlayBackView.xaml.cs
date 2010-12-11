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
using Miku.Client.Views.UserControl;
using Miku.Client.Helpers;
using Miku.Client.Controllers;

namespace Miku.Client.Views.SubViews
{
    /// <summary>
    /// Interaction logic for PlayView.xaml
    /// </summary>
    public partial class PlayBackView : Window,IActionView
    {
        private IActionController actionController;
        public PlayBackView()
        {
            InitializeComponent();
            actionController = new ActionController(this);            
        }
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

        #region
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            YesNoMessageBox messageBox = new YesNoMessageBox("是否这的要关闭系统？");            
            messageBox.ShowDialog();
            if (messageBox.YesOrNo == YesNoMessageBox.YesAndNo.Yes)
            {
                if(this.Owner!=null)
                    this.Owner.Close();
                this.Close();
            }
        }

        private void btnMaxSize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMinSize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != System.Windows.WindowState.Minimized)
                WindowState = System.Windows.WindowState.Minimized;
            else
                WindowState = System.Windows.WindowState.Normal;
        }
        #endregion

        #region
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnManual_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainView = (MainWindow)this.Owner;
            mainView.Show();
            this.Close();
        }

        #region
        private void btnprevious_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnplay_Click(object sender, RoutedEventArgs e)
        {

        }
        public void ResponseStartRecording()
        {
            throw new NotImplementedException();
        }
        private void btnstop_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        public void ResponseStopRecording()
        {
            throw new NotImplementedException();
        }

        public void ResponseStartPlayback()
        {
            try
            {
                actionController.RequestStartPlayback();                
            }
            catch (Exception e)
            {
                YesNoMessageBox messageBox = new YesNoMessageBox(e.Message);
                messageBox.ShowDialog();
            }
        }

        public void ResponseKeepRecording(string recordInfo)
        {
            throw new NotImplementedException();
        }

        private void btnnext_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        #region GetDesktopHandle
        public IntPtr deskHandle = IntPtr.Zero;
        /// <summary>
        /// Get the handle to the desktop.
        /// </summary>
        /// <returns>Handle to the desktop.</returns>
        public IntPtr GetDesktopHandle()
        {
            try
            {
                IntPtr Handle = IntPtr.Zero;

                //It is applicable to Windows xp and some kinds of windows vista and windows 7
                Handle = Win32API.FindWindow("Progman", "Program Manager");
                Handle = Win32API.FindWindowEx(Handle, IntPtr.Zero, "SHELLDLL_DefView", null);
                Handle = Win32API.FindWindowEx(Handle, IntPtr.Zero, "SysListView32", "FolderView");

                //It is applicable to  some kinds of windows vista and windows 7
                if (Handle == IntPtr.Zero)
                {
                    bool notFind = true;
                    Win32API.CallBack myCallback = new Win32API.CallBack(FindDesktop_Top);
                    Win32API.EnumWindows(myCallback, ref notFind);
                    if (deskHandle != IntPtr.Zero)
                    {
                        Handle = deskHandle;
                    }
                }

                return Handle;
            }
            catch (Exception ex)
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        ///  Enumerates all top-level windows on the screen to find the windows whose class name
        ///  is WorkerW,and then find folderview in it.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public bool FindDesktop_Top(IntPtr hwnd, ref bool lParam)
        {
            StringBuilder sb = new StringBuilder(512);
            Win32API.GetClassName(hwnd, sb, sb.Capacity);
            if (sb.ToString() == "WorkerW")
            {
                // find folderview
                Win32API.EnumChildWindows(hwnd, FindDesktopRecur, ref lParam);
            }
            return true;
        }

        public bool FindDesktopRecur(IntPtr hwnd, ref bool lParam)
        {
            if (lParam == false)
            {
                return false;
            }
            StringBuilder sb = new StringBuilder(512);
            Win32API.GetWindowText(hwnd, sb, sb.Capacity);

            //if it is founded ,returns.
            if (sb.ToString() == "FolderView")
            {
                deskHandle = hwnd;
                lParam = false;
            }
            Win32API.EnumChildWindows(hwnd, FindDesktopRecur, ref lParam);
            return lParam;
        }
        #endregion
        
    }
}
