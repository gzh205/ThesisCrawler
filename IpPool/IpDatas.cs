using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.IpPool
{
    public class IpDatas
    {
        /// <summary>
        /// 输入一个json字符串，将它封装成一个链表，方便后续操作
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<string> SetDatasJson(string jsonStr)
        {
            ConnectionData dat = new ConnectionData();
            dat.loadFromString(jsonStr);
            InfomationIO.Output.OutputObject(dat);
            List<string> result = new List<string>();
            foreach(DataNode n in dat.data)
            {
                result.Add(n.ip + ":" + n.port);
            }
            return result;
        }
        /// <summary>
        /// 输入一个使用换行符分割的http代理服务器的IP地址和端口号，把它封装成一个链表，方便后续操作
        /// </summary>
        /// <param name="agentStr">保存有http代理服务器的字符串</param>
        /// <returns>存储代理服务器IP和端口号的链表</returns>
        public static List<string> SetDatasString(string agentStr)
        {
            List<string> result = new List<string>();
            string[] ips = agentStr.Split('\n');
            foreach (string ip in ips)
            {
                if (ip != "" || ip.Length != 0)
                    result.Add(ip);
            }
            return result;
        }
        /// <summary>
        /// 根据参数中的文件名打开指定的文件，并获取文件中的数据以字符串的形式返回
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>文件中的内容</returns>
        [Obsolete]
        public static string ReadFromFile(string filename)
        {
            string result = null;
            try
            {
                FileStream file = new FileStream(filename, FileMode.Open);
                StreamReader reader = new StreamReader(file);
                result = reader.ReadToEnd();
                reader.Close();
                file.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("找不到配置文件");
            }
            return result;
        }
        /// <summary>
        /// 根据参数中指定的uri下载对应网页的字符串，HTML或者文本
        /// </summary>
        /// <param name="url"></param>
        /// <returns>下载的字符串</returns>
        public static string ReadFromWeb(string url)
        {
            string result = null;
            try
            {
                WebClient client = new WebClient();
                result = client.DownloadString(url);
            }
            catch (WebException)
            {
                Console.WriteLine("网络异常");
            }
            return result;
        }
    }
}
