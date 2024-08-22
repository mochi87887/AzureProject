using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;

namespace Assistant_API.Services
{
    public class AssistantService
    {
        private readonly string _apiEndpoint;
        private readonly string _apiKey;

        // 構造函數，從 app.config 中讀取 API 端點和 API 密鑰
        public AssistantService()
        {
            _apiEndpoint = ConfigurationManager.AppSettings["APIendpoint"];
            _apiKey = ConfigurationManager.AppSettings["APIkey"];
        }

        // 列出助理的方法
        public async Task<string> ListAssistantsAsync()
        {
            // 創建 HttpClient 實例
            var client = new HttpClient();

            // 創建 HttpRequestMessage，設置請求方法為 GET，並組合 URL
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiEndpoint}/assistants?api-version=2024-05-01-preview");

            // 添加 API 密鑰到請求頭
            request.Headers.Add("api-key", _apiKey);

            // 創建空的 StringContent，設置內容類型為 application/json
            var content = new StringContent("", null, "application/json");
            request.Content = content;

            // 發送請求並獲取響應
            var response = await client.SendAsync(request);

            // 確保請求成功，否則拋出異常
            response.EnsureSuccessStatusCode();

            // 讀取並返回響應內容
            return await response.Content.ReadAsStringAsync();
        }
    }
}