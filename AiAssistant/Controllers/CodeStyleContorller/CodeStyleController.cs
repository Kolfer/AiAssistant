using AiAssistant.Models.CodeStyle;
using AiAssistant.Shared.Clients.OpenAIClient;
using AiAssistant.Shared.Clients.OpenAIClient.Models;
using AiAssistant.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AiAssistant.Controllers.CodeStyleController
{
    [Route("api/v1/[controller]")]
    public class CodeStyleController(IConfiguration configuration) : Controller
    {
        public readonly OpenAIClient _openAIClient = new(configuration["AppSettings:ApiKey"]);

        [HttpGet("FixStyle")]
        public async Task<ActionResult<FixStyleResponse>> FixStyleAsync(FixStyleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest("Code is not provided");
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
                        return choice.Message.Content != null
                            ? Ok( new FixStyleResponse (choice.Message.Content))
                            : BadRequest ("Assistant returned empty message");
                    case FinishReason.Length:
                        return BadRequest ("Request size is reached");
                    case FinishReason.ContentFilter:
                        return BadRequest ("Request was filtered by ContentFilters");
                }
            }
            
            return BadRequest ("Something went wrong");
        }
    }
}
