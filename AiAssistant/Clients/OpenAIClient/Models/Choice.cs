using Newtonsoft.Json;

namespace AiAssistant.Clients.OpenAIClient.Models
{
    public class Choice
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("logprobs")]
        public object? LogProbs { get; set; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }
    }
}
