namespace Assistant_API.Model
{
    public class Run_Model
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long CreatedAt { get; set; }
        public string AssistantId { get; set; }
        public string ThreadId { get; set; }
        public string Status { get; set; }
        public long? StartedAt { get; set; }
        public long ExpiresAt { get; set; }
        public long? CancelledAt { get; set; }
        public long? FailedAt { get; set; }
        public long? CompletedAt { get; set; }
        public string RequiredAction { get; set; }
        public string LastError { get; set; }
        public string Model { get; set; }
        public string Instructions { get; set; }
        public object[] Tools { get; set; }
        public object ToolResources { get; set; }
        public object Metadata { get; set; }
        public double Temperature { get; set; }
        public double TopP { get; set; }
        public int? MaxCompletionTokens { get; set; }
        public int? MaxPromptTokens { get; set; }
        public TruncationStrategy TruncationStrategy { get; set; }
        public object IncompleteDetails { get; set; }
        public object Usage { get; set; }
        public string ResponseFormat { get; set; }
        public string ToolChoice { get; set; }
        public bool ParallelToolCalls { get; set; }
    }

    public class TruncationStrategy
    {
        public string Type { get; set; }
        public object LastMessages { get; set; }
    }
}