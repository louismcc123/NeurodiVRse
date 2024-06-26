/*using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour
{
    private const string ttsApiUrl = "https://api.openai.com/v1/audio/speech";

    public IEnumerator ConvertTextToSpeech(string text, System.Action<AudioClip> callback)
    {
        string jsonData = JsonUtility.ToJson(new
        {
            input = text,
            model = "tts-1",
            voice = "alloy"
        });

        using (UnityWebRequest request = new UnityWebRequest(ttsApiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + APIManager.GetTTSApiKey());

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                byte[] audioData = request.downloadHandler.data;
                AudioClip audioClip = WavUtility.ToAudioClip(audioData);
                callback(audioClip);
            }
        }
    }
}
*/