using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Miku.Client.Views.UserControl
{
	public partial class YesNoMessageBox
    {
        #region ctor
        /// <summary>
        /// 无消息构造函数
        /// </summary>
        public YesNoMessageBox()
        {
            this.InitializeComponent();
            SetWindowProperty();
            // Insert code required on object creation below this point.
        }

        /// <summary>
        /// 自定义消息构造函数
        /// </summary>
        /// <param name="message"></param>
        public YesNoMessageBox(string message)
            : this()
        {
            this.messageTextBlock.Text = message;
        }
        #endregion ctor

        #region Properties
        private Window _thisParent = new Window();
        /// <summary>
        /// 父窗口
        /// </summary>
        public Window ThisParent
        {
            get { return _thisParent; }

        }
        public enum YesAndNo { Yes, No };
        public YesAndNo YesOrNo { get; private set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// 无模式地显示对话框
        /// </summary>
        public void Show()
        {
            this.ThisParent.Show();
        }

        /// <summary>
        /// 有模式地显示对话框
        /// </summary>
        public void ShowDialog()
        {
            this.ThisParent.ShowDialog();
        }

        /// <summary>
        /// 设置父窗口属性
        /// </summary>
        private void SetWindowProperty()
        {
            this.ThisParent.AllowsTransparency = true;
            this.ThisParent.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ThisParent.WindowStyle = WindowStyle.None;
            this.ThisParent.Topmost = true;
            this.ThisParent.ShowInTaskbar = false;
            this.ThisParent.Background = null;
            this.ThisParent.SizeToContent = SizeToContent.WidthAndHeight;
            this.ThisParent.Content = this;
            this.ThisParent.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(Window1_MouseLeftButtonDown);
        }

        /// <summary>
        /// 鼠标左键按下拖动弹出的窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (sender as Window).DragMove();
        }

        /// <summary>
        /// 关闭按钮处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ThisParent.Close();
        }

        /// <summary>
        /// 点击Yes处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.YesEvent != null)
            {
                YesEvent(sender, e);
            }
            YesOrNo = YesAndNo.Yes;
            this.ThisParent.Close();
        }

        /// <summary>
        /// 点击No处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NoEvent != null)
            {
                NoEvent(sender, e);
            }
            YesOrNo = YesAndNo.No;
            this.ThisParent.Close();
        }
        #endregion Methods

        #region Events
        public delegate void YesEventHandler(object sender, RoutedEventArgs e);
        /// <summary>
        /// 点击Yes事件
        /// </summary>
        public event YesEventHandler YesEvent;

        public delegate void NoEventHandler(object sender, RoutedEventArgs e);
        /// <summary>
        /// 点击No事件
        /// </summary>
        public event NoEventHandler NoEvent;
        #endregion Events
    }
}