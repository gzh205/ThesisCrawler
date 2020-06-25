using DocCrawler.cdpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocCrawler.DataBaseInsert
{
    /// <summary>
    /// 数据库数据写入线程池，根据任务队列中的数据库动态调整数据存储线程数量
    /// </summary>
    class DataWriter
    {
        //数据表对象队列
        private static Queue<ThesisData> tasks;
        //队列中的元素数量信号量
        private static Semaphore taskNum;
        //线程读取信号量
        private static Semaphore read;
        //线程列表
        private static Thread[] threads;
        //该对象的指针(单例模式)
        private static DataWriter inst;
        //得到一个对象
        public static DataWriter GetWriter()
        {
            if (inst == null)
                DataWriter.inst = new DataWriter();
            return DataWriter.inst;
        }
        public void run()
        {
            for(int i=0;i<threads.Length;i++)
            {
                threads[i] = new Thread(this.ThreadProc);
                threads[i].Start();
            }
        }
        public static void Insert(ThesisData data)
        {
            tasks.Enqueue(data);
            taskNum.Release();
        }
        private DataWriter()
        {
            DataWriter.tasks = new Queue<ThesisData>();
            DataWriter.taskNum = new Semaphore(0, int.MaxValue);
            DataWriter.read = new Semaphore(1, 1);
            DataWriter.threads = new Thread[8];
        }
        private void ThreadProc()
        {
            //从任务队列中取出一个任务
            taskNum.WaitOne();
            read.WaitOne();
            ThesisData dat = tasks.Dequeue();
            read.Release();
            dat.abs = dat.abs.Trim(' ','\"','\'','\r','\n','\t');
            dat.title = dat.title.Trim(' ', '\"', '\'', '\r', '\n', '\t');
            dat.url = dat.url.Trim(' ', '\"', '\'', '\r', '\n', '\t');
            //执行数据库操作
            ConnectionFactory.getConnection(ConfigurationManager.ConnectionStrings["ThesisConn"].ToString()).Insert(dat);
        }
        public static void ObservProc()
        {
            int count = 0;
            while (count < threads.Length || threads.Length > 0)
            {
                foreach (Thread t in threads)
                {
                    if (t.ThreadState == ThreadState.Unstarted)
                    {
                        count++;
                    }
                }
            }
            foreach(Thread t in threads)
            {
                t.Abort();
            }
        }
    }
}
