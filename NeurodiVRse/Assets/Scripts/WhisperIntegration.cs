using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class WhisperIntegration : MonoBehaviour
{
    private const string whisperApiKey = "your-whisper-api-key";
    private const string whisperApiUrl = "https://api.openai.com/v1/audio/transcriptions";

    public IEnumerator TranscribeAudio(string audioFilePath, System.Action<string> callback)
    {
        byte[] audioData = File.ReadAllBytes(audioFilePath);
        string fileName = Path.GetFileName(audioFilePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, fileName, "audio/wav");

        using (UnityWebRequest request = UnityWebRequest.Post(whisperApiUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + whisperApiKey);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                var response = JsonUtility.FromJson<WhisperResponse>(jsonResponse);
                callback(response.text);
            }
        }
    }

    [System.Serializable]
    public class WhisperResponse
    {
        public string text;
    }
}
