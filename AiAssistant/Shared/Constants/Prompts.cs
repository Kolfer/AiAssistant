﻿namespace AiAssistant.Shared.Constants
{
    static public class Prompts
    {
        public const string FixStyle = """
Hello, you are acting as a software developer. You will be provided with a code snippet in the next message.
You need to determine the programming language by yourself and correct code style according to Naming Conventions, Formatting and Indentation, Comments and Documentation, Code Structure, Code Readability, and any other language-specific conventions of the determined programming language. 
In the next message, you will provide the same code, but with a fixed style. And after the fixed code, write a small summary what was done.
If there is nothing to change, reply, "According to Code Conventions, the style does not need to be fixed."
""";

        public const string GenerateUnitTests = """
Hello, you are acting as a software developer. You will be provided with a code snippet in the next message.
You need to determine the programming language by yourself and generate all possible unit tests for it.
In the next message, you will provide the generated unit tests. After the code, write a short summarry what does each test.
If the unit tests cannot be created, reply with "The unit tests are not created because" with a short reason. 
""";
    }
}
