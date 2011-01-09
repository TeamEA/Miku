using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using Miku.Client.Controllers;

namespace Miku.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window, IActionView
    {
        private IActionController actionController;

        public MainView()
        {
            InitializeComponent();
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            actionController = new ActionController(this,RecordStrategies.MouseAndKeyboard);
            

            //Recorder r = new Recorder();
            //r.WriteData("KeyDown", System.Windows.Forms.Keys.A, this.listBoxActions);
            //r.WriteData("KeyUp", System.Windows.Forms.Keys.A, this.listBoxActions);
            //r.GetData();



        }

        private void btnPlayback_Click(object sender, RoutedEventArgs e)
        {
            //kbActionManager.StopRecordActions();
            //Thread.Sleep(2000);
            //kbActionManager.StartPlayback();

           // actionController.StopRecordActions();
           // Thread.Sleep(2000);
           // actionController.StartPlayback();
           // actionController.RequestStopRecordActions();
           // Thread.Sleep(2000);
            actionController.RequestStartPlayback();
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            //File.Delete("actionlist.xml");
           
        }

        #region IActionView
        public void ResponseKeepRecording(string recordInfo)
        {
            this.listBoxActions.Items.Add(recordInfo);
            this.Title = recordInfo;
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

        private void btnStartRecord_Click(object sender, RoutedEventArgs e)
        {
            actionController.RequestStartRecordActions();
        }

        private void btnStopRecord_Click(object sender, RoutedEventArgs e)
        {
            actionController.RequestStopRecordActions();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.listBoxActions.Items.Clear();
            File.Delete("actionlist.xml");
        }

        private void btnSimulate_Click(object sender, RoutedEventArgs e)
        {
            Win32API.SetCursorPos(52, 36);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 37);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 38);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 39);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 40);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 41);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 42);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 43);
            Thread.Sleep(500);
            Win32API.SetCursorPos(52, 44);
            Thread.Sleep(500);
            //Win32API.Mouse_Event(Win32API.MouseEventFlag.Absolute | Win32API.MouseEventFlag.Move,
            //    52, 36, 0, UIntPtr.Zero);

            //Win32API.Mouse_Event(Win32API.MouseEventFlag.Absolute | Win32API.MouseEventFlag.LeftDown, 
            //    52, 36, 0, UIntPtr.Zero);
            //Win32API.Mouse_Event(Win32API.MouseEventFlag.Absolute | Win32API.MouseEventFlag.LeftUp,
            //    52, 36, 0, UIntPtr.Zero);

            //Win32API.Mouse_Event(Win32API.MouseEventFlag.Absolute | Win32API.MouseEventFlag.LeftDown,
            //    52, 36, 0, UIntPtr.Zero);
            //Win32API.Mouse_Event(Win32API.MouseEventFlag.Absolute | Win32API.MouseEventFlag.LeftUp,
            //    52, 36, 0, UIntPtr.Zero);
        }

        private void btnSimulateRelative_Click(object sender, RoutedEventArgs e)
        {
            Win32API.SetCursorPos(52, 36);
            Win32API.Mouse_Event(Win32API.MouseEventFlag.LeftDown,
                0, 0, 0, UIntPtr.Zero);
            Win32API.Mouse_Event(Win32API.MouseEventFlag.LeftUp,
                0, 0, 0, UIntPtr.Zero);

            Win32API.Mouse_Event(Win32API.MouseEventFlag.LeftDown,
                0, 0, 0, UIntPtr.Zero);
            Win32API.Mouse_Event(Win32API.MouseEventFlag.LeftUp,
                0, 0, 0, UIntPtr.Zero);
        }

        public void ResponseStartRecording()
        {
            throw new NotImplementedException();
        }

        public void ResponseStopRecording()
        {
            throw new NotImplementedException();
        }

        public void ResponseStartPlayback()
        {
            throw new NotImplementedException();
        }


        public void ResponseSaveActions(string filepath)
        {
            throw new NotImplementedException();
        }


        public void ResponseSaveActions()
        {
            throw new NotImplementedException();
        }


        public void ResponseSaveActions(ref string filepath)
        {
            throw new NotImplementedException();
        }


        public void ResponsePlaybackExistFile(ref string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
