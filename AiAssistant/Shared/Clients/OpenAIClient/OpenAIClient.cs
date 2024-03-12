using AiAssistant.Shared.Clients.OpenAIClient.Models;
using Newtonsoft.Json;
using System.Text;

namespace AiAssistant.Shared.Clients.OpenAIClient
{
    public class OpenAIClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public OpenAIClient(string apiKey)
        {
            _client = new HttpClient();
            _apiKey = apiKey;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<CreateChatCompletionResponse> CreateChatCompletion(CreateChatCompletionRequest request)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateChatCompletionResponse>(responseContent);
            }
            else
            {
                throw new Exception($"Failed to generate chat completion: {response.StatusCode}");
            }
        }
    }
}
