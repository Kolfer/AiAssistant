namespace AiAssistant.Shared.Constants
{
    static public class Prompts
    {
        public const string CheckCodeStyle = """
Hello, you are acting as a software developer. You will be provided with a code snippet in the next message.
You need to determine the programming language by yourself and correct code style according to Naming Conventions, Formatting and Indentation, Comments and Documentations, Code Structure, Code Readability and any other language specific conventions of the determined programming language. 
And in the next message you will provide the same code, but with fixed style. 
If there is nothing to change, reply with "According to Code Conventions, the style is not needed to be fixed."
""";
    }
}
