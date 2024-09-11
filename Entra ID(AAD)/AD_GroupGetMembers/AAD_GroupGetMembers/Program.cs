using System; // 引入 System 命名空間
using System.Configuration; // 引入 System.Configuration 命名空間，用於讀取配置
using System.Threading.Tasks; // 引入 System.Threading.Tasks 命名空間，用於異步操作
using Azure.Identity; // 引入 Azure.Identity 命名空間，用於 Azure 身份驗證
using Microsoft.Graph; // 引入 Microsoft.Graph 命名空間，用於 Microsoft Graph API
using Microsoft.Graph.Models; // 引入 Microsoft.Graph.Models 命名空間，用於 Graph API 模型

class Program
{
    // 定義一個靜態的 GraphServiceClient 變數，用於與 Microsoft Graph API 交互
    private static GraphServiceClient? graphClient;

    // 主程序入口點，使用異步方法
    static async Task Main(string[] args)
    {
        // 從 app.config 中讀取群組名稱，如果未配置則拋出異常
        string groupName = ConfigurationManager.AppSettings["GroupName"] ?? throw new InvalidOperationException("GroupName is not configured.");

        try
        {
            // 初始化 Graph 客戶端
            InitializeGraphClient();

            // 根據群組名稱查找群組
            var group = await FindGroupByNameAsync(groupName);
            if (group != null && group.Id != null)
            {
                // 如果找到群組，顯示群組名稱
                Console.WriteLine($"群組 '{groupName}' 的成員有：");
                // 顯示群組成員
                await DisplayGroupMembersAsync(group.Id);
            }
            else
            {
                // 如果找不到群組，顯示錯誤訊息
                Console.WriteLine($"找不到群組 '{groupName}'");
            }
        }
        catch (Exception ex)
        {
            // 捕捉並顯示例外錯誤訊息
            Console.WriteLine($"發生錯誤: {ex.Message}");
        }

        // 等待使用者按下任意鍵後結束程式
        Console.ReadKey();
    }

    // 初始化 Graph 客戶端的方法
    private static void InitializeGraphClient()
    {
        // 從 app.config 中讀取設定，如果未配置則拋出異常
        var clientId = ConfigurationManager.AppSettings["ClientId"] ?? throw new InvalidOperationException("ClientId is not configured.");
        var tenantId = ConfigurationManager.AppSettings["TenantId"] ?? throw new InvalidOperationException("TenantId is not configured.");
        var clientSecret = ConfigurationManager.AppSettings["ClientSecret"] ?? throw new InvalidOperationException("ClientSecret is not configured.");

        // 使用客戶端憑證初始化 Graph 客戶端
        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        graphClient = new GraphServiceClient(clientSecretCredential);
    }

    // 根據群組名稱查找群組的異步方法
    private static async Task<Group?> FindGroupByNameAsync(string groupName)
    {
        // 如果 Graph 客戶端未初始化，則拋出異常
        if (graphClient == null)
        {
            throw new InvalidOperationException("Graph client is not initialized.");
        }

        // 使用 Graph API 查找群組
        var groups = await graphClient.Groups
            .GetAsync(requestConfiguration =>
            {
                // 設置查詢參數過濾條件
                requestConfiguration.QueryParameters.Filter = $"displayName eq '{groupName}'";
            });

        // 返回找到的群組，如果有的話
        return groups?.Value?.Count > 0 ? groups.Value[0] : null;
    }

    // 顯示群組成員的異步方法
    private static async Task DisplayGroupMembersAsync(string groupId)
    {
        // 如果 Graph 客戶端未初始化，則拋出異常
        if (graphClient == null)
        {
            throw new InvalidOperationException("Graph client is not initialized.");
        }

        // 使用 Graph API 獲取群組成員
        var members = await graphClient.Groups[groupId].Members.GetAsync();

        // 如果成員列表不為空
        if (members?.Value != null)
        {
            // 遍歷每個成員
            foreach (var member in members.Value)
            {
                // 如果成員是使用者，顯示使用者的顯示名稱
                if (member is User user)
                {
                    Console.WriteLine($"名稱: {user.DisplayName}, 電子郵件: {user.Mail}");
                }
                // 如果成員是子群組，顯示子群組名稱並遞迴顯示其成員
                else if (member is Group subGroup && subGroup.Id != null)
                {
                    Console.WriteLine($"子群組 '{subGroup.DisplayName}' 的成員有：");
                    await DisplayGroupMembersAsync(subGroup.Id);
                }
            }
        }
    }
}
