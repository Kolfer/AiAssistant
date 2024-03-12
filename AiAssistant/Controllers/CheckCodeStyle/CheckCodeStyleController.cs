using AiAssistant.Models.CheckCodeStyle;
using AiAssistant.Shared.Clients.OpenAIClient;
using AiAssistant.Shared.Clients.OpenAIClient.Models;
using AiAssistant.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AiAssistant.Controllers.CheckCodeStyleController
{
    public class CheckCodeStyleController(string apiKey) : Controller
    {
        public readonly OpenAIClient _openAIClient = new(apiKey);

        [Route("CheckCodeStyle")]
        [HttpGet]
        public async Task<CheckCodeStyleResponse> GenerateCodeStyleAsync(CheckCodeStyleRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                return new CheckCodeStyleResponse("Code is not provided");
            }

            var openAiRequest = new CreateChatCompletionRequest()
            {
                Messages =
                [
                    new Message
                    {
                        Role = Role.System,
                        Content = Prompts.CheckCodeStyle
                    },
                    new Message
                    {
                        Role = Role.User,
                        Content = request.Code
                    }
                ]
            };

            var response = await _openAIClient.CreateChatCompletion(openAiRequest);
            var choice = response?.Choices?.FirstOrDefault();

            if (choice != null)
            {
                switch (choice.FinishReason)
                {
                    case FinishReason.Stop:
                        return new CheckCodeStyleResponse(choice.Message.Content ?? "Assistant returned empty message");
                    case FinishReason.Length:
                        return new CheckCodeStyleResponse("Request size is reached");
                    case FinishReason.ContentFilter:
                        return new CheckCodeStyleResponse("Request was filtered by ContentFilters");
                }
            }
            
            return new CheckCodeStyleResponse("Something went wrong");
        }
    }
}
