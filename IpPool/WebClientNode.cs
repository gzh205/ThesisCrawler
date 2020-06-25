using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.IpPool
{
    public class WebClientNode
    {
        /// <summary>
        /// 用于创建http连接的WebClient对象
        /// </summary>
        public WebClient client { get; private set; }
        /// <summary>
        /// WebClient对象是否正在被使用
        /// </summary>
        public bool used { get; private set; }
        /// <summary>
        /// 创建一个链表的节点，不设置代理
        /// </summary>
        public WebClientNode()
        {
            this.client = new WebClient();
            this.client.Encoding = System.Text.Encoding.UTF8;
            this.used = false;
        }
        /// <summary>
        /// 创建一个链表的节点，通过第一个参数设置WebClient的代理
        /// </summary>
        /// <param name="agentHost">代理的ip地址:端口号</param>
        public WebClientNode(string agentHost)
        {
            this.client = new WebClient();
            WebProxy proxy = new WebProxy();
            proxy.UseDefaultCredentials = false;
            proxy.Address = new Uri(agentHost);
            this.client.Encoding = System.Text.Encoding.UTF8;
            this.client.Proxy = proxy;
            this.used = false;
        }
        /// <summary>
        /// 当WebCLient使用完毕后，调用此方法将used属性标记为false，以供下一次使用
        /// </summary>
        public void finished()
        {
            this.used = false;
        }
        /// <summary>
        /// 该对象现在被连接池选中，used被标记为true，防止因其他线程重复调用而出错
        /// </summary>
        internal void started()
        {
            this.used = true;
        }
    }
}
