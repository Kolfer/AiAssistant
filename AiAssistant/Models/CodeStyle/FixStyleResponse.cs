namespace AiAssistant.Models.CodeStyle
{
    public class FixStyleResponse(string response)
    {
        public string Response { get; set; } = response;
    }
}
