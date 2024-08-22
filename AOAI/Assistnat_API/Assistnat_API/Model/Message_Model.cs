namespace Assistant_API.Model
{
    public class Message_Model
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long CreatedAt { get; set; }
        public string AssistantId { get; set; }
        public string ThreadId { get; set; }
        public string RunId { get; set; }
        public string Role { get; set; }
        public List<Content> Content { get; set; } = new List<Content>();
        public List<object> Attachments { get; set; } = new List<object>();
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

    public class Content
    {
        public string Type { get; set; }
        public Text Text { get; set; } = new Text();
    }

    public class Text
    {
        public string Value { get; set; }
        public List<object> Annotations { get; set; } = new List<object>();
    }
}
