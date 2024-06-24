using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAIManager : MonoBehaviour
{
    private const string apiKey = "YOUR_OPENAI_API_KEY";
    private const string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

    [System.Serializable]
    public class OpenAIResponse
    {
        public string id;
        public string obj;
        public long created;
        public string model;
        public Choice[] choices;
        public Usage usage;
    }

    [System.Serializable]
    public class Choice
    {
        public string text;
        public int index;
        public object logprobs;
        public string finish_reason;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

    public IEnumerator GetOpenAIResponse(string prompt, System.Action<string> callback)
    {
        string jsonData = JsonUtility.ToJson(new
        {
            prompt = prompt,
            max_tokens = 150
        });

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(jsonResponse);
                string result = response.choices[0].text.Trim();
                callback(result);
            }
        }
    }
}
