﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.cdpf
{
    public class Node
    {
        public string name;
        public string value;
        public Node()
        {
        }
        public Node(string name,string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
