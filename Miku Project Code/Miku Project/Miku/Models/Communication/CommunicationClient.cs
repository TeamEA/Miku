using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using Miku.Core.Communication;
using Miku.Core.Communication.Helpers;
using System.Net;

namespace Miku.Client.Models.Communication
{
    public class CommunicationClient
    {
        public delegate void RecieveMsgDel(string msg);
        public event RecieveMsgDel RecieveMsgEvent;

        public delegate void RecieveFileDel(out string filePath, string fileName);
        public event RecieveFileDel RecieveFileEvent;

        public delegate void EndRecieveFileEventDel();
        public event EndRecieveFileEventDel EndRecieveFileEvent;

        public delegate void OnRecievedControlImage(MemoryStream screenImageStream);
        public event OnRecievedControlImage OnRecievedControlImageEvent;

        public delegate void OnRecievedControlCommand(object command);
        public event OnRecievedControlCommand OnRecievedControlCommandEvent;

        private TcpSocket tcpSocket;
        private const int defaultSingleDataBlockSize = 1000 * 1024;

        private string recieveFilePath;

        public IPEndPoint target { get; set; }

        private Thread backgroundImageSendingThread = null;

        public CommunicationClient(IPEndPoint target, int listeningPort)
        {
            tcpSocket = new TcpSocket();
            tcpSocket.ListeningPort = listeningPort;
            tcpSocket.ReceiveBufferSize = Int32.MaxValue;
            tcpSocket.SendBufferSize = Int32.MaxValue;
            tcpSocket.DataArrival += new TcpSocket.DataArrivalEventHandler(tcpSocket_DataArrival);
            tcpSocket.Active = true;
            this.target = target;
        }

        #region Recieve
        void tcpSocket_DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port)
        {
            DataPackage dataPackage =
                (DataPackage)SerializeHelper.DeSerializeFromBinary(new MemoryStream(Data));
            switch (dataPackage.DataTypes)
            {
                case DataTypes.SendNone:
                    break;
                case DataTypes.SendCommad:
                    RecieveCommand(dataPackage, Ip, Port);
                    break;
                case DataTypes.SendMsg:
                    RecieveMsg(dataPackage);
                    break;
                case DataTypes.SendFile:
                    RecieveFile(dataPackage);
                    break;
                case DataTypes.SendControlImage:
                    MemoryStream memoryStream = new MemoryStream(dataPackage.Datas);
                    OnRecievedControlImageEvent(memoryStream);
                    break;
                case DataTypes.SendControlCommand:
                    OnRecievedControlCommandEvent(SerializeHelper.DeSerializeFromBinary(dataPackage.Datas));
                    break;
                default:
                    break;
            }

        }

        private void RecieveCommand(DataPackage dataPackage, System.Net.IPAddress Ip, int Port)
        {
            switch (dataPackage.CommunicatinCmds)
            {
                case CommunicationCmds.SendToOne:
                    break;
                case CommunicationCmds.RequestFile:
                    SendFileToOneThroughTargetUserName(Encoding.Unicode.GetString(dataPackage.Datas), dataPackage.FileInfo.RecieverName);
                    break;
                case CommunicationCmds.RequestSendFile:
                    RecieveFileEvent(out recieveFilePath, dataPackage.FileInfo.FileName);

                    tcpSocket.Send(target, SerializeHelper.SerializeToBinary(
                    new DataPackage
                    {
                        CommunicatinCmds = CommunicationCmds.RequestFile,
                        Datas = dataPackage.Datas,
                        DataTypes = DataTypes.SendCommad,
                        SendingStates = SendingStates.None,
                        TargetUserName = dataPackage.FileInfo.SenderName,
                        FileInfo = dataPackage.FileInfo
                    }).ToArray());
                    break;
                default:
                    break;
            }
        }

        private void RecieveFile(DataPackage dataPackage)
        {
            switch (dataPackage.SendingStates)
            {
                case SendingStates.None:
                    break;
                case SendingStates.Single:
                    RecieveSmallFile(dataPackage);
                    break;
                case SendingStates.KeepSending:
                    KeepRecievingFile(dataPackage);
                    break;
                case SendingStates.End:
                    EndRecieveFile(dataPackage);
                    break;
                default:
                    break;
            }
        }

        private void EndRecieveFile(DataPackage dataPackage)
        {
            FileStream fsEnd = new FileStream(recieveFilePath, FileMode.Append, FileAccess.Write);
            fsEnd.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
            fsEnd.Close();
            EndRecieveFileEvent();
        }

        private void KeepRecievingFile(DataPackage dataPackage)
        {
            //recieveFilePath = dataPackage.FileName;
            if (!String.IsNullOrEmpty(recieveFilePath))
            {
                KeepWritingDatasToFile(dataPackage);
            }
        }

        private void KeepWritingDatasToFile(DataPackage dataPackage)
        {
            if (!File.Exists(recieveFilePath))
            {
                FileStream fs = new FileStream(recieveFilePath, FileMode.Create, FileAccess.Write);
                fs.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(recieveFilePath, FileMode.Append, FileAccess.Write);
                fs.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                fs.Close();
            }
        }

        private void RecieveSmallFile(DataPackage dataPackage)
        {
            string filePath;
            RecieveFileEvent(out filePath, dataPackage.FileInfo.FileName);

            if (!String.IsNullOrEmpty(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fs.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                fs.Close();
            }

            EndRecieveFileEvent();
        }

        private void RecieveMsg(DataPackage dataPackage)
        {
            switch (dataPackage.SendingStates)
            {
                case SendingStates.None:
                    break;
                case SendingStates.Single:
                    string msg = Encoding.Unicode.GetString(dataPackage.Datas);
                    //string msg = (string)SerializeHelper.DeSerializeFromBinary(new MemoryStream(dataPackage.Datas));
                    RecieveMsgEvent(msg);
                    break;
                case SendingStates.KeepSending:
                    KeepRecievingMsg(dataPackage);
                    break;
                case SendingStates.End:
                    FileStream fsEnd = new FileStream(dataPackage.FileInfo.FileName, FileMode.Append, FileAccess.Write);
                    fsEnd.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                    fsEnd.Close();
                    string msgBig = Encoding.Unicode.GetString(File.ReadAllBytes(dataPackage.FileInfo.FileName));
                    RecieveMsgEvent(msgBig);
                    File.Delete(dataPackage.FileInfo.FileName);
                    break;
                default:
                    break;
            }
        }

        private void KeepRecievingMsg(DataPackage dataPackage)
        {
            if (!File.Exists(dataPackage.FileInfo.FileName))
            {
                FileStream fs = new FileStream(dataPackage.FileInfo.FileName, FileMode.Create, FileAccess.Write);
                fs.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                fs.Close();
                File.SetAttributes(dataPackage.FileInfo.FileName, FileAttributes.Hidden | FileAttributes.Temporary);
            }
            else
            {
                FileStream fs = new FileStream(dataPackage.FileInfo.FileName, FileMode.Append, FileAccess.Write);
                fs.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                fs.Close();
            }
        }
        #endregion

        #region Sending Msg

        public void SendMsgToOneThroughIPEndPoint(string msg, IPEndPoint ipEndPoint)
        {
            MemoryStream msgContents = new MemoryStream(Encoding.Unicode.GetBytes(msg));
            //MemoryStream msgContents = SerializeHelper.SerializeToBinary(msg);//为什么不能以序列化二进制流的形式传递数据？
            if (msgContents.Length <= defaultSingleDataBlockSize)
            {
                DataPackage dataPackage = new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.SendToOne,
                    Datas = msgContents.ToArray(),
                    DataTypes = DataTypes.SendMsg,
                    SendingStates = SendingStates.Single,
                };

                tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
            }
            else
            {
                SendBigMsgThroughIPEndPoint(ipEndPoint, msgContents);
            }


        }

        private void SendBigMsgThroughIPEndPoint(IPEndPoint ipEndPoint, MemoryStream msgContents)
        {
            Guid tmpfileID = Guid.NewGuid();
            bool isSizeJustFit;
            byte[] dataContents = new byte[defaultSingleDataBlockSize];
            long count = msgContents.Length / defaultSingleDataBlockSize;

            if (count * defaultSingleDataBlockSize < msgContents.Length)
            {
                isSizeJustFit = false;
            }
            else
            {
                isSizeJustFit = true;
            }

            int i = 0;
            while (i < count)
            {
                msgContents.Read((byte[])dataContents, 0, defaultSingleDataBlockSize);
                DataPackage dataPackage;
                if (isSizeJustFit && (i == count - 1))
                {
                    dataPackage = new DataPackage
                    {
                        CommunicatinCmds = CommunicationCmds.SendToOne,
                        Datas = (byte[])dataContents,
                        DataTypes = DataTypes.SendMsg,
                        SendingStates = SendingStates.End,
                        FileInfo = new Core.Communication.FileInfo() { FileName = tmpfileID.ToString() }
                    };
                }
                else
                {
                    dataPackage = new DataPackage
                    {
                        CommunicatinCmds = CommunicationCmds.SendToOne,
                        Datas = (byte[])dataContents,
                        DataTypes = DataTypes.SendMsg,
                        SendingStates = SendingStates.KeepSending,
                        FileInfo = new Core.Communication.FileInfo() { FileName = tmpfileID.ToString() }
                    };
                }

                tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

                i++;
                Thread.Sleep(10);
            }
            if (isSizeJustFit == false)
            {
                dataContents = new byte[msgContents.Length - count * defaultSingleDataBlockSize];
                msgContents.Read((byte[])dataContents, 0, Convert.ToInt32(msgContents.Length - count * defaultSingleDataBlockSize));

                DataPackage dataPackage = new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.SendToOne,
                    Datas = (byte[])dataContents,
                    DataTypes = DataTypes.SendMsg,
                    SendingStates = SendingStates.End,
                    FileInfo = new Core.Communication.FileInfo() { FileName = tmpfileID.ToString() }
                };

                tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
            }
        }

        public void SendMsgToOneThroughTargetUserName(string msg, string targetUserName)
        {
            MemoryStream msgContents = new MemoryStream(Encoding.Unicode.GetBytes(msg));
            //MemoryStream msgContents = SerializeHelper.SerializeToBinary(msg);//为什么不能以序列化二进制流的形式传递数据？
            if (msgContents.Length <= defaultSingleDataBlockSize)
            {
                DataPackage dataPackage = new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.SendToOne,
                    Datas = msgContents.ToArray(),
                    DataTypes = DataTypes.SendMsg,
                    SendingStates = SendingStates.Single,
                    TargetUserName = targetUserName
                };

                tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
            }
            else
            {
                SendBigMsgThroughTargetUserName(targetUserName, msgContents);
            }
        }

        private void SendBigMsgThroughTargetUserName(string targetUserName, MemoryStream msgContents)
        {
            Guid tmpfileID = Guid.NewGuid();
            bool isSizeJustFit;
            byte[] dataContents = new byte[defaultSingleDataBlockSize];
            long count = msgContents.Length / defaultSingleDataBlockSize;

            if (count * defaultSingleDataBlockSize < msgContents.Length)
            {
                isSizeJustFit = false;
            }
            else
            {
                isSizeJustFit = true;
            }

            int i = 0;
            while (i < count)
            {
                msgContents.Read((byte[])dataContents, 0, defaultSingleDataBlockSize);
                DataPackage dataPackage;
                if (isSizeJustFit && (i == count - 1))
                {
                    dataPackage = new DataPackage
                    {
                        CommunicatinCmds = CommunicationCmds.SendToOne,
                        Datas = (byte[])dataContents,
                        DataTypes = DataTypes.SendMsg,
                        SendingStates = SendingStates.End,
                        FileInfo = new Core.Communication.FileInfo() { FileName = tmpfileID.ToString() }
                    };
                }
                else
                {
                    dataPackage = new DataPackage
                    {
                        CommunicatinCmds = CommunicationCmds.SendToOne,
                        Datas = (byte[])dataContents,
                        DataTypes = DataTypes.SendMsg,
                        SendingStates = SendingStates.KeepSending,
                        FileInfo = new Core.Communication.FileInfo() { FileName = tmpfileID.ToString() },
                        TargetUserName = targetUserName
                    };
                }

                tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

                i++;
                Thread.Sleep(10);
            }
            if (isSizeJustFit == false)
            {
                dataContents = new byte[msgContents.Length - count * defaultSingleDataBlockSize];
                msgContents.Read((byte[])dataContents, 0, Convert.ToInt32(msgContents.Length - count * defaultSingleDataBlockSize));

                DataPackage dataPackage = new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.SendToOne,
                    Datas = (byte[])dataContents,
                    DataTypes = DataTypes.SendMsg,
                    SendingStates = SendingStates.End,
                    FileInfo = new Core.Communication.FileInfo() { FileName = tmpfileID.ToString() },
                    TargetUserName = targetUserName
                };

                tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
            }
        }

        #endregion

        #region Sending File
        public void SendFileToOneThroughIPEndPoint(string filePath, System.Net.IPAddress Ip, int Port, int blockSize = defaultSingleDataBlockSize)
        {
            SendFileToOneThroughIPEndPoint(filePath, new IPEndPoint(Ip, Port), blockSize);
        }

        public void RequestSendFileToOneThroughIPEndPoint(string filePath, IPEndPoint ipEndPoint)
        {
            int index = filePath.LastIndexOf('\\');
            string fileName = filePath.Substring(index + 1, filePath.Length - index - 1);

            //Send a package to start
            tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(
                new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.RequestSendFile,
                    Datas = Encoding.Unicode.GetBytes(filePath),
                    DataTypes = DataTypes.SendCommad,
                    SendingStates = SendingStates.None,
                    FileInfo = new Core.Communication.FileInfo() { FileName = fileName }
                }).ToArray());
        }

        public void RequestSendFileToOneThroughTargetUserName(string filePath, string targetUserName, string clientUserName)
        {
            int index = filePath.LastIndexOf('\\');
            string fileName = filePath.Substring(index + 1, filePath.Length - index - 1);

            //Send a package to start
            tcpSocket.Send(target, SerializeHelper.SerializeToBinary(
                new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.RequestSendFile,
                    Datas = Encoding.Unicode.GetBytes(filePath),
                    DataTypes = DataTypes.SendCommad,
                    SendingStates = SendingStates.None,
                    FileInfo = new Core.Communication.FileInfo() { FileName = fileName, SenderName = clientUserName, RecieverName = targetUserName },
                    TargetUserName = targetUserName
                }).ToArray());
        }

        public void SendFileToOneThroughIPEndPoint(string filePath, IPEndPoint ipEndPoint, int blockSize = defaultSingleDataBlockSize)
        {
            int index = filePath.LastIndexOf('\\');
            string fileName = filePath.Substring(index + 1, filePath.Length - index - 1);
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Array dataContents;

            if (fs.Length <= blockSize)
            {
                dataContents = Array.CreateInstance(typeof(byte), fs.Length);
                fs.Read((byte[])dataContents, 0, dataContents.Length);

                DataPackage dataPackage = new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.SendToOne,
                    Datas = (byte[])dataContents,
                    DataTypes = DataTypes.SendFile,
                    SendingStates = SendingStates.Single,
                    FileInfo = new Core.Communication.FileInfo() { FileName = fileName }
                };

                tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
            }
            else
            {

                bool isSizeJustFit;
                dataContents = Array.CreateInstance(typeof(byte), blockSize);
                long count = fs.Length / blockSize;
                if (count * blockSize < fs.Length)
                {
                    isSizeJustFit = false;
                }
                else
                {
                    isSizeJustFit = true;
                }
                int i = 0;
                //count = 40;
                while (i < count)
                {
                    fs.Read((byte[])dataContents, 0, blockSize);
                    DataPackage dataPackage;
                    if (isSizeJustFit && (i == count - 1))
                    {
                        dataPackage = new DataPackage
                        {
                            CommunicatinCmds = CommunicationCmds.SendToOne,
                            Datas = (byte[])dataContents,
                            DataTypes = DataTypes.SendFile,
                            SendingStates = SendingStates.End,
                            FileInfo = new Core.Communication.FileInfo() { FileName = fileName }
                        };
                    }
                    else
                    {
                        dataPackage = new DataPackage
                        {
                            CommunicatinCmds = CommunicationCmds.SendToOne,
                            Datas = (byte[])dataContents,
                            DataTypes = DataTypes.SendFile,
                            SendingStates = SendingStates.KeepSending,
                            FileInfo = new Core.Communication.FileInfo() { FileName = fileName }
                        };
                    }

                    tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

                    i++;
                    Thread.Sleep(100);
                }
                if (isSizeJustFit == false)
                {
                    dataContents = Array.CreateInstance(typeof(byte), fs.Length - count * blockSize);
                    fs.Read((byte[])dataContents, 0, Convert.ToInt32(fs.Length - count * blockSize));

                    DataPackage dataPackage = new DataPackage
                    {
                        CommunicatinCmds = CommunicationCmds.SendToOne,
                        Datas = (byte[])dataContents,
                        DataTypes = DataTypes.SendFile,
                        SendingStates = SendingStates.End,
                        FileInfo = new Core.Communication.FileInfo() { FileName = fileName }
                    };

                    tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
                }
            }
        }

        public void SendFileToOneThroughTargetUserName(string filePath, string targetUserName, int blockSize = defaultSingleDataBlockSize)
        {
            int index = filePath.LastIndexOf('\\');
            string fileName = filePath.Substring(index + 1, filePath.Length - index - 1);
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Array dataContents;

            if (fs.Length <= blockSize)
            {
                dataContents = Array.CreateInstance(typeof(byte), fs.Length);
                fs.Read((byte[])dataContents, 0, dataContents.Length);

                DataPackage dataPackage = new DataPackage
                {
                    CommunicatinCmds = CommunicationCmds.SendToOne,
                    Datas = (byte[])dataContents,
                    DataTypes = DataTypes.SendFile,
                    SendingStates = SendingStates.Single,
                    FileInfo = new Core.Communication.FileInfo() { FileName = fileName },
                    TargetUserName = targetUserName
                };

                tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
            }
            else
            {

                bool isSizeJustFit;
                dataContents = Array.CreateInstance(typeof(byte), blockSize);
                long count = fs.Length / blockSize;
                if (count * blockSize < fs.Length)
                {
                    isSizeJustFit = false;
                }
                else
                {
                    isSizeJustFit = true;
                }
                int i = 0;
                while (i < count)
                {
                    fs.Read((byte[])dataContents, 0, blockSize);
                    DataPackage dataPackage;
                    if (isSizeJustFit && (i == count - 1))
                    {
                        dataPackage = new DataPackage
                        {
                            CommunicatinCmds = CommunicationCmds.SendToOne,
                            Datas = (byte[])dataContents,
                            DataTypes = DataTypes.SendFile,
                            SendingStates = SendingStates.End,
                            FileInfo = new Core.Communication.FileInfo() { FileName = fileName },
                            TargetUserName = targetUserName
                        };
                    }
                    else
                    {
                        dataPackage = new DataPackage
                        {
                            CommunicatinCmds = CommunicationCmds.SendToOne,
                            Datas = (byte[])dataContents,
                            DataTypes = DataTypes.SendFile,
                            SendingStates = SendingStates.KeepSending,
                            FileInfo = new Core.Communication.FileInfo() { FileName = fileName },
                            TargetUserName = targetUserName
                        };
                    }

                    tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

                    i++;
                    Thread.Sleep(100);
                }
                if (isSizeJustFit == false)
                {
                    dataContents = Array.CreateInstance(typeof(byte), fs.Length - count * blockSize);
                    fs.Read((byte[])dataContents, 0, Convert.ToInt32(fs.Length - count * blockSize));

                    DataPackage dataPackage = new DataPackage
                    {
                        CommunicatinCmds = CommunicationCmds.SendToOne,
                        Datas = (byte[])dataContents,
                        DataTypes = DataTypes.SendFile,
                        SendingStates = SendingStates.End,
                        FileInfo = new Core.Communication.FileInfo() { FileName = fileName },
                        TargetUserName = targetUserName
                    };

                    tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
                }
            }
        }

        #endregion

        #region Sending Image

        public void SendImageToOneThroughTargetUserName(System.Drawing.Bitmap screenImage, string targetUserName)
        {
            MemoryStream msgContents = new MemoryStream();

            screenImage.Save(msgContents, System.Drawing.Imaging.ImageFormat.Bmp);

            DataPackage dataPackage = new DataPackage
            {
                CommunicatinCmds = CommunicationCmds.SendToOne,
                Datas = msgContents.ToArray(),
                DataTypes = DataTypes.SendControlImage,
                SendingStates = SendingStates.None,
                TargetUserName = targetUserName
            };

            tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

        }

        public void BeginSendBackgroundImageToOneThroughTargetUserName(string targetUserName)
        {
            backgroundImageSendingThread = new Thread(KeepSendingBackgroundImage);
            backgroundImageSendingThread.Start(targetUserName);
        }

        public void EndSendBackgroundImageToOneThroughTargetUserName()
        {
            if (backgroundImageSendingThread != null )
            {
                try
                {
                    backgroundImageSendingThread.Abort();
                }
                catch (ThreadAbortException ex)
                {
                   
                }
               
            }
        }

        private void KeepSendingBackgroundImage(object target)
        {
            while (true)
            {
                SendImageToOneThroughTargetUserName(
                    Miku.Client.Helpers.DestopScreenHelper.GetScreenImage(true, new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0)),
                    target as string);
                Thread.Sleep(2000);
            }
        }

        #endregion

        #region Sending Remote Control Command

        public void SendControlCommandToOneThroughTargetUserName(object command, string targetUserName)
        {
            SendObjectToOneThroughTargetUserName(command, targetUserName, DataTypes.SendControlCommand);
        }

        #endregion

        #region Sending Object

        public void SendObjectToOneThroughTargetUserName(object obj, string targetUserName,DataTypes datatype,
            SendingStates sendingstate = SendingStates.None)
        {
            MemoryStream msgContents = SerializeHelper.SerializeToBinary(obj);//为什么不能以序列化二进制流的形式传递数据？

            DataPackage dataPackage = new DataPackage
            {
                CommunicatinCmds = CommunicationCmds.SendToOne,
                Datas = msgContents.ToArray(),
                DataTypes = datatype,
                SendingStates = sendingstate,
                TargetUserName = targetUserName
            };

            tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

        }

        #endregion
        #region Regist
        public void Register(string userName, IPEndPoint clientIPEndPoint)
        {
            RegisterData regData = new RegisterData
            {
                UserName = userName,
                ClientIPEndPoint = clientIPEndPoint
            };

            DataPackage dataPackage = new DataPackage
            {
                CommunicatinCmds = CommunicationCmds.Registering,
                Datas = SerializeHelper.SerializeToBinary(regData).ToArray(),
                DataTypes = DataTypes.SendCommad,
                SendingStates = SendingStates.None,
            };

            tcpSocket.Send(target, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

        }
        #endregion
    }
}
