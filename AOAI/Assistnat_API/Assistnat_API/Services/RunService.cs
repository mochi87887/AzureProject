using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assistant_API.Services
{
    public class RunService
    {
        private readonly HttpClient _client;

        public RunService()
        {
            _client = new HttpClient();
        }

        public async Task<string> CreateRunAsync(string threadId, string assistantId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://codegpt-aoai.openai.azure.com/openai/threads/{threadId}/runs?api-version=2024-05-01-preview");
            request.Headers.Add("api-key", "fb636f9e7e0f448c8a8cc20d54bcc8f6");

            // 構建 JSON 請求內容
            var runContent = new
            {
                assistant_id = assistantId
            };
            var jsonContent = JsonConvert.SerializeObject(runContent);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
