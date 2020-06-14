using DocCrawler.Core;
using DocCrawler.Core.PageImpl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.DocSearch.WanFang
{
    class GetPageNum : PageNum
    {
        public override string urlFormat { get; set; }
        public override Type type { get; set; }
        public override string xpath { get; set; }

        public GetPageNum(string searchStr)
        {
            this.xpath = "//*[@id='here']/div[3]/div[3]/div[2]/div[4]/ul/li[2]/span[3]";
            this.type = WanfangSearch.getType();
            this.urlFormat = "http://www.wanfangdata.com.cn/search/searchList.do?searchType=all&showType=&pageSize=50&page={0}&searchWord=" + searchStr + "&isTriggerTag=";
            this.uri = new Uri("http://www.wanfangdata.com.cn/search/searchList.do?searchType=all&showType=&pageSize=50&page=1&searchWord=" + searchStr + "&isTriggerTag=");
        }
    }
}
