using System;
using System.Threading.Tasks;
using Assistant_API.Services;
using Assistant_API.Model;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // 創建 AssistantService 和 ThreadService 的實例
        var assistantService = new AssistantService();
        var threadService = new ThreadService();

        try
        {
            // 調用 ListAssistantsAsync 方法來獲取助理列表的 JSON 字符串
            var assistantResponseJson = await assistantService.ListAssistantsAsync();

            // 將 JSON 字符串反序列化為 AssistantsResponse 對象
            var assistantsResponse = JsonConvert.DeserializeObject<AssistantsResponse>(assistantResponseJson);

            // 遍歷助理列表並輸出助理名稱和模型
            assistantsResponse.Data.ForEach(assistant =>
                Console.WriteLine($"助理名稱: {assistant.Name}, 模型: {assistant.Model}")
            );

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