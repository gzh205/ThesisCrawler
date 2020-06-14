using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace DocCrawler.Core
{
    public class Crawler
    {
        private static ConcurrentQueue<Page> queue;
        private static Thread[] threads;
        public static int threadNum;
        private static Semaphore read;
        private static Semaphore queueNum;
        private static Crawler inst;
        public static Crawler GetCrawler()
        {
            if (inst == null)
            {
                Crawler.inst = new Crawler();
            }
            return Crawler.inst;
        }
        private Crawler()
        {
            queue = new ConcurrentQueue<Page>();
            Crawler.threadNum = 1;
            threads = new Thread[threadNum];
            Crawler.read = new Semaphore(1, 1);
            Crawler.queueNum = new Semaphore(0, int.MaxValue);
        }
        public Crawler setThreadNum(int num)
        {
            if (threadNum > 0)
            {
                Crawler.threadNum = num;
            }
            return this;
        }
        public void run(Page[] pages)
        {
            foreach (Page p in pages)
            {
                Crawler.queue.Enqueue(p);
                Crawler.queueNum.Release();
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(ThreadProc);
                threads[i].Start();
            }
        }
        public static void ObservProc()
        {
            int count = 0;
            while (count < threads.Length || threads.Length > 0)
            {
                for (int i = 0; i < threads.Length; i++)
                {
                    if (threads[i].ThreadState == ThreadState.Unstarted)
                    {
                        count++;
                    }
                }
            }
            for (int i = 0; i < threads.Length; i++)
                threads[i].Abort();
        }
        public static Page ReadFromQueue()
        {
            read.WaitOne();
            queueNum.WaitOne();
            Page p;
            bool res = queue.TryDequeue(out p);
            read.Release();
            return res?p:null;
        }
        public static void InsertIntoQueue(Page p)
        {
            queue.Enqueue(p);
            queueNum.Release();
        }
        private void ThreadProc()
        {
            while (true)
            {      
                read.WaitOne();
                queueNum.WaitOne();
                Page page;
                bool res = queue.TryDequeue(out page);
                read.Release();
                if(res)
                    page.PageAnalysis();
            }
        }
    }
}
