using System;
using System.Linq;

namespace DocCrawler.JsonReader
{
    public class Tool
    {
        public static bool validStr(string data)
        {
            foreach (char c in data)
            {
                if (c > 31 && c != 127)
                {
                    return true;
                }
            }
            return false;
        }
        public static string trimStr(string data)
        {
            string result = "";
            if (data.Contains('"'))
            {
                bool isData = false;
                foreach (char c in data)
                {
                    if (c == '"')
                    {
                        isData = !isData;
                        continue;
                    }
                    if (isData)
                        result += c;
                }
            }
            else
            {
                foreach (char c in data)
                {
                    if (c != '\n' && c != '\r' && c != '"' && c != '\t')
                        result += c;
                }
            }
            return result.Trim();
        }
        public static object setValue(string dat, Type t)
        {
            if (t == typeof(string))
            {
                return dat;
            }
            else if (t == typeof(int))
            {
                return Convert.ToInt32(dat);
            }
            else if (t == typeof(double))
            {
                return Convert.ToDouble(dat);
            }
            else if (t == typeof(DateTime))
            {
                return Convert.ToDateTime(dat);
            }
            else if(t==typeof(bool))
            {
                return Convert.ToBoolean(dat);
            }
            else throw new Exception("目前不支持除了string,int,DateTime,double之外的其他类型");
        }
    }
}
