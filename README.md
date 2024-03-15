# AiAssistant

AiAssistant uses OpenAI client and provides 2 endpoints:
/api/v1/CodeStyle/FixStyle - for fixing the style of a code snippet according to Code Conventions
/api/v1/UnitTests/Generate - for creating unit tests for provided code snippet

## Getting Started

For this project you need to have a working API key for OpenAI API

### Installation and usage

1. Clone project from "https://github.com/Kolfer/AiAssistant.git".
2. Navigate to appsettings.json file in solution root.
3. Insert your OpenAI API key into AppSettings:ApiKey.
4. If you want, you can change OpenAI model in AppSettings:Model (By default the "gpt-3.5-turbo" is used).
5. Start the application.
6. You will be redirected to Swagger UI page, where you can choose an endpoint.
7. Press "Try it out" button.
8. Enter code snippet in "Code" field and press "Execute" button.
9. You will see the result in Responses below.
