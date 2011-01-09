using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Miku.Core.Communication
{
    public enum CommunicationCmds
    {
        //None,
        Registering,
        //Registered,
        //Logining,
        //Logined,
        SendToOne,
        RequestSendFile,
        //SendToAll,
        //UserList,
        //UpdateState,
        //VideoOpen,
        //Videoing,
        //VideoClose,
        //Close
        RequestFile
    }

    public enum SendingStates
    {
        None,
        Single,
        KeepSending,
        End
    }

    public enum DataTypes
    {
        SendNone,
        SendCommad,
        SendMsg,
        SendFile,
        SendControlCommand,
        SendControlImage
    }

    [Serializable]
    public class RegisterData
    {
        public IPEndPoint ClientIPEndPoint { get; set; }
        public string UserName { get; set; }
    }

    [Serializable]
    public class FileInfo
    {
        public string FileName { get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }
    }

    [Serializable]
    public class DataPackage
    {
        public SendingStates SendingStates { get; set; }
        public DataTypes DataTypes { get; set; }
        public CommunicationCmds CommunicatinCmds { get; set; }
        public byte[] Datas { get; set; }

        public FileInfo FileInfo { get; set; }

        public string TargetUserName { get; set; }
    }
}
