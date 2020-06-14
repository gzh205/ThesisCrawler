using DocCrawler.cdpf;
using DocCrawler.Core.PageImpl;
using DocCrawler.DataBaseInsert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.DocSearch.WanFang
{
    class WanfangThesis:PageThesis
    {
        public override string xpathTitle { get; set; }
        public override string xpathAbstract { get; set; }

        public WanfangThesis()
        {
            this.xpathTitle = "//*[@id='div_a']/div/div[2]/div[1]/div[3]";
            this.xpathAbstract = "//*[@id='see_alldiv']/text()";
        }

        public override void insert(string title, string abs, string url)
        {
            DataBaseInsert.DataWriter.Insert(new ThesisData(title,abs,url));
        }
        public static Type getType()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().ReflectedType;
        }
    }
}
