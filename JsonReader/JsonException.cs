using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.JsonReader
{
    class JsonException : Exception
    {
        public JsonException(string message) : base(message)
        {
        }
        public static string NullInput = "输入为空";
    }
}
