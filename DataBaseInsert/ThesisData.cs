using DocCrawler.cdpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.DataBaseInsert
{
    public class ThesisData:Table
    {
        //主键(自动增长)
        [PrimaryKey]
        [Identity]
        public int id { get; set; }
        //论文的标题
        public string title { get; set; }
        //摘要
        public string abs { get; set; }
        //论文的链接
        public string url { get; set; }
        public ThesisData(string title,string abs,string url)
        {
            this.id = 0;
            this.title = title;
            this.abs = abs;
            this.url = url;
        }
        public ThesisData() { this.id = 0; }
    }
}
