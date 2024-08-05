using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace KeyVault
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 【查看方式一】: Entra識別碼 > 租用戶識別碼
            // 【查看方式二】: Entra識別碼 > 應用程式註冊 > 應用程式 (用戶端) 識別碼
            var clientId = "78c8f999-******";

            // Entra識別碼 > 應用程式註冊 > 管理 > 憑證及祕密 > 值
            var clientSecret = "BYa8Q~******";

            // 【查看方式一】: Entra識別碼 > 概觀 > 租用戶識別碼
            // 【查看方式二】: Entra識別碼 > 應用程式註冊 > 目錄 (租用戶) 識別碼
            var tenantId = "d30c40cf-******";

            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", clientId);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", clientSecret);
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", tenantId);

            // Key Vault > 概觀 > 保管庫 URI
            var kvUri = "https://{Your KeyVault Name}.vault.azure.net/";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            // Key Vault > 物件 > 祕密 > 祕密名稱
            KeyVaultSecret secret = client.GetSecret("{Your Secret Name}");

            Console.WriteLine(secret.Value);
            Console.ReadKey();
        }
    }
}