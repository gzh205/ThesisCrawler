namespace DocCrawler.JsonReader
{
    class JsonArray : JsonTree
    {
        public override void constract()
        {
            int brace = 0;
            int bracket = 0;
            int index = 0;
            string data = this.value;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == ',' && brace == 0 && bracket == 0)
                {
                    string tmp = data.Substring(index, i - index);
                    JsonTree node = null;
                    for (int j = 0; j < tmp.Length; j++)
                    {
                        if (tmp[j] == '"')
                        {
                            node = new JsonNode();
                            node.value = tmp.Trim(' ', '\n', '\r');
                            break;
                        }
                        else if (tmp[j] == '{')
                        {
                            node = new JsonObject();
                            node.value = tmp.Trim(' ', '\n', '\r', '{', '}');
                            break;
                        }
                        else if (tmp[j] == '[')
                        {
                            node = new JsonArray();
                            node.value = tmp.Trim(' ', '\n', '\r', '[', ']');
                            break;
                        }
                        else if (j == tmp.Length - 1)
                        {
                            node = new JsonNode();
                            node.value = tmp.Trim(' ', '\n', '\r');
                        }
                    }
                    node.name = this.name;
                    node.isArrayElement = true;
                    this.nodes.Add(node);
                    index = i + 1;
                }
                if (data[i] == '{') brace++;
                else if (data[i] == '}') brace--;
                else if (data[i] == '[') bracket++;
                else if (data[i] == ']') bracket--;
            }
            if (brace == 0 && bracket == 0)
            {
                string tmp = data.Substring(index, data.Length - index);
                JsonTree node = null;
                for (int j = 0; j < tmp.Length; j++)
                {
                    if (tmp[j] == '"')
                    {
                        node = new JsonNode();
                        node.value = tmp.Trim(' ', '\n', '\r');
                        break;
                    }
                    else if (tmp[j] == '{')
                    {
                        node = new JsonObject();
                        node.value = tmp.Trim(' ', '\n', '\r', '{', '}');
                        break;
                    }
                    else if (tmp[j] == '[')
                    {
                        node = new JsonArray();
                        node.value = tmp.Trim(' ', '\n', '\r', '[', ']');
                        break;
                    }
                    else if (j == tmp.Length - 1)
                    {
                        node = new JsonNode();
                        node.value = tmp.Trim(' ', '\n', '\r');
                    }
                }
                node.name = this.name;
                node.isArrayElement = true;
                this.nodes.Add(node);
            }
        }
    }
}
