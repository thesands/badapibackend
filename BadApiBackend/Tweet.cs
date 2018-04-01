namespace BadApiBackend
{
    public class Tweet
    {
        private readonly string id;
        private readonly string stamp;
        private readonly string text;
        
        public string Id { get { return id; } }
        public string Stamp { get { return stamp; } }
        public string Text { get { return text; } }


        public Tweet(string id, string stamp, string text)
        {
            this.id = id;
            this.stamp = stamp;
            this.text = text;
        }

        public override string ToString()
        {
            return string.Format("id: {0}, stamp: {1}, text: {2}", Id, Stamp, Text);
        }
    }
}