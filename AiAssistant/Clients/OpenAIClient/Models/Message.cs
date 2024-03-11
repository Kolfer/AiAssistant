using Newtonsoft.Json;

namespace AiAssistant.Clients.OpenAIClient.Models
{
    public class Message
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }
    }
}
