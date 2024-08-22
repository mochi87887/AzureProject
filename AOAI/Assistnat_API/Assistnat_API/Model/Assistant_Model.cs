namespace Assistant_API.Model
{
    // 助理數據模型
    public class Assistant
    {
        public string Id { get; set; } = string.Empty;
        public string Object { get; set; } = string.Empty;
        public long CreatedAt { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public List<object> Tools { get; set; } = new List<object>();
        public double TopP { get; set; }
        public double Temperature { get; set; }
        public Dictionary<string, object> ToolResources { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        public string ResponseFormat { get; set; } = string.Empty;
    }

    // 助理列表的回應模型
    public class AssistantsResponse
    {
        public string Object { get; set; } = string.Empty;
        public List<Assistant> Data { get; set; } = new List<Assistant>();
        public string FirstId { get; set; } = string.Empty;
        public string LastId { get; set; } = string.Empty;
        public bool HasMore { get; set; }
    }
}
