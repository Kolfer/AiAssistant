using AiAssistant.Models.CodeStyle;
using AiAssistant.Shared.Clients.OpenAIClient;
using AiAssistant.Shared.Clients.OpenAIClient.Models;
using AiAssistant.Shared.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AiAssistant.Controllers.CodeStyle
{
    [Route("api/v1/[controller]")]
    public class CodeStyleController(OpenAIClient openAIClient, IConfiguration configuration) : ControllerBase
    {
        private readonly OpenAIClient _openAIClient = openAIClient ?? throw new ArgumentNullException(nameof(openAIClient), "Issue with injecting OpenAIClient");

        /// <summary>
        /// Fixing style of a provided code snippet according to Code Conventions
        /// </summary>
        /// <param name="request">Request with a code snippet</param>
        /// <returns>Formatted code snippet</returns>
        [HttpPost("FixStyle")]
        [Produces("text/plain")]
        public async Task<ActionResult<FixStyleResponse>> FixStyleAsync(FixStyleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest("Code is not provided");
            }

            var openAiRequest = new CreateChatCompletionRequest(
                configuration["AppSettings:Model"] ?? throw new ArgumentNullException(nameof(configuration), "Model is not configured"),
                Prompts.FixStyle,
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
                    => BadRequest("Assistant returned empty message"),

                    FinishReason.Length => BadRequest("Request size is reached"),

                    FinishReason.ContentFilter => BadRequest("Request was filtered by ContentFilters"),
                    _ => BadRequest("Something went wrong")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(502);
            }
        }
    }
}
