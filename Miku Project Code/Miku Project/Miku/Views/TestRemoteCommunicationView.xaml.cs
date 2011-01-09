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
using System.Windows.Forms;
using System.IO;
using System.Windows.Interop;
using Miku.Client.Models.Hooks;
using System.Runtime.InteropServices;
using Miku.Client.Models.Simulators;

namespace Miku.Client.Views
{
    /// <summary>
    /// Interaction logic for TestRemoteCommunicationView.xaml
    /// </summary>
    public partial class TestRemoteCommunicationView : Window, ICommunicationView
    {

        private CommunicationController communicationController;
        private SynchronizationContext synchronizationContext;
        private KBMSimulator simulator;
        public TestRemoteCommunicationView()
        {
            InitializeComponent();
        }


        public void ResponseRecieveMsg(string msg)
        {
            SynchronizationContext.SetSynchronizationContext((SynchronizationContext)synchronizationContext);
            SynchronizationContext.Current.Post(delegate
                 {
                     txtMsgRecieve.Text += msg;
                 }, null);
        }

        public string SendingMsg
        {
            get { return txtMsgSend.Text; }
        }




        public void ResponseRecieveFile(out string filePath, string fileName)
        {
            if (String.IsNullOrEmpty(this.FilePath))
            {
                Thread th = new Thread(ShowSaveAsDialog);
                th.Start(Thread.CurrentThread);
                //Thread.CurrentThread.Suspend(); 
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException ex)
                {
                }
            }

            if (!String.IsNullOrEmpty(FilePath))
            {
                filePath = this.FilePath;
            }
            else
            {
                filePath = String.Empty;
            }

        }

        private void ShowSaveAsDialog(object thd)
        {
            SynchronizationContext.SetSynchronizationContext((SynchronizationContext)synchronizationContext);
            SynchronizationContext.Current.Post(delegate
            {

                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FilePath = dialog.FileName;
                    //((Thread)thd).Resume();
                    ((Thread)thd).Interrupt();
                }

            }, null);
        }

        public void ResponseEndRecieveFile()
        {
            this.FilePath = null;
        }

        string filePath;
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }


        public void ResponseSendFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = dialog.FileName;
            }
            else
            {
                FilePath = string.Empty;
            }
        }


        public System.Net.IPEndPoint ipEndPoint
        {
            get
            {
                return new System.Net.IPEndPoint(System.Net.IPAddress.Parse(txtIpAddress.Text), 11002);
            }
        }

        public int ListeningPort
        {
            get { return Convert.ToInt32(txtListeningPort.Text); }
        }


        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            communicationController.RequestRegisterToServer();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            communicationController.RequestSendMsg();
        }


        public string SenderName
        {
            get { return txtSender.Text; }
        }

        public string RecieverName
        {
            get { return txtReciever.Text; }
        }

        private void txtSendFile_Click(object sender, RoutedEventArgs e)
        {
            this.communicationController.RequestSendFile();
        }


        public void OnResponseRecievedControlImage(MemoryStream screenImageStream)
        {

            Dispatcher.BeginInvoke((Action)(() =>
            {
                ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
                imagebg.Source = (ImageSource)imageSourceConverter.ConvertFrom(screenImageStream);

            }));
        }

        private void btnSendImage_Click(object sender, RoutedEventArgs e)
        {
            this.communicationController.RequestAcceptRemoteControl();
        }


        private IntPtr MessageHookHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
           
            return IntPtr.Zero;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            communicationController = new CommunicationController(this);
            synchronizationContext = SynchronizationContext.Current;
            simulator = new KBMSimulator();

            imagebg.Width = Screen.PrimaryScreen.Bounds.Width;
            imagebg.Height = Screen.PrimaryScreen.Bounds.Right;

            imagebg.MouseMove +=new System.Windows.Input.MouseEventHandler(imagebg_MouseMove);
            imagebg.MouseLeftButtonDown += new MouseButtonEventHandler(imagebg_MouseLeftButtonDown);
            imagebg.MouseLeftButtonUp += new MouseButtonEventHandler(imagebg_MouseLeftButtonUp);
            imagebg.MouseRightButtonDown += new MouseButtonEventHandler(imagebg_MouseRightButtonDown);
            imagebg.MouseRightButtonUp += new MouseButtonEventHandler(imagebg_MouseRightButtonUp);
            imagebg.MouseWheel += new MouseWheelEventHandler(imagebg_MouseWheel);

            imagebg.KeyDown += new System.Windows.Input.KeyEventHandler(imagebg_KeyDown);
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd != IntPtr.Zero)
            {
               // HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(this.MessageHookHandler));
            }
        }

        void imagebg_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        void imagebg_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Win32API.MouseEvent mymouse = new Win32API.MouseEvent();
            mymouse.dwFlags = Miku.Client.Win32API.MouseEventFlag.Wheel;

            mymouse = SendingAction(e, mymouse);
        }

        void imagebg_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Win32API.MouseEvent mymouse = new Win32API.MouseEvent();
            mymouse.dwFlags = Miku.Client.Win32API.MouseEventFlag.RightUp;

            mymouse = SendingAction(e, mymouse);
        }

        void imagebg_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Win32API.MouseEvent mymouse = new Win32API.MouseEvent();
            mymouse.dwFlags = Miku.Client.Win32API.MouseEventFlag.RightDown;

            mymouse = SendingAction(e, mymouse);
        }

        void imagebg_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Win32API.MouseEvent mymouse = new Win32API.MouseEvent();
            mymouse.dwFlags = Miku.Client.Win32API.MouseEventFlag.LeftUp;

            mymouse = SendingAction(e, mymouse);
        }

        void imagebg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Win32API.MouseEvent mymouse = new Win32API.MouseEvent();
            mymouse.dwFlags = Miku.Client.Win32API.MouseEventFlag.LeftDown;

            mymouse = SendingAction(e, mymouse);
        }


        void imagebg_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //e.KeyboardDevice.
        }

        void imagebg_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

            Win32API.MouseEvent mymouse = new Win32API.MouseEvent();
            mymouse.dwFlags =Miku.Client.Win32API.MouseEventFlag.Move;

            mymouse = SendingAction(e, mymouse);
        }

        private Win32API.MouseEvent SendingAction(System.Windows.Input.MouseEventArgs e, Win32API.MouseEvent mymouse)
        {
            Point position = e.GetPosition(imagebg);
            mymouse.pt = new Win32API.POINT((int)position.X, (int)position.Y);

            OnResponseSendingActionEventHandler handler = OnResponseSendingActionEvent;
            if (handler != null)
            {
                handler(mymouse, this.RecieverName);
            }

            e.Handled = true;
            return mymouse;
        }


        public event OnResponseSendingActionEventHandler OnResponseSendingActionEvent;


        public void OnResponseRecievedControlCommand(object command)
        {
            if (command is Win32API.MouseEvent)
            {
                Win32API.MouseEvent mouseEvent = ((Win32API.MouseEvent)command);
                simulator.Simulate(mouseEvent.dwFlags, mouseEvent.pt.x, mouseEvent.pt.y, 0, UIntPtr.Zero);
                
            }
            else if (command is Win32API.KeyEvent)
            {
                Win32API.KeyEvent keyEvent = ((Win32API.KeyEvent)command);
                simulator.Simulate(keyEvent.bVk, keyEvent.vScan, keyEvent.dwFlags, 0);
            }
        }

        private void btnControl_Click(object sender, RoutedEventArgs e)
        {
            this.communicationController.RequestRemoteControl();
        }
    }
}
