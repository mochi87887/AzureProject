using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;


// 官方文件: https://learn.microsoft.com/zh-tw/azure/ai-services/openai/chatgpt-quickstart?tabs=command-line%2Cpython-new&pivots=programming-language-csharp
// 參數的網址參考: https://platform.openai.com/docs/guides/images/usage?context=node

namespace Dalle3
{
    // 需要安裝 NuGet 套件: Azure.AI.OpenAI (專案使用 1.0.0-beta.17 版本)
    public partial class GenerateImages
    {
        public static async Task Main(string[] args)
        {
            // 設定 Azure OpenAI 服務的 endpoint 和 key (明碼的寫法僅供測試使用，實際應用請使用安全的方式存取金鑰)
            string endpoint = "AOAI 的 端點(endpoint)";
            string key = "AOAI 的 金鑰(key)";

            OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

            for (int i = 0; i < 3; i++)
            {
                Response<ImageGenerations> imageGenerations = await client.GetImageGenerationsAsync(
                    new ImageGenerationOptions()
                    {
                        // 下方程式碼需修改 DeploymentName 和 Prompt 內容，其餘參數可依需求調整
                        DeploymentName = "對應 AOAI 的模型「部署名稱」",
                        Prompt = "設定圖片的敘述，例如:在街道上盛開的櫻花樹",
                        Size = ImageSize.Size1792x1024,     //1024x1024, 1024x1792 or 1792x1024
                        Quality = "hd",                     // standard or hd
                        Style = "vivid"                     // natural or vivid
                    });

                // 從 ImageGenerations 回應中取得圖片的 URL
                Uri imageUri = imageGenerations.Value.Data[0].Url;

                // 將圖片的 URI 輸出:
                //Console.WriteLine(imageUri);

                // 開啟圖片的 URL
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = imageUri.ToString(),
                    UseShellExecute = true
                });
            }

            Console.ReadKey();
        }
    }
}
