using Azure;
using Azure.AI.OpenAI.Assistants;
using Azure.Core;
using System;
using System.Configuration;
using System.Threading.Tasks;

class AssistantSetup
{
    public static async Task<Assistant> CreateAssistantAsync()
    {
        // 從 app.config 中讀取端點和金鑰
        string? endpoint = ConfigurationManager.AppSettings["AZURE_OPENAI_ENDPOINT"];
        string? key = ConfigurationManager.AppSettings["AZURE_OPENAI_API_KEY"];

        // 從 app.config 中讀取助理名稱和助理說明
        string? name = ConfigurationManager.AppSettings["ASSISTANT_NAME"];
        string? instructions = ConfigurationManager.AppSettings["ASSISTANT_INSTRUCTIONS"];

        string? model = ConfigurationManager.AppSettings["AZURE_OPENAI_MODEL"];

        if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
        {
            Console.WriteLine("Please set the appSettings AZURE_OPENAI_ENDPOINT and AZURE_OPENAI_API_KEY in app.config.");
            return null;
        }

        // 使用端點和金鑰建立 AssistantsClient
        AssistantsClient client = new AssistantsClient(new Uri(endpoint), new AzureKeyCredential(key));

        // 建立助理
        AssistantCreationOptions assistantCreationOptions = new AssistantCreationOptions(model)
        {
            Name = name,
            Instructions = instructions,
            Tools = { },
        };

        Response<Assistant> assistantResponse = await client.CreateAssistantAsync(assistantCreationOptions);
        return assistantResponse.Value;
    }
}
