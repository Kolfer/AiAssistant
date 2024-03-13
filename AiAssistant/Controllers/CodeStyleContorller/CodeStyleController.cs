using AiAssistant.Models.CodeStyle;
using AiAssistant.Shared.Clients.OpenAIClient;
using AiAssistant.Shared.Clients.OpenAIClient.Models;
using AiAssistant.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AiAssistant.Controllers.CodeStyleController
{
    public class CodeStyleController(IConfiguration configuration) : Controller
    {
        public readonly OpenAIClient _openAIClient = new(configuration["AppSettings:ApiKey"]);

        [Route("CheckCodeStyle")]
        [HttpGet]
        
        public async Task<FixStyleResponse> FixStyleAsync(FixStyleRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                return new FixStyleResponse("Code is not provided");
            }

            var openAiRequest = new CreateChatCompletionRequest()
            {
                Model = configuration["AppSettings:Model"],
                Messages =
                [
                    new Message
                    {
                        Role = Role.System,
                        Content = Prompts.FixStyle
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
                        return new FixStyleResponse(choice.Message.Content ?? "Assistant returned empty message");
                    case FinishReason.Length:
                        return new FixStyleResponse("Request size is reached");
                    case FinishReason.ContentFilter:
                        return new FixStyleResponse("Request was filtered by ContentFilters");
                }
            }
            
            return new FixStyleResponse("Something went wrong");
        }
    }
}
