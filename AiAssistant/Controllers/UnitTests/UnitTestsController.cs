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
        private readonly OpenAIClient _openAIClient = openAIClient  ?? throw new ArgumentNullException(nameof(openAIClient), "Issue with injecting OpenAIClient");

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
                configuration["AppSettings:Model"] ?? throw new ArgumentNullException(nameof(configuration), "Model is not configured"),
                Prompts.GenerateUnitTests,
                request.Code);

            try
            {
                var response = await _openAIClient.CreateChatCompletion(openAiRequest);

                var choice = response?.Choices?.FirstOrDefault();

                return choice?.FinishReason switch
                {
                    FinishReason.Stop when choice.Message != null && choice.Message.Content != null
                    => Content(choice.Message.Content),

                    FinishReason.Stop when choice.Message == null || choice.Message.Content == null
                    => StatusCode(502, "Assistant returned empty message"),

                    FinishReason.Length => BadRequest("Request size is reached"),

                    FinishReason.ContentFilter => BadRequest("Request was filtered by ContentFilters"),
                    _ => StatusCode(502, "Something went wrong")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(502, ex.Message);
            }
        }
    }
}