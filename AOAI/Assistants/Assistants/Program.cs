using Azure;
using Azure.AI.OpenAI.Assistants;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // 建立助理
        Assistant assistant = await AssistantSetup.CreateAssistantAsync();
        if (assistant == null)
        {
            return;
        }

        // 從 app.config 中讀取端點和金鑰
        string? endpoint = ConfigurationManager.AppSettings["AZURE_OPENAI_ENDPOINT"];
        string? key = ConfigurationManager.AppSettings["AZURE_OPENAI_API_KEY"];

        // 使用端點和金鑰建立 AssistantsClient
        AssistantsClient client = new AssistantsClient(new Uri(endpoint), new AzureKeyCredential(key));

        // 建立一個新的執行緒
        Response<AssistantThread> threadResponse = await client.CreateThreadAsync();
        AssistantThread thread = threadResponse.Value;

        // 讓使用者輸入問題
        Console.WriteLine("請輸入您的問題：");
        string? userInput = Console.ReadLine();

        // 將使用者的問題添加到執行緒中
        Response<ThreadMessage> messageResponse = await client.CreateMessageAsync(
            thread.Id,
            MessageRole.User,
            userInput
        );
        ThreadMessage message = messageResponse.Value;

        // 執行該執行緒
        Response<ThreadRun> runResponse = await client.CreateRunAsync(
            thread.Id,
            new CreateRunOptions(assistant.Id)
        );
        ThreadRun run = runResponse.Value;

        // 等待助理回應，直到執行狀態不再是 Queued 或 InProgress
        do
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500)); // 每隔 500 毫秒檢查一次狀態
            runResponse = await client.GetRunAsync(thread.Id, run.Id);
        } while (runResponse.Value.Status == RunStatus.Queued || runResponse.Value.Status == RunStatus.InProgress);

        // 一旦執行完成，獲取執行緒中的所有消息
        if (runResponse.Value.Status == RunStatus.Completed)
        {
            Response<PageableList<ThreadMessage>> afterRunMessagesResponse = await client.GetMessagesAsync(thread.Id);
            IReadOnlyList<ThreadMessage> messages = afterRunMessagesResponse.Value.Data;

            // 注意：消息從最新到最舊迭代，messages[0] 是最新的
            foreach (ThreadMessage threadMessage in messages.Reverse())
            {
                Console.Write($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role,10}: ");
                foreach (MessageContent contentItem in threadMessage.ContentItems)
                {
                    if (contentItem is MessageTextContent textItem)
                    {
                        Console.Write(textItem.Text); // 輸出文本內容
                    }
                    else if (contentItem is MessageImageFileContent imageFileItem)
                    {
                        Console.Write($"<image from ID: {imageFileItem.FileId}>"); // 輸出圖片文件 ID
                    }
                    Console.WriteLine();
                }
            }

            Console.ReadKey();
        }
        else
        {
            // 如果執行狀態不是 Completed，輸出錯誤信息
            Console.WriteLine($"執行狀態為 {runResponse.Value.Status}，無法獲取消息。");
            Console.ReadKey();
        }
    }
}
