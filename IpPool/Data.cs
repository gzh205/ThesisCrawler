using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.IpPool
{
    internal class Data
    {
        public String ip { get; private set; }
        public int port { get; private set; }
        public Data(String ip,int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }
}
