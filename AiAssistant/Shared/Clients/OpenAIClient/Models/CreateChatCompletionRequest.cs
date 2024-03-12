using Newtonsoft.Json;

namespace AiAssistant.Shared.Clients.OpenAIClient.Models
{
    public class CreateChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { set; get; } = "gpt-3.5-turbo";

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }
}
