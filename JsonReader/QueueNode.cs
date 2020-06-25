namespace DocCrawler.JsonReader
{
    class QueueNode
    {
        public JsonTree tree;
        public object obj;
        public QueueNode(JsonTree tree, object obj)
        {
            this.tree = tree;
            this.obj = obj;
        }
        public QueueNode() { }
    }
}
