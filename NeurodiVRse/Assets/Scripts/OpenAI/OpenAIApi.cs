using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

namespace LLM
{
    public class OpenAIApi
    {
        private string apiKey;
        private const string BASE_PATH = "https://api.openai.com/v1";

        public OpenAIApi(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<CreateChatCompletionResponse> CreateChatCompletion(CreateChatCompletionRequest request)
        {
            string path = $"{BASE_PATH}/chat/completions";
            string payload = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            });
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            using (var webRequest = new UnityWebRequest(path, "POST"))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(payloadBytes);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                await webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error: {webRequest.error}");
                    return null;
                }

                try
                {
                    return JsonConvert.DeserializeObject<CreateChatCompletionResponse>(webRequest.downloadHandler.text);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed to parse response: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
