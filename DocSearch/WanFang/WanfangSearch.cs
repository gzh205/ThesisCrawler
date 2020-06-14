using DocCrawler.Core;
using DocCrawler.Core.PageImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.DocSearch.WanFang
{
    public class WanfangSearch:PageViewer
    {
        public override string xpath { get; set; }
        public override Type type { get; set; }

        public WanfangSearch()
        {
            this.xpath = "//*[@id='aysnsearch']/div/node()/div[2]/div[1]/a[1]";
            this.type = WanfangThesis.getType();
        }

        public static Type getType()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().ReflectedType;
        }
    }
}
