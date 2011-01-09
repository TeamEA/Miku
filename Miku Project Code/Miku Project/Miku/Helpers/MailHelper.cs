using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Miku.Client.Helpers
{
    public static class Mailer
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="yourMailAddress">发送者</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="toMailAddress">发送到</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="userName">邮箱用户名</param>
        /// <param name="passWord">邮箱密码</param>
        /// <param name="host">SMTP服务器</param>
        public static void SendEmail(string yourMailAddress, string displayName, string toMailAddress, string subject, string body, string userName, string passWord, string host)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(yourMailAddress, displayName);
            mailMessage.To.Add(toMailAddress);
            mailMessage.Subject = subject;//邮件标题 
            mailMessage.Body = body;  //邮件内容 
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//邮件正文的编码方式 
            mailMessage.IsBodyHtml = true; //是否是HTML邮件 
            mailMessage.Priority = MailPriority.High;//优先级 

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(userName, passWord);//邮箱名和密码 
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//电子邮件通过网络发送到Smtp服务器 
            client.Host = host;//发送邮件所使用的Smtp事务的主机名称或IP地址 
            try
            {
                client.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                
            }
        }
    }
}
