using Newtonsoft.Json;

namespace AiAssistant.Shared.Clients.OpenAIClient.Models
{
    public class Message
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }
    }
}
