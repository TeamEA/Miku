using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Miku.Core.Communication.Helpers
{

    /// <summary>
    /// 
    /// </summary>
    public static class SerializeHelper
    {
        public static MemoryStream SerializeToBinary(object request)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            serializer.Serialize(memStream, request);
            return memStream;
        }

        public static object DeSerializeFromBinary(MemoryStream memStream)
        {
            memStream.Position = 0;
            BinaryFormatter deserializer = new BinaryFormatter();
            object newObj = deserializer.Deserialize(memStream);
            return newObj;
        }

        public static object DeSerializeFromBinary(Byte[] datas)
        {
            MemoryStream memStream = new MemoryStream(datas);
            memStream.Position = 0;
            BinaryFormatter deserializer = new BinaryFormatter();
            object newObj = deserializer.Deserialize(memStream);
            return newObj;
        }
    }
}
