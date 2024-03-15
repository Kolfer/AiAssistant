using AiAssistant.Models.CodeStyle;
using AiAssistant.Shared.Clients.OpenAIClient.Models;
using AiAssistant.Shared.Clients.OpenAIClient;
using AiAssistant.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using AiAssistant.Models.UnitTests;

namespace AiAssistant.Controllers.UnitTests
{
    [Route("api/v1/[controller]")]
    public class UnitTestsController(OpenAIClient openAIClient, IConfiguration configuration) : ControllerBase
    {
        private readonly OpenAIClient _openAIClient = openAIClient;

        /// <summary>
        /// Generating unit tests for the provided code snippet
        /// </summary>
        /// <param name="request">Request with a code snippet</param>
        /// <returns>Code snipped with generated unit tests</returns>
        [HttpPost("Generate")]
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

            try
            {
                var response = await _openAIClient.CreateChatCompletion(openAiRequest);

                var choice = response?.Choices?.FirstOrDefault();

                return choice?.FinishReason switch
                {
                    FinishReason.Stop when choice.Message != null => Content(choice.Message.Content),
                    FinishReason.Stop when choice.Message == null => BadRequest("Assistant returned empty message"),
                    FinishReason.Length => BadRequest("Request size is reached"),
                    FinishReason.ContentFilter => BadRequest("Request was filtered by ContentFilters"),
                    _ => BadRequest("Something went wrong")
                };
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}

