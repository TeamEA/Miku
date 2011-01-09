using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Miku.Core.Communication;
using Miku.Core.Communication.Helpers;
using System.Net;
using System.Diagnostics;
using Miku.Server.Entities;

namespace Miku.Server.Communication
{
    public class CommunicationServer
    {
        Dictionary<string,Client> clientList = ClientList.GetUserList();

        public delegate void OnRegistring(Client clientInfo);
        public event OnRegistring OnRegistringEvent;

        private TcpSocket tcpSocket;
        private const int defaultSingleDataBlockSize = 1024;

        public IPEndPoint target { get; set; }

        public CommunicationServer(int listeningPort)
        {
            tcpSocket = new TcpSocket();
            tcpSocket.ListeningPort = listeningPort;
            tcpSocket.ReceiveBufferSize = Int32.MaxValue;
            tcpSocket.SendBufferSize = Int32.MaxValue;
            tcpSocket.DataArrival += new TcpSocket.DataArrivalEventHandler(tcpSocket_DataArrival);
            tcpSocket.Active = true;
        }

        void tcpSocket_DataArrival(byte[] Data, System.Net.IPAddress Ip, int Port)
        {
       
            DataPackage dataPackage =
                (DataPackage)SerializeHelper.DeSerializeFromBinary(new MemoryStream(Data));
            switch (dataPackage.DataTypes)
            {
                case DataTypes.SendNone:
                    break;
                case DataTypes.SendCommad:
                    if (dataPackage.CommunicatinCmds == CommunicationCmds.RequestSendFile ||
                        dataPackage.CommunicatinCmds == CommunicationCmds.RequestFile)
                    {
                        if (clientList.Keys.Contains(dataPackage.TargetUserName))
                        {
                            tcpSocket.Send(clientList[dataPackage.TargetUserName].ClientIPEndPoint, Data);
                        }
                    }
                    else
                    {
                        RecieveCommand(dataPackage, Ip, Port);
                    }
                    break;
                case DataTypes.SendMsg:
                case DataTypes.SendFile:
                case DataTypes.SendControlImage:
                case DataTypes.SendControlCommand:
                   // case DataTypes.senm
                    if (clientList.Keys.Contains(dataPackage.TargetUserName))
                    {
                        tcpSocket.Send(clientList[dataPackage.TargetUserName].ClientIPEndPoint, Data);
                    }
                    break;
                default:
                    throw new Exception("");
                    break;
            }
        }

        private void RecieveCommand(DataPackage dataPackage, System.Net.IPAddress Ip, int Port)
        {
            switch (dataPackage.CommunicatinCmds)
            {
                case CommunicationCmds.SendToOne:
                    break;
                case CommunicationCmds.Registering:
                    RegisterData regData = SerializeHelper.DeSerializeFromBinary(new MemoryStream(dataPackage.Datas)) as RegisterData;
                    Client clientInfo;
                    if (clientList.Keys.Contains(regData.UserName))
                    {
                        clientInfo = clientList[regData.UserName];
                        clientInfo.ClientIPEndPoint = regData.ClientIPEndPoint;
                    }
                    else
                    {
                         clientInfo = new Client()
                            {
                                UserName = regData.UserName,
                                ClientIPEndPoint = regData.ClientIPEndPoint,
                                PhoneUri = null
                            };
                        clientList.Add(regData.UserName,
                            clientInfo);
                    }
                    OnRegistringEvent(clientInfo);
                    break;
                default:
                    break;
            }
        }
    }
}
