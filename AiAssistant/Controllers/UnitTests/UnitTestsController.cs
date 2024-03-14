using AiAssistant.Models.CodeStyle;
using AiAssistant.Shared.Clients.OpenAIClient.Models;
using AiAssistant.Shared.Clients.OpenAIClient;
using AiAssistant.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using AiAssistant.Models.UnitTests;

namespace AiAssistant.Controllers.UnitTests
{
    [Route("api/v1/[controller]")]
    public class UnitTestsController(IConfiguration configuration) : ControllerBase
    {
        private readonly OpenAIClient _openAIClient = new(configuration["AppSettings:ApiKey"]);

        /// <summary>
        /// Generating unit tests for the provided code snippet
        /// </summary>
        /// <param name="request">Request with a code snippet</param>
        /// <returns>Code snipped with generated unit tests</returns>
        [HttpGet("Generate")]
        [Produces("text/plain")]
        public async Task<ActionResult<GenerateResponse>> GenerateAsync(GenerateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest("Code is not provided");
            }

            var openAiRequest = new CreateChatCompletionRequest(
                configuration["AppSettings:Model"],
                Prompts.GenerateUnitTests,
                request.Code);

            var response = await _openAIClient.CreateChatCompletion(openAiRequest);
            var choice = response?.Choices?.FirstOrDefault();

            if (choice != null)
            {
                switch (choice.FinishReason)
                {
                    case FinishReason.Stop:
                        return choice.Message.Content != null
                            ? Content(choice.Message.Content)
                            : BadRequest("Assistant returned empty message");
                    case FinishReason.Length:
                        return BadRequest("Request size is reached");
                    case FinishReason.ContentFilter:
                        return BadRequest("Request was filtered by ContentFilters");
                }
            }

            return BadRequest("Something went wrong");
        }
    }
}

