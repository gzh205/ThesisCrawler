using DocCrawler.cdpf;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.Core.PageImpl
{
    /// <summary>
    /// 爬取论文具体内容页面的实现
    /// </summary>
    public abstract class PageThesis : Page
    {
        /// <summary>
        /// 论文标题的xpath
        /// </summary>
        public abstract string xpathTitle { get; set; }
        /// <summary>
        /// 论文摘要的xpath
        /// </summary>
        public abstract string xpathAbstract { get; set; }
        /// <summary>
        /// 爬取算法的实现
        /// </summary>
        /// <param name="doc">html文档</param>
        /// <returns></returns>
        public override Page[] pageProcesser(HtmlDocument doc)
        {
            Console.WriteLine("正在处理:" + this.uri.AbsoluteUri);
            //查找论文的标题
            HtmlAgilityPack.HtmlNodeCollection datas = doc.DocumentNode.SelectNodes(xpathTitle);
            if (datas == null)
                return null;
            string title = datas[0].InnerText.Trim();
            //查找论文的摘要
            datas = doc.DocumentNode.SelectNodes(xpathAbstract);
            if (datas == null)
                return null;
            string abs = datas[0].InnerText.Trim();
            //将标题、摘要和链接保存进数据库中
            insert(title, abs, this.uri.AbsoluteUri);
            return null;
        }
        /// <summary>
        /// 将爬到的数据插入数据库的算法
        /// </summary>
        /// <param name="title">论文标题</param>
        /// <param name="abs">论文摘要</param>
        /// <param name="url">论文的链接</param>
        public abstract void insert(string title,string abs,string url);
    }
}
