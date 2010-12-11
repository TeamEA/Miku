using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Miku.Client.Helpers
{
    /// <summary>
    /// 通过飞信发送短信
    /// </summary>
    public class Fetion
    {
        #region web api
        /// <summary>
        /// 通过http://sms.api.bz/fetion.php发送飞信
        /// </summary>
        /// <param name="from">发送者</param>
        /// <param name="password">飞信密码</param>
        /// <param name="to">接受者</param>
        /// <param name="message">发送的消息</param>
        public void SendFetion(string from, string password, string to, string message)
        {
            string postData = "username=" + from + "&";
            postData += ("password=" + password + "&");
            postData += ("sendto=" + to + "&");
            postData += ("message=" + message);
            HttpWebRequest myRequest =
            (HttpWebRequest)WebRequest.Create("http://sms.api.bz/fetion.php?" + postData);
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
        }

        /// <summary>
        /// 通过http://sms.api.bz/fetion.php发送飞信
        /// </summary>
        /// <param name="from">发送者</param>
        /// <param name="password">飞信密码</param>
        /// <param name="to">接受者</param>
        /// <param name="message">发送的消息</param>
        public void SendFetion2(string from, string password, string to, string message)
        {
            string postData = "username=" + from + "&";
            postData += ("password=" + password + "&");
            postData += ("sendto=" + to + "&");
            postData += ("message=" + message);

            HttpWebRequest myRequest =
           (HttpWebRequest)WebRequest.Create("http://sms.api.bz/fetion.php");

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();

            //// Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            // Get response
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
        }

        #endregion

    }
}
