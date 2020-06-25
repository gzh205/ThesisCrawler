using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.DocSearch
{
    /// <summary>
    /// 设置SMTP邮件服务器的基本信息
    /// </summary>
    public class QQEmailSend : MailMsg.Email
    {
        protected override string host => "smtp.qq.com";

        protected override string address => "1942592358@qq.com";

        protected override string password => "akfprtcajbqqddbc";

        protected override string title => "验证邮件";
    }
}
