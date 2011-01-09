using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Miku.Core.Commuication
{
    public partial class UdpSocket : Component
    {
        private IPEndPoint ServerEndPoint = null;   //定义网络端点
        private UdpClient UdpServer = new UdpClient(); //创建网络服务,也就是UDP的Socket
        private System.Threading.Thread thdUdp; //创建一个线程

        //定义了一个托管方法
        public delegate void DataArrivalEventHandler(byte[] Data, IPAddress Ip, int Port);

        //通过托管理在控件中定义一个事件
        public event DataArrivalEventHandler DataArrival;
        private string localHost = "127.0.0.1";

        //在“属性”窗口中显示localHost属性
        [Browsable(true), Category("Local"), Description("本地IP地址")]
        public string LocalHost
        {
            get { return localHost; }
            set { localHost = value; }
        }

        private int listeningPort = 11000;

        //在“属性”窗口中显示localPort属性
        [Browsable(true), Category("Local"), Description("本地监听端口号")]
        public int ListeningPort
        {
            get { return listeningPort; }
            set { listeningPort = value; }
        }

        private bool active = false;

        //在“属性”窗口中显示active属性
        [Browsable(true), Category("Local"), Description("激活监听")]
        public bool Active
        {
            get { return active; }
            set //该属性读取值
            {
                active = value;
                if (active) //当值为True时
                {
                    OpenSocket();   //打开监听
                }
                else
                {
                    CloseSocket();  //关闭监听
                }
            }
        }

        //接收缓冲区大小
         [Browsable(true), Category("BufferSize"), Description("接收缓冲区大小")]
        public int ReceiveBufferSize
        {
            get 
            {
                return UdpServer.Client.ReceiveBufferSize;
            }
            set
            {
                UdpServer.Client.ReceiveBufferSize = value;
            }
        }

        //发送缓冲区大小
         [Browsable(true), Category("BufferSize"), Description("发送缓冲区大小")]
        public int SendBufferSize
        {
            get 
            {
                return UdpServer.Client.SendBufferSize;
            }
            set
            {
                UdpServer.Client.SendBufferSize = value;
            }
        }

        public UdpSocket()
        {
            InitializeComponent();

            UdpServer.Client.ReceiveBufferSize = int.MaxValue;   //接收缓冲区大小
            UdpServer.Client.SendBufferSize = int.MaxValue;  //发送缓冲区大小
        }

        public UdpSocket(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            UdpServer.Client.ReceiveBufferSize = int.MaxValue;   //接收缓冲区大小
            UdpServer.Client.SendBufferSize = int.MaxValue;  //发送缓冲区大小
        }

        protected void Listener()   //监听
        {
            //将IP地址和端口号以网络端点存储
            ServerEndPoint = new IPEndPoint(IPAddress.Any, listeningPort);
            if (UdpServer != null)
                UdpServer.Close();
            UdpServer = new UdpClient(listeningPort);  //创建一个新的端口号

            //UDP_Server.Client.SendTimeout = 30000;
            //UDP_Server.Client.ReceiveTimeout = 3000;

            try
            {
                thdUdp = new Thread(new ThreadStart(GetUDPData));   //创建一个线程
                thdUdp.IsBackground = true;
                thdUdp.Start(); //执行当前线程
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());  //显示线程的错误信息
            }
        }

        private void GetUDPData()   //获取当前接收的消息
        {
            while (active)
            {
                try
                {
                    //将获取的远程消息转换成二进制流
                    byte[] Data = UdpServer.Receive(ref ServerEndPoint);

                    if (DataArrival != null)    //如果当前正在托管
                    {
                        //利用当前控件的DataArrival事件将消息发给远程计算机
                        DataArrival(Data, ServerEndPoint.Address, ServerEndPoint.Port);
                    }

                    Thread.Sleep(25);
                }
                catch (SocketException e)
                {
                    MessageBox.Show(e.ErrorCode.ToString());
                }
            }
        }

        private void CallBackMethod(IAsyncResult ar)
        {
            //从异步状态ar.AsyncState中，获取委托对象
            DataArrivalEventHandler dn = (DataArrivalEventHandler)ar.AsyncState;
            //一定要EndInvoke
            dn.EndInvoke(ar);
        }


        public void Send(System.Net.IPAddress Host, int Port, byte[] Data)
        {
            try
            {
                //将IP地址和端口号实例化一个IPEndPoint对象
                IPEndPoint server = new IPEndPoint(Host, Port);

                //将消息发给远程计算机
                UdpServer.Send(Data, Data.Length, server);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void OpenSocket()    //打开socket
        {
            Listener();         //通过该方法对UDP协议进行监听
        }

        private void CloseSocket()    //关闭socket
        {
            if (UdpServer != null)     //如果socket不为空
                UdpServer.Close();     //关闭socket
            if (thdUdp != null)         //如果自定义线程被打开
            {
                Thread.Sleep(30);       //睡眠主线程
                thdUdp.Abort();         //关闭子线程
            }
        }
    }
}
