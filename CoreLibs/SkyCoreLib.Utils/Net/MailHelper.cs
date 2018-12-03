using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace SkyMallCore.Core
{
    public class MailHelper
    {

        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        private string mailServer { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        private string mailUserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        private string mailPassword { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        private string displayName { get; set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        private int serverPort { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailServer">邮件服务器地址</param>
        /// <param name="sender">发件人邮箱</param>
        /// <param name="pass">发件人密码</param>
        /// <param name="displayName">发件人显示名</param>
        /// <param name="port">端口</param>
        public MailHelper(string mailServer,string sender,string pass,string displayName, int port = 25)
        {
            this.mailServer = mailServer;
            this.serverPort = port;
            this.mailUserName = sender;
            this.mailPassword = pass;
            this.displayName = displayName;
        }


        /// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <param name="to">收件人邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="encoding">编码</param>
        /// <param name="isBodyHtml">是否Html</param>
        /// <param name="enableSsl">是否SSL加密连接</param>
        /// <returns>是否成功</returns>
        public bool Send(string to, string subject, string body, string encoding = "UTF-8", bool isBodyHtml = true, bool enableSsl = false)
        {
            try
            {
                MailMessage message = new MailMessage();
                // 接收人邮箱地址
                message.To.Add(new MailAddress(to));
                message.From = new MailAddress(mailUserName, displayName);
                message.BodyEncoding = Encoding.GetEncoding(encoding);
                message.Body = body;
                //GB2312
                message.SubjectEncoding = Encoding.GetEncoding(encoding);
                message.Subject = subject;
                message.IsBodyHtml = isBodyHtml;

                SmtpClient smtpclient = new SmtpClient(mailServer, serverPort);
                smtpclient.Credentials = new System.Net.NetworkCredential(mailUserName, mailPassword);
                //SSL连接
                smtpclient.EnableSsl = enableSsl;
                smtpclient.Send(message);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 异步发送邮件 独立线程
        /// </summary>
        /// <param name="to">邮件接收人</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public void SendByThread(string to, string title, string body, int port = 25)
        {
            new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    //邮箱的smtp地址
                    smtp.Host = mailServer;
                    //端口号
                    smtp.Port = port;
                    //构建发件人的身份凭据类
                    smtp.Credentials = new NetworkCredential(mailUserName, mailPassword);
                    //构建消息类
                    MailMessage objMailMessage = new MailMessage();
                    //设置优先级
                    objMailMessage.Priority = MailPriority.High;
                    //消息发送人
                    objMailMessage.From = new MailAddress(mailUserName, displayName, System.Text.Encoding.UTF8);
                    //收件人
                    objMailMessage.To.Add(to);
                    //标题
                    objMailMessage.Subject = title.Trim();
                    //标题字符编码
                    objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    //正文
                    objMailMessage.Body = body.Trim();
                    objMailMessage.IsBodyHtml = true;
                    //内容字符编码
                    objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    //发送
                    smtp.Send(objMailMessage);
                }
                catch (Exception)
                {
                    throw;
                }

            })).Start();
        }
    }
}
