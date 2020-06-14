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
        private WebClient client 
        { 
            get; 
            set; 
        }
        public Uri uri
        {
            get;
            set;
        }
        public abstract Page[] pageProcesser(HtmlAgilityPack.HtmlDocument doc);
        public void PageAnalysis()
        {
            client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Page.DownloadConplete);
            client.DownloadStringAsync(this.uri, this);
        }
        public static void DownloadConplete(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                Page page = e.UserState as Page;
                //将下载的字符串经过编码转换存入doc中
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(e.Result);
                //利用PageProcessor方法获得Page数组并将其插入进队列中
                Page[] pages = page.pageProcesser(doc);
                if (pages != null)
                {
                    foreach (Page pg in pages)
                        Crawler.InsertIntoQueue(pg);
                }
            }
            catch (Exception){
                return;
            }
        }
    }
}
