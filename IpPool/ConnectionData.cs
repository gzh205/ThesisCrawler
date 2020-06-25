using DocCrawler.JsonReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.IpPool
{
    internal class ConnectionData:Json
    {
        public int code;
        public DataNode[] data;
        public string msg;
        public bool success;
    }
    internal class DataNode
    {
        public string ip;
        public int port;
        public DateTime expire_time;
        public string city;
        public string isp;  
    }
}
