﻿using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assistant_API.Services
{
    public class RunService
    {
        private readonly string _apiEndpoint;
        private readonly string _apiKey;

        // 構造函數，從配置文件中讀取 API 端點和 API 密鑰
        public RunService()
        {
            // 從配置文件中讀取 API 端點
            _apiEndpoint = ConfigurationManager.AppSettings["APIendpoint"];
            // 從配置文件中讀取 API 密鑰
            _apiKey = ConfigurationManager.AppSettings["APIkey"];
        }

        // 建立執行
        public async Task<string> CreateRunAsync(string threadId, string assistantId)
        {
            // 創建 HttpClient 實例，用於發送 HTTP 請求
            var client = new HttpClient();

            // 創建 HttpRequestMessage 實例，設置請求方法為 POST，並組合 URL
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiEndpoint}/openai/threads/{threadId}/runs?api-version=2024-05-01-preview");

            // 添加 API 密鑰到請求頭，以便進行身份驗證
            request.Headers.Add("api-key", _apiKey);

            // 構建 JSON 請求內容
            var runContent = new
            {
                // 助理的 ID
                assistant_id = assistantId
            };

            // 將執行內容序列化為 JSON 字符串
            var jsonContent = JsonConvert.SerializeObject(runContent);

            // 設置請求的內容，並指定內容類型為 application/json
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // 發送 HTTP 請求並等待響應
            var response = await client.SendAsync(request);

            // 確保請求成功，否則拋出異常
            response.EnsureSuccessStatusCode();

            // 讀取並返回響應內容，這裡返回的是 JSON 字符串
            return await response.Content.ReadAsStringAsync();
        }
    }
}
