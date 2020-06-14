using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
namespace DocCrawler.cdpf
{
    public class Connection
    {
        public SqlConnection sqlConn { get; private set; }
        protected SqlCommand cmd { get; set; }
        public string sql { get; protected set; }
        /// <summary>
        /// 构造函数，不建议直接调用
        /// </summary>
        /// <param name="sqlConn"></param>
        public Connection(SqlConnection sqlConn)
        {
            this.sqlConn = sqlConn;
        }
        /// <summary>
        /// 自定义select查询的where语句部分,其中select与from均可省略,如不省略,该方法也会自动忽略
        /// </summary>
        /// <typeparam name="T">继承自Table类的数据库表类型</typeparam>
        /// <param name="where">自定义的where语句</param>
        /// <returns>返回所有符合where语句限定条件的记录</returns>
        public T[] SelectSome<T>(string where)
        {
            if (where == null)
                where = "";
            if (where.Contains(where) && !where.Contains("'where'") && !where.Contains("\"where\""))
            {
                where = Regex.Replace(where, ".*where", "where");
            }
            List<T> table = new List<T>();
            FieldInfo[] fields = typeof(T).GetFields();
            PropertyInfo[] properties = typeof(T).GetProperties();
            this.sql = "select ";
            string selectStr = "";
            for (int i = 0; i < fields.Length; i++)
                selectStr += ","+fields[i].Name;
            for (int i = 0; i < properties.Length; i++)
                selectStr += "," + properties[i].Name;
            this.sql += selectStr.Substring(1) + " from " + typeof(T).Name + where;
            cmd = new SqlCommand(sql, this.sqlConn);
            this.sqlConn.Open();
            SqlDataReader sdr = this.cmd.ExecuteReader();
            fields = null;
            properties = null;
            while(sdr.Read())
            {
                T t = System.Activator.CreateInstance<T>();
                FieldInfo[] tfields = t.GetType().GetFields();
                PropertyInfo[] tproperties = t.GetType().GetProperties();
                for (int i = 0; i < tfields.Length; i++) 
                {
                    tfields[i].SetValue(t, sdr[tfields[i].Name]);
                }
                for(int i = 0; i < tproperties.Length; i++)
                {
                    tproperties[i].SetValue(t, sdr[tproperties[i].Name]);
                }
                table.Add(t);
            }
            this.sqlConn.Close();
            return table.ToArray();
        }
        /// <summary>
        /// 查询数据库中的记录
        /// </summary>
        /// <param name="table">某条记录的对象,继承自Table,只要[PrimaryKey]有值即可,其余成员会自动填充完成</param>
        public void Select(Table table)
        {
            FieldInfo[] fields;
            PropertyInfo[] properties;
            LinkedList<Node>[] lists = this.getFieldsAndAttributes(table,out fields,out properties);
            this.sql = "select ";
            string selectStr = "";
            string whereStr = "";
            foreach(Node node in lists[0])
            {
                whereStr += " and " + node.value + "=" + node.name;
            }
            foreach(Node node in lists[1])
            {
                selectStr += "," + node.name;
            }
            whereStr = whereStr.Substring(5);
            selectStr = selectStr.Substring(1);
            this.sql += selectStr + " from " + table.GetType().Name + " where " + whereStr;
            cmd = new SqlCommand(sql, this.sqlConn);
            this.sqlConn.Open();
            SqlDataReader sdr = this.cmd.ExecuteReader();
            sdr.Read();
            //int index = 0;
            for(int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(table, sdr[fields[i].Name]);
            }
            for(int i = 0; i < properties.Length; i++)
            {
                properties[i].SetValue(table, sdr[properties[i].Name]);
            }
            this.sqlConn.Close();
        }
        /// <summary>
        /// 修改数据库表中的记录
        /// </summary>
        /// <param name="table"></param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool Update(Table table)
        {
            LinkedList<Node>[] lists = this.getFieldsAndAttributes(table);
            this.sql = "update " + table.GetType().Name + " set ";
            string setStr = "";
            string whereStr = "";
            foreach(Node node in lists[1])
            {
                setStr += "," + node.name + "=" + node.value;
            }
            foreach(Node node in lists[0])
            {
                whereStr += " and " + node.name + "=" + node.value;
            }
            this.sql += setStr.Substring(1) + " where " + whereStr.Substring(5);
            cmd = new SqlCommand(sql, this.sqlConn);
            this.sqlConn.Open();
            int n = this.cmd.ExecuteNonQuery();
            this.sqlConn.Close();
            return n > 0;
        }
        /// <summary>
        /// 从数据库表中删除一条记录
        /// </summary>
        /// <param name="table">某条记录的对象,继承自Table,只要[PrimaryKey]正确即可,其余成员均可为null或任意值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool Delete(Table table)
        {
            LinkedList<Node>[] lists = this.getFieldsAndAttributes(table);
            this.sql = "delete from " + table.GetType().Name + " where ";
            string whereString = "";
            foreach (Node node in lists[0])
                whereString += " and " + node.name + "=" + node.value;
            this.sql += whereString.Substring(5);
            cmd = new SqlCommand(sql, this.sqlConn);
            this.sqlConn.Open();
            int n = this.cmd.ExecuteNonQuery();
            this.sqlConn.Close();
            return n > 0;
        }
        /// <summary>
        /// 将一条记录插入数据库的表中
        /// </summary>
        /// <param name="table">某条记录的对象,继承自Table,[PrimaryKey]标记表示该成员为主关键字</param>
        /// <returns>插入成功返回true,失败返回false</returns>
        public bool Insert(Table table)
        {
            LinkedList<Node> lists = this.getFieldsAndAttributesWithoutIdentity(table);
            this.sql = "insert into " + table.GetType().Name;
            string name = "(";
            string value = "(";
            foreach (Node node in lists)
            {
                name += node.name + ",";
                value += node.value + ",";
            }
            name = name.Remove(name.Length - 1) + ")";
            value = value.Remove(value.Length - 1) + ")";
            sql += name + " values" + value;
            cmd = new SqlCommand(sql,this.sqlConn);
            sqlConn.Open();
            int n = cmd.ExecuteNonQuery();
            sqlConn.Close();
            return n > 0;
        }
        /// <summary>
        /// 将table中的数据插入到数据库的表中(异步执行版本)
        /// </summary>
        /// <param name="table"></param>
        public async Task InsertAsync(Table table)
        {
            LinkedList<Node> lists = this.getFieldsAndAttributesWithoutIdentity(table);
            this.sql = "insert into " + table.GetType().Name;
            string name = "(";
            string value = "(";
            foreach (Node node in lists)
            {
                name += node.name + ",";
                value += node.value + ",";
            }
            name = name.Remove(name.Length - 1) + ")";
            value = value.Remove(value.Length - 1) + ")";
            sql += name + " values" + value;
            cmd = new SqlCommand(sql, this.sqlConn);
            await this.sqlConn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// 查找并输出table中的所有属性和方法(除了有Identity注解的属性或方法)
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private LinkedList<Node> getFieldsAndAttributesWithoutIdentity(Table table)
        {
            LinkedList<Node> list = new LinkedList<Node>();
            FieldInfo[] fields = table.GetType().GetFields();
            PropertyInfo[] properties = table.GetType().GetProperties();
            foreach (FieldInfo field in fields)
            {
                string value = field.GetValue(table).ToString();
                if (field.FieldType.Name == "String" || field.FieldType.Name == "DateTime")
                    value = "'" + value + "'";
                if (field.GetCustomAttribute(typeof(Identity)) == null)
                    list.AddLast(new Node(field.Name, value));
            }
            foreach (PropertyInfo property in properties)
            {
                string value = property.GetValue(table).ToString();
                if (property.PropertyType.Name == "String" || property.PropertyType.Name == "DateTime")
                    value = "'" + value + "'";
                if (property.GetCustomAttribute(typeof(Identity)) == null)
                    list.AddLast(new Node(property.Name, value));
            }
            return list;
        }
        /// <summary>
        /// 查找table表中所有的成员并且输出它们的ID和值
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private LinkedList<Node>[] getFieldsAndAttributes(Table table)
        {
            LinkedList<Node>[] lists = new LinkedList<Node>[2];
            for (int i = 0; i < lists.Length; i++)
                lists[i] = new LinkedList<Node>();
            FieldInfo[] fields = table.GetType().GetFields();
            PropertyInfo[] properties = table.GetType().GetProperties();
            foreach(FieldInfo field in fields)
            {
                string value = field.GetValue(table).ToString();
                if (field.FieldType.Name == "String" || field.FieldType.Name == "DateTime")
                    value = "'" + value + "'";
                if (field.GetCustomAttribute(typeof(PrimaryKey)) == null)
                    lists[1].AddLast(new Node(field.Name,value));
                else
                    lists[0].AddLast(new Node(field.Name, value));
            }
            foreach(PropertyInfo property in properties)
            {
                string value = property.GetValue(table).ToString();
                if (property.PropertyType.Name == "String" || property.PropertyType.Name == "DateTime")
                    value = "'" + value + "'";
                if (property.GetCustomAttribute(typeof(PrimaryKey)) == null)
                    lists[1].AddLast(new Node(property.Name, value));
                else
                    lists[0].AddLast(new Node(property.Name, value));
            }
            return lists;
        }
        /// <summary>
        /// 查找table表中所有的成员并且输出它们的ID和值,并且获得他们的对象
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fInfos"></param>
        /// <param name="pInfos"></param>
        /// <returns></returns>
        private LinkedList<Node>[] getFieldsAndAttributes(Table table,out FieldInfo[] fInfos,out PropertyInfo[] pInfos)
        {
            List<FieldInfo> datField = new List<FieldInfo>();
            List<PropertyInfo> datProperty = new List<PropertyInfo>();
            LinkedList<Node>[] lists = new LinkedList<Node>[2];
            for (int i = 0; i < lists.Length; i++)
                lists[i] = new LinkedList<Node>();
            FieldInfo[] fields = table.GetType().GetFields();
            PropertyInfo[] properties = table.GetType().GetProperties();
            foreach (FieldInfo field in fields)
            {
                lists[1].AddLast(new Node(field.Name, null));
                datField.Add(field);
                if (field.GetCustomAttribute(typeof(PrimaryKey)) != null)
                {
                    string value = field.GetValue(table).ToString();
                    if (field.FieldType.Name == "String" || field.FieldType.Name == "DateTime")
                        value = "'" + value + "'";
                    lists[0].AddLast(new Node(field.Name, value));
                }
            }
            foreach (PropertyInfo property in properties)
            {
                lists[1].AddLast(new Node(property.Name, null));
                datProperty.Add(property);
                if (property.GetCustomAttribute(typeof(PrimaryKey)) != null)
                {
                    string value = property.GetValue(table).ToString();
                    if (property.PropertyType.Name == "String" || property.PropertyType.Name == "DateTime")
                        value = "'" + value + "'";
                    lists[0].AddLast(new Node(property.Name, value));
                }
            }
            fInfos = datField.ToArray();
            pInfos = datProperty.ToArray();
            return lists;
        }
    }
}
