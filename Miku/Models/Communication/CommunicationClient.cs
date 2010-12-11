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

        private TcpSocket tcpSocket;
        private const int defaultSingleDataBlockSize = 1024;

        public CommunicationClient()
        {
            tcpSocket = new TcpSocket();
            tcpSocket.DataArrival += new TcpSocket.DataArrivalEventHandler(tcpSocket_DataArrival);
            tcpSocket.Active = true;
        }

        void tcpSocket_DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port)
        {
            //ShowMsg(DeSerializeBinary(new MemoryStream(Data)).ToString());
            //FileStream fs =  File.Create("arrivedData.txt", 1000);
            //fs.Write(Data, 0, Data.Length);
            //fs.Close();
            //ShowMsg("Recieve!:" + Data.Length);

            DataPackage dataPackage =
                (DataPackage)SerializeHelper.DeSerializeFromBinary(new MemoryStream(Data));
            switch (dataPackage.DataTypes)
            {
                case DataTypes.SendNone:
                    break;
                case DataTypes.SendCommad:
                    break;
                case DataTypes.SendMsg:
                    RecieveMsg(dataPackage);
                    break;
                case DataTypes.SendFile:
                    break;
                default:
                    break;
            }

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
                    if (!File.Exists(dataPackage.FileName))
                    {
                        FileStream fs = new FileStream(dataPackage.FileName, FileMode.Create, FileAccess.Write);
                        //string msgSending = (string)SerializeHelper.DeSerializeFromBinary(new MemoryStream(dataPackage.Datas));
                        //byte[] datas = Encoding.Unicode.GetBytes(msgSending);
                        //fs.Write(datas, 0, datas.Length);
                        fs.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                        fs.Close();
                        File.SetAttributes(dataPackage.FileName, FileAttributes.Hidden | FileAttributes.Temporary);
                    }
                    else
                    {
                        FileStream fs = new FileStream(dataPackage.FileName, FileMode.Append, FileAccess.Write);
                        //string msgSending = (string)SerializeHelper.DeSerializeFromBinary(new MemoryStream(dataPackage.Datas));
                        //byte[] datas = Encoding.Unicode.GetBytes(msgSending);
                        //fs.Write(datas, 0, datas.Length);
                        fs.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                        fs.Close();
                    }
                    break;
                case SendingStates.End:
                    FileStream fsEnd = new FileStream(dataPackage.FileName, FileMode.Append, FileAccess.Write);
                    //string msgEnd = (string)SerializeHelper.DeSerializeFromBinary(new MemoryStream(dataPackage.Datas));
                    //byte[] datasEnd = Encoding.Unicode.GetBytes(msgEnd);
                    //fsEnd.Write(datasEnd, 0, datasEnd.Length);
                    fsEnd.Write(dataPackage.Datas, 0, dataPackage.Datas.Length);
                    fsEnd.Close();
                    //string msgBig = (string)SerializeHelper.DeSerializeFromBinary(new MemoryStream(File.ReadAllBytes(dataPackage.FileName)));
                    string msgBig = Encoding.Unicode.GetString(File.ReadAllBytes(dataPackage.FileName));
                    RecieveMsgEvent(msgBig);
                    File.Delete(dataPackage.FileName);
                    break;
                default:
                    break;
            }
        }

        private void ShowMsg(string msg)
        {
            //try
            //{
            //    listBox1.Items.Add(msg + ":" + DateTime.Now + ":" + i.ToString());
            //    //MessageBox.Show(msg + ":" + DateTime.Now);
            //    i++;
            //}
            //catch (Exception)
            //{


            //}

        }

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
                    FileName = String.Empty
                };

                tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
            }
            else
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
                            FileName = tmpfileID.ToString()
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
                            FileName = tmpfileID.ToString()
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
                        FileName = tmpfileID.ToString()
                    };

                    tcpSocket.Send(ipEndPoint, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
                }
            }


        }

        public void SendFileToOneThroughIPEndPoint(string filePath, System.Net.IPAddress Ip, int Port, int blockSize = defaultSingleDataBlockSize)
        {
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
                    // FileName = String.Empty
                };

                tcpSocket.Send(Ip, Port, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
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
                            // FileName = String.Empty
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
                           // FileName = String.Empty
                       };
                    }

                    tcpSocket.Send(Ip, Port, SerializeHelper.SerializeToBinary(dataPackage).ToArray());

                    i++;
                    Thread.Sleep(10);
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
                        // FileName = String.Empty
                    };

                    tcpSocket.Send(Ip, Port, SerializeHelper.SerializeToBinary(dataPackage).ToArray());
                }
            }
        }
        public void SendFileToOneThroughIPEndPoint(string filePath, IPEndPoint ipEndPoint, int blockSize = 1024)
        {
            throw new NotImplementedException();
        }
    }
}
