namespace DocCrawler.JsonReader
{
    class JsonObject : JsonTree
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
                    string[] tmp = data.Substring(index, i - index).Split(new char[]{ ':'},2);
                    JsonTree node = null;
                    for (int j = 0; j < tmp[1].Length; j++)
                    {
                        if (tmp[1][j] == '"')
                        {
                            node = new JsonNode();
                            node.value = tmp[1].Trim(' ', '\n', '\r');
                            break;
                        }
                        else if (tmp[1][j] == '{')
                        {
                            string rowStr = "";
                            for (int k = 1; k < tmp.Length; k++)
                                rowStr += ":" + tmp[k];
                            node = new JsonObject();
                            node.value = rowStr.Substring(1).Trim(' ', '{', '}', '\n', '\r');
                            break;
                        }
                        else if (tmp[1][j] == '[')
                        {
                            string rowStr = "";
                            for (int k = 1; k < tmp.Length; k++)
                                rowStr += ":" + tmp[k];
                            node = new JsonArray();
                            node.value = rowStr.Substring(1).Trim(' ', '[', ']', '\n', '\r');
                            break;
                        }
                        else if (j == tmp[1].Length - 1)
                        {
                            node = new JsonNode();
                            node.value = tmp[1].Trim(' ', '\n', '\r');
                        }
                    }
                    node.name = tmp[0].Trim(' ', '\n', '\r', '"', ',');
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
                string[] tmp = data.Substring(index, data.Length - index).Split(new char[] { ':' }, 2);
                JsonTree node = null;
                for (int j = 0; j < tmp[1].Length; j++)
                {
                    if (tmp[1][j] == '"')
                    {
                        node = new JsonNode();
                        node.value = tmp[1].Trim(' ', '\n', '\r');
                        break;
                    }
                    else if (tmp[1][j] == '{')
                    {
                        string rowStr = "";
                        for (int k = 1; k < tmp.Length; k++)
                            rowStr += ":" + tmp[k];
                        node = new JsonObject();
                        node.value = rowStr.Substring(1).Trim(' ', '{', '}', '\n', '\r');
                        break;
                    }
                    else if (tmp[1][j] == '[')
                    {
                        string rowStr = "";
                        for (int k = 1; k < tmp.Length; k++)
                            rowStr += ":" + tmp[k];
                        node = new JsonArray();
                        node.value = rowStr.Substring(1).Trim(' ', '[', ']', '\n', '\r');
                        break;
                    }
                    else if (j == tmp[1].Length - 1)
                    {
                        node = new JsonNode();
                        node.value = tmp[1].Trim(' ', '\n', '\r');
                    }
                }
                node.name = tmp[0].Trim(' ', '\n', '\r', '"', ',');
                this.nodes.Add(node);
            }
        }
    }
}
