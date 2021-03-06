﻿using DocCrawler.Exceptions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.Core.PageImpl
{
    /// <summary>
    /// 用于获取最大页面数量的实现
    /// </summary>
    public abstract class PageNum:Page
    {
        public abstract string urlFormat { get; set; }
        public abstract Type type { get; set; }
        public abstract string xpath { get; set; }
        public override Page[] pageProcesser(HtmlDocument doc)
        {
            Console.WriteLine("正在处理:" + this.uri.AbsoluteUri);
            List<Page> resultset = new List<Page>();
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xpath);
            if (nodes == null)
            {
                throw new PageErrorOpenException("无法通过搜索结果的第一页获取总页面数量，可能是服务器拒绝了连接");
            }
            int number = Convert.ToInt32(nodes[0].InnerText.Trim());
            for (int i = 1; i <= number; i++)
            {
                Page p = Activator.CreateInstance(this.type) as Page;
                p.uri = new Uri(string.Format(urlFormat, i));
                resultset.Add(p);
            }
            return resultset.ToArray();
        }
    }
}
