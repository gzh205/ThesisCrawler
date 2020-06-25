using DocCrawler.Core;
using DocCrawler.DataBaseInsert;
using DocCrawler.DocSearch.WanFang;
using DocCrawler.IpPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocCrawler.DocSearch
{
    /// <summary>
    /// 爬虫的调用算法
    /// </summary>
    public class AddInCrawlers
    {
        public static string KeyWordList;
        public static string mailAddress => "1942592358@qq.com";
        public static void run()
        {
            //新建连接池，并在其中添加连接
            ConnectionPool.getInstance().addConnections(10);
            //启动sql写入
            DataWriter.GetWriter().run();
            //记录Pages
            List<Page> pages = new List<Page>();
            pages.Add(new DocSearch.WanFang.GetPageNum("爬虫"));
            //循环将url插入Crawler的队列中
            Crawler crawler = Crawler.GetCrawler();
            crawler.setThreadNum(10);
            crawler.run(pages.ToArray());
            //插入完毕后等待爬虫结束
            Crawler.ObservProc();
            DataWriter.ObservProc();
            //发送邮件提示爬取结束
            new QQEmailSend().send(mailAddress, "爬虫处理完毕");
        }
    }
}
