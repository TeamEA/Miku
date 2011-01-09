using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Miku.Client.Helpers.ExceptionHandler
{
    public static class ExceptionLogger
    {
        public static bool IsFirstWrite = true;
        /// <summary>
        /// 记录事件
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="errorMsg"></param>
        public static void LogEvent(string className,string methodName,string errorMsg)
        {
            FileStream fs = new FileStream("EventLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fs);
            sr.WriteLine(string.Format("Exception occurs in {0}.{1}",className,methodName));
            sr.WriteLine("Error Message is : "+errorMsg);
            sr.WriteLine();
            sr.Close();
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="ex">被记录的异常</param>
        public static void LogException(Exception ex)
        {
            if (IsFirstWrite)
            {
                if (File.Exists("EventLog.log"))
                {
                    FileStream fs = new FileStream("EventLog.log", FileMode.Append, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine(string.Format("Exception {0} occurs:", ex.GetType().ToString()));
                    sr.WriteLine("Error Message is : " + ex.Message);
                    sr.WriteLine();
                    sr.Close();
                }
                else
                {
                    FileStream fs = new FileStream("EventLog.log", FileMode.Create, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine(string.Format("Exception {0} occurs:", ex.GetType().ToString()));
                    sr.WriteLine("Error Message is : " + ex.Message);
                    sr.WriteLine();
                    sr.Close();
                    File.SetAttributes("EventLog.log", FileAttributes.Hidden);
                }

                IsFirstWrite = false;
            }
            else
            {
                FileStream fs = new FileStream("EventLog.log", FileMode.Append, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(string.Format("Exception {0} occurs:", ex.GetType().ToString()));
                sr.WriteLine("Error Message is : " + ex.Message);
                sr.WriteLine();
                sr.Close();
            }
        }
    }
}
