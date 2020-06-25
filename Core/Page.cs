using DocCrawler.IpPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.Core
{
    public abstract class Page
    {
        public Uri uri
        {
            get;
            set;
        }
        public abstract Page[] pageProcesser(HtmlAgilityPack.HtmlDocument doc);
        public void PageAnalysis()
        {
            WebClientNode node = ConnectionPool.getInstance().getOne();
            string htmlPage = node.client.DownloadString(this.uri);
            //将下载的字符串经过编码转换存入doc中
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlPage);
            //利用PageProcessor方法获得Page数组并将其插入进队列中
            Page[] pages = this.pageProcesser(doc);
            if (pages != null)
            {
                foreach (Page pg in pages)
                    Crawler.InsertIntoQueue(pg);
            }
            node.finished();
        }
    }
}
