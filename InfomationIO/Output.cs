using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocCrawler.InfomationIO
{
    public class Output
    {
        public static void OutputObject(object obj)
        {
            Type type = obj.GetType();
            Console.WriteLine(type.Name + "{");
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo f in fields)
            {
                Console.WriteLine(f.Name + "=" + f.GetValue(obj).ToString());
            }
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo p in properties)
            {
                Console.WriteLine(p.Name + "=" + p.GetValue(obj).ToString());
            }
            Console.WriteLine("}");
        }
    }
}
