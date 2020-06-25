using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocCrawler.IpPool
{
    /// <summary>
    /// http连接池，爬虫的线程从连接池中获取WebClient对象
    /// </summary>
    public class ConnectionPool
    {
        /// <summary>
        /// 链表读取信号量，确保同时只能有一个线程访问链表
        /// </summary>
        private Semaphore read;
        /// <summary>
        /// 一个存储WebCLient连接的链表
        /// </summary>
        private List<WebClientNode> list;
        /// <summary>
        /// 用于存储单例对象的静态变量，以供getInstance方法该对象引用传出
        /// </summary>
        private static ConnectionPool inst;
        /// <summary>
        /// 单例模式，调用该静态方法返回一个唯一的ConnectionPool对象
        /// </summary>
        /// <returns>唯一的ConnectionPool对象</returns>
        public static ConnectionPool getInstance()
        {
            if (inst == null)
                inst = new ConnectionPool();
            return inst;
        }
        /// <summary>
        /// 构造函数，因为是单例模式，因此不允许调用
        /// </summary>
        private ConnectionPool()
        {
            this.read = new Semaphore(1,1);
            this.list = new List<WebClientNode>();
        }
        /// <summary>
        /// 从连接池中获取一个WebClient对象，并且同一时刻只能有一个线程进入该函数
        /// </summary>
        /// <returns>WebClient对象，用于http连接</returns>
        public WebClientNode getOne()
        {
            WebClientNode result = null;
            this.read.WaitOne();
            foreach(WebClientNode node in list)
            {
                if(!node.used)
                {
                    result = node;
                    node.started();
                }
            }
            this.read.Release();
            return result;
        }
        /// <summary>
        /// 相连接池中添加新的连接(不设置代理)
        /// </summary>
        /// <param name="num"></param>
        public void addConnections(int num)
        {
            for(int i=0;i<num;i++)
            {
                this.list.Add(new WebClientNode());
            }
        }
        /// <summary>
        /// 向连接池中添加新的连接(使用代理)
        /// </summary>
        /// <param name="datas">代理服务器的地址的数组</param>
        public void addConnections(List<string> datas)
        {
            foreach(string d in datas)
            {
                this.list.Add(new WebClientNode("http://"+d));
            }
        }
    }
}
