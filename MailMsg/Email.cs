using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace DocCrawler.MailMsg
{
    public abstract class Email
    {
        protected abstract string host { get; }
        protected abstract string address { get; }
        protected abstract string password { get; }
        protected abstract string title { get; }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">目标地址</param>
        /// <param name="subject">邮件的标题</param>
        /// <param name="body">邮件的正文</param>
        public void send(string to,string text)
        {
            SmtpClient smtpclient = new SmtpClient();
            smtpclient.Host = host;
            smtpclient.Credentials = new NetworkCredential(address, password);
            smtpclient.Send(new MailMessage(address, to, title, text));
        }
    }
}
