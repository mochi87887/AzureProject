using Azure;
using Azure.AI.OpenAI.Assistants;
using Azure.Core;
//using Azure.Core.Credential;
using System;
using System.Threading.Tasks;

public class AssistantSetup
{
    public static async Task<Assistant> CreateAssistantAsync(AssistantsClient client)
    {
        // 建立助理
        AssistantCreationOptions assistantCreationOptions = new AssistantCreationOptions("GPT-4o")
        {
            Name = "遊戲工程師",
            Instructions = "你是一名遊戲開發工程師，回答會使用繁體中文說明。",
            Tools = { },
        };

        Response<Assistant> assistantResponse = await client.CreateAssistantAsync(assistantCreationOptions);
        return assistantResponse.Value;
    }
}
