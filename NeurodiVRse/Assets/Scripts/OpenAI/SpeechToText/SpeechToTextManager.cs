using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class SpeechToTextManager : MonoBehaviour
{
    public AudioManager audioManager;

    IEnumerator SendAudioToAPI()
    {
        string path = "path_to_your_audio_file";
        byte[] audioBytes = System.IO.File.ReadAllBytes(path);
        string encodedAudio = System.Convert.ToBase64String(audioBytes);

        string jsonData = JsonUtility.ToJson(new
        {
            model = "whisper-1",
            audio = encodedAudio
        });

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/audio/transcriptions", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer your_openai_api_key");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }
}
