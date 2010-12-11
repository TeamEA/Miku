using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miku.Core.Communication
{
    public enum CommunicationCmds
    {
        //None,
        //Registering,
        //Registered,
        //Logining,
        //Logined,
        SendToOne,
        //SendToAll,
        //UserList,
        //UpdateState,
        //VideoOpen,
        //Videoing,
        //VideoClose,
        //Close
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
        SendFile
    }

    [Serializable]
    public class DataPackage
    {
        public SendingStates SendingStates { get; set; }
        public DataTypes DataTypes { get; set; }
        public CommunicationCmds CommunicatinCmds { get; set; }
        public byte[] Datas { get; set; }

        public string FileName { get; set; }
    }
}
