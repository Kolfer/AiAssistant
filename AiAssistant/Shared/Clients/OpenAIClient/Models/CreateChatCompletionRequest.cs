using Newtonsoft.Json;

namespace AiAssistant.Shared.Clients.OpenAIClient.Models
{
    public class CreateChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { set; get; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }
}
