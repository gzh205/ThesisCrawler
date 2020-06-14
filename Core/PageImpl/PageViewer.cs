using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.Core.PageImpl
{
    //爬取论文列表页面的实现
    public abstract class PageViewer : Page
    {
        public abstract string xpath { get; set; }
        public abstract Type type { get; set; }
        public override Page[] pageProcesser(HtmlAgilityPack.HtmlDocument doc)
        {
            Console.WriteLine("正在处理:" + this.uri.AbsoluteUri);
            List<Page> lstPage = new List<Page>();
            //xpath为://*[@id='aysnsearch']/div/node()/div[2]/div[1]/a[1]
            HtmlAgilityPack.HtmlNodeCollection datas = doc.DocumentNode.SelectNodes(xpath);
            if (datas != null)
            {
                foreach (HtmlAgilityPack.HtmlNode data in datas)
                {
                    //通过反射机制动态创建Page的对象
                    Page pg = Activator.CreateInstance(type) as Page;
                    //设置爬取的url
                    pg.uri = new Uri(this.uri, data.GetAttributeValue("href", "").Trim());
                    //将pg添加进链表中
                    lstPage.Add(pg);
                }
            }
            //根据xpath获取下一页的链接
            return lstPage.ToArray();      
        }
    }
}
