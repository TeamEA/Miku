using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Miku.Core.Communication
{
    public partial class TcpSocket : Component
    {
        private IPEndPoint ServerEndPoint = null;   //定义网络端点
        private TcpClient TcpServer = new TcpClient(); //创建网络服务,也就是UDP的Socket
        private TcpListener tcpListener;
        private System.Threading.Thread thdTcp; //创建一个线程

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

        private int listeningPort = 11002;

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
                return TcpServer.Client.ReceiveBufferSize;
            }
            set
            {
                TcpServer.Client.ReceiveBufferSize = value;
            }
        }

        //发送缓冲区大小
        [Browsable(true), Category("BufferSize"), Description("发送缓冲区大小")]
        public int SendBufferSize
        {
            get
            {
                return TcpServer.Client.SendBufferSize;
            }
            set
            {
                TcpServer.Client.SendBufferSize = value;
            }
        }

        public TcpSocket()
        {
            InitializeComponent();

            //TcpServer.Client.ReceiveBufferSize = int.MaxValue;   //接收缓冲区大小
            //TcpServer.Client.SendBufferSize = int.MaxValue;  //发送缓冲区大小
        }

        public TcpSocket(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            //TcpServer.Client.ReceiveBufferSize = int.MaxValue;   //接收缓冲区大小
            //TcpServer.Client.SendBufferSize = int.MaxValue;  //发送缓冲区大小
        }

        protected void Listener()   //监听
        {
            //将IP地址和端口号以网络端点存储
            ServerEndPoint = new IPEndPoint(IPAddress.Any, listeningPort);
            //if (tcpListener != null)
            //    tcpListener.Stop();
            tcpListener = new TcpListener(listeningPort);  //创建一个新的端口号
            tcpListener.Start();
            //UDP_Server.Client.SendTimeout = 30000;
            //UDP_Server.Client.ReceiveTimeout = 3000;

            try
            {
                thdTcp = new Thread(new ThreadStart(GetTCPData));   //创建一个线程
                thdTcp.IsBackground = true;
                thdTcp.Start(); //执行当前线程
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());  //显示线程的错误信息
            }
        }

        private void GetTCPData()   //获取当前接收的消息
        {
            while (active)
            {
                //try
                //{
                    //将获取的远程消息转换成二进制流
                    TcpClient client = tcpListener.AcceptTcpClient();
                    NetworkStream ns = client.GetStream();
                    if (ns.CanRead)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        if (DataArrival != null)    //如果当前正在托管
                        {

                            byte[] bytes = new byte[1024];
                            int i;
                            while ((i = ns.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                memoryStream.Write(bytes, 0, i);
                            }
                            
                            //利用当前控件的DataArrival事件将消息发给远程计算机
                            DataArrival(memoryStream.ToArray(), ServerEndPoint.Address, ServerEndPoint.Port);
                        }



                    }
                    Thread.Sleep(25);
                //}
                //catch (SocketException e)
                //{
                //    //Active = false;
                //    MessageBox.Show(e.ErrorCode.ToString());
                //    //Active = true;
                //}
                //catch (Exception e)
                //{
                //    //Active = false;
                //    MessageBox.Show(e.ToString());
                //    //Active = true;
                //}
            }
        }

        private void CallBackMethod(IAsyncResult ar)
        {
            //从异步状态ar.AsyncState中，获取委托对象
            DataArrivalEventHandler dn = (DataArrivalEventHandler)ar.AsyncState;
            //一定要EndInvoke
            dn.EndInvoke(ar);
        }

        public void Send(IPEndPoint ipEndPoint, byte[] Data)
        {
            //try
            //{
                //将消息发给远程计算机
                TcpServer = new TcpClient();
                TcpServer.Connect(ipEndPoint);
                if (TcpServer.Connected)
                {
                    NetworkStream ns = TcpServer.GetStream();
                    ns.Write(Data, 0, Data.Length);
                    ns.Close();
                    TcpServer.Close();
                }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}
        }

        public void Send(System.Net.IPAddress Host, int Port, byte[] Data)
        {
            //try
            //{
                //将IP地址和端口号实例化一个IPEndPoint对象
                IPEndPoint ipEndPoint = new IPEndPoint(Host, Port);

                //将消息发给远程计算机
                TcpServer = new TcpClient();
                TcpServer.Connect(ipEndPoint);
                if (TcpServer.Connected)
                {
                    NetworkStream ns = TcpServer.GetStream();
                    ns.Write(Data, 0, Data.Length);
                    ns.Close();
                    TcpServer.Close();
                }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}
        }

        private void OpenSocket()    //打开socket
        {
            Listener();         //通过该方法对UDP协议进行监听
        }

        private void CloseSocket()    //关闭socket
        {
            if (TcpServer != null)     //如果socket不为空
                TcpServer.Close();     //关闭socket
            if (thdTcp != null)         //如果自定义线程被打开
            {
                Thread.Sleep(30);       //睡眠主线程
                thdTcp.Abort();         //关闭子线程
            }
        }
    }

}
