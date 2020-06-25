using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DocCrawler.JsonReader
{
    class JsonReader
    {
        public void createObject(JsonTree tree, object obj)
        {
            Type type = obj.GetType();
            object o = obj;
            Queue<QueueNode> queue = new Queue<QueueNode>();
            queue.Enqueue(new QueueNode(tree, obj));
            while (true)
            {
                QueueNode node = null;
                try
                {
                    node = queue.Dequeue();
                }
                catch (InvalidOperationException)
                {
                    break;
                }
                for (int i = 0; i < node.tree.nodes.Count; i++)
                {

                    if (node.tree.GetType() == typeof(JsonArray))
                    {
                        Array arr = (Array)node.obj;
                        if (node.tree.nodes[i].GetType() == typeof(JsonNode))
                        {
                            arr.SetValue(Tool.setValue(node.tree.nodes[i].value.Trim(' ', '"'), arr.GetType().GetElementType()), i);
                        }
                        else if (node.tree.nodes[i].GetType() == typeof(JsonObject))
                        {
                            //这里出现了问题创建对象为空
                            object oo = Activator.CreateInstance(arr.GetType().GetElementType());
                            arr.SetValue(oo, i);
                            queue.Enqueue(new QueueNode(node.tree.nodes[i], oo));
                        }
                        else if (node.tree.nodes[i].GetType() == typeof(JsonArray))
                        {
                            object oo = Array.CreateInstance(node.tree.nodes[i].GetType().GetElementType(), node.tree.nodes[i].nodes.Count);
                            arr.SetValue(oo, i);
                            queue.Enqueue(new QueueNode(node.tree.nodes[i], oo));
                        }
                    }
                    else
                    {
                        FieldInfo field = node.obj.GetType().GetField(node.tree.nodes[i].name);
                        if (node.tree.nodes[i].GetType() == typeof(JsonNode))
                        {
                            field.SetValue(node.obj, Tool.setValue(node.tree.nodes[i].value.Trim(' ', '"'), field.FieldType));
                        }
                        else if (node.tree.nodes[i].GetType() == typeof(JsonObject))
                        {
                            object oo = Activator.CreateInstance(field.FieldType);
                            field.SetValue(node.obj, oo);
                            queue.Enqueue(new QueueNode(node.tree.nodes[i], oo));
                        }
                        else if (node.tree.nodes[i].GetType() == typeof(JsonArray))
                        {
                            object oo = Array.CreateInstance(field.FieldType.GetElementType(), node.tree.nodes[i].nodes.Count);
                            field.SetValue(node.obj, oo);
                            queue.Enqueue(new QueueNode(node.tree.nodes[i], oo));
                        }
                    }
                }
            }
        }
        public JsonTree createTree(string data)
        {
            JsonTree head = new JsonObject();
            head.value = data.Trim(' ', '\n', '\r', '{', '}');
            Queue<JsonTree> queue = new Queue<JsonTree>();
            queue.Enqueue(head);
            while (true)
            {
                JsonTree tree = null;
                try
                {
                    tree = queue.Dequeue();
                }
                catch (InvalidOperationException)
                {
                    break;
                }
                tree.constract();
                foreach (JsonTree node in tree.nodes)
                    queue.Enqueue(node);
            }
            return head;
        }
        public string loadFromFile(string filename)
        {
            StreamReader sr = new StreamReader(new FileStream(filename, FileMode.Open));
            string data = sr.ReadToEnd();
            sr.Close();
            return data;
        }
    }
}
