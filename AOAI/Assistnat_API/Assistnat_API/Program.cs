using System;
using System.Threading.Tasks;
using Assistant_API.Services;
using Assistant_API.Model;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // 創建 AssistantService、ThreadService、MessageService 和 RunService 的實例
        var assistantService = new AssistantService();
        var threadService = new ThreadService();
        var messageService = new MessageService();
        var runService = new RunService();

        try
        {
            // 調用 ListAssistantsAsync 方法來獲取助理列表的 JSON 字符串
            var assistantResponseJson = await assistantService.ListAssistantsAsync();

            // 將 JSON 字符串反序列化為 AssistantsResponse 對象
            var assistantsResponse = JsonConvert.DeserializeObject<AssistantsResponse>(assistantResponseJson);

            // 遍歷助理列表並輸出助理名稱和模型
            for (int i = 0; i < assistantsResponse.Data.Count; i++)
            {
                var assistant = assistantsResponse.Data[i];
                Console.WriteLine($"{i + 1}. 助理名稱: {assistant.Name}, 模型: {assistant.Model}");
            }

            // 讓使用者選擇一個助理的編號
            Console.WriteLine("請輸入要使用的助理編號:");
            int assistantIndex;
            while (!int.TryParse(Console.ReadLine(), out assistantIndex) || assistantIndex < 1 || assistantIndex > assistantsResponse.Data.Count)
            {
                Console.WriteLine("無效的編號，請重新輸入:");
            }

            // 根據使用者選擇的編號獲取助理的 ID
            string assistantId = assistantsResponse.Data[assistantIndex - 1].Id;

            // 初始化執行緒 ID
            string threadId = string.Empty;

            // 調用 CreateThreadAsync 方法來創建一個新的執行緒，並獲取其 JSON 字符串
            var threadResponseJson = await threadService.CreateThreadAsync();

            // 將 JSON 字符串反序列化為 Thread_Model 對象
            var threadResponse = JsonConvert.DeserializeObject<Thread_Model>(threadResponseJson);

            // 獲取執行緒 ID
            threadId = threadResponse.Id;

            // 輸出執行緒 ID 和創建時間
            Console.WriteLine($"執行緒 ID: {threadResponse.Id}, 創建時間: {threadResponse.CreatedAt}");

            // 如果執行緒 ID 不為空，則調用 GetThreadAsync 方法來擷取執行緒的詳細信息
            if (!string.IsNullOrEmpty(threadId))
            {
                // 調用 GetThreadAsync 方法來獲取執行緒的 JSON 字符串
                var getThreadResponseJson = await threadService.GetThreadAsync(threadId);

                // 將 JSON 字符串反序列化為 Thread_Model 對象
                var getThreadResponse = JsonConvert.DeserializeObject<Thread_Model>(getThreadResponseJson);

                // 輸出擷取的執行緒 ID 和創建時間
                Console.WriteLine($"擷取的執行緒 ID: {getThreadResponse.Id}, 創建時間: {getThreadResponse.CreatedAt}");

                // 從控制台讀取訊息內容
                Console.WriteLine("請輸入訊息內容:");
                string messageContent = Console.ReadLine();

                // 發送訊息
                var sendMessageResponseJson = await messageService.CreateMessageAsync(threadId, "user", messageContent);

                // 將 JSON 字符串反序列化為 Message_Model 對象
                var sendMessageResponse = JsonConvert.DeserializeObject<Message_Model>(sendMessageResponseJson);

                // 輸出訊息 ID 和內容
                Console.WriteLine($"訊息 ID: {sendMessageResponse.Id}, 內容: {sendMessageResponse.Content[0].Text.Value}");

                // 輸出選擇的助理 ID
                Console.WriteLine($"選擇的助理 ID: {assistantId}");

                // 調用 CreateRunAsync 方法來建立執行
                var runResponseJson = await runService.CreateRunAsync(threadId, assistantId);

                // 將 JSON 字符串反序列化為 Run_Model 對象
                var runResponse = JsonConvert.DeserializeObject<Run_Model>(runResponseJson);

                // 輸出執行結果
                Console.WriteLine($"執行 ID: {runResponse.Id}, 狀態: {runResponse.Status}");
            }
        }
        catch (Exception ex)
        {
            // 捕獲並輸出異常信息
            Console.WriteLine($"發生錯誤: {ex.Message}");
        }

        // 等待用戶按下任意鍵後結束程序
        Console.ReadKey();
    }
}