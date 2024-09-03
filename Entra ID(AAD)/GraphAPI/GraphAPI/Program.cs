using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    // 從 app.config 中讀取 Azure AD 目錄識別碼
    private static readonly string? tenantId = ConfigurationManager.AppSettings["TenantId"];
    // 從 app.config 中讀取應用程式識別碼
    private static readonly string? clientId = ConfigurationManager.AppSettings["ClientId"];
    // 從 app.config 中讀取用戶端密碼
    private static readonly string? clientSecret = ConfigurationManager.AppSettings["ClientSecret"];


    static async Task Main(string[] args)
    {
        // 獲取訪問令牌
        var token = await GetAccessToken();
        // 使用訪問令牌獲取使用者資料
        var usersJson = await GetUsers(token);
        // 將使用者資料轉換為 DataTable
        var usersTable = ConvertJsonToDataTable(usersJson);
        // 列印 DataTable 的內容
        PrintDataTable(usersTable);

        // 防止程式立即結束，等待使用者按下任意鍵
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    // 獲取訪問令牌的方法
    private static async Task<string> GetAccessToken()
    {
        using (var client = new HttpClient())
        {
            // 設定 OAuth 2.0 端點 URL
            var url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            // 設定請求內容
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            // 發送 POST 請求以獲取訪問令牌
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            // 解析回應內容
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(resultString);

            // 返回訪問令牌
            return result["access_token"]?.ToString() ?? throw new Exception("Access token not found");
        }
    }

    // 使用訪問令牌獲取使用者資料的方法
    private static async Task<string> GetUsers(string token)
    {
        using (var client = new HttpClient())
        {
            // 設定 Microsoft Graph API 端點 URL
            var url = "https://graph.microsoft.com/v1.0/users?$top=999&$filter=accountEnabled eq true";

            // 設定請求標頭
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 發送 GET 請求以獲取使用者資料
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // 返回回應內容
            return await response.Content.ReadAsStringAsync();
        }
    }

    // 將 JSON 字串轉換為 DataTable 的方法
    private static DataTable ConvertJsonToDataTable(string json)
    {
        var dataTable = new DataTable();
        var jsonObject = JObject.Parse(json);
        var users = jsonObject["value"];

        // 檢查是否有使用者資料
        if (users == null || !users.HasValues)
        {
            throw new Exception("No users found in the response");
        }

        // 添加欄位
        foreach (var column in users[0].Children<JProperty>())
        {
            dataTable.Columns.Add(column.Name);
        }

        // 添加行
        foreach (var user in users)
        {
            var row = dataTable.NewRow();
            foreach (var column in user.Children<JProperty>())
            {
                row[column.Name] = column.Value?.ToString() ?? string.Empty;
            }
            dataTable.Rows.Add(row);
        }

        return dataTable;
    }

    // 列印 DataTable 的內容的方法
    private static void PrintDataTable(DataTable table)
    {
        // 列印欄位名稱
        foreach (DataColumn column in table.Columns)
        {
            Console.Write($"{column.ColumnName}\t");
        }
        Console.WriteLine();

        // 列印每一行的資料
        foreach (DataRow row in table.Rows)
        {
            foreach (var item in row.ItemArray)
            {
                Console.Write($"{item}\t");
            }
            Console.WriteLine();
        }
    }
}
