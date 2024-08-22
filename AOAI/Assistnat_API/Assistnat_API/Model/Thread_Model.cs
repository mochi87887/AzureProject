namespace Assistant_API.Model
{
    // 建立執行緒的回應模型
    public class Thread_Model
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long CreatedAt { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> ToolResources { get; set; } = new Dictionary<string, object>();
    }

    // ToolResources 類別
    public class ToolResources
    {
        public CodeInterpreter CodeInterpreter { get; set; } = new CodeInterpreter();
    }

    // CodeInterpreter 類別
    public class CodeInterpreter
    {
        public List<string> FileIds { get; set; } = new List<string>();
    }
}
