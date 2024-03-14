using AiAssistant.Shared.Constants;
using Newtonsoft.Json;

namespace AiAssistant.Shared.Clients.OpenAIClient.Models
{
    public class CreateChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { set; get; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        public CreateChatCompletionRequest(string model, string prompt, string content) 
        {
            Model = model;
            Messages =
                [
                    new Message
                    {
                        Role = Role.System,
                        Content = prompt
                    },
                    new Message
                    {
                        Role = Role.User,
                        Content = content
                    }
                ];
        }
    }
}
