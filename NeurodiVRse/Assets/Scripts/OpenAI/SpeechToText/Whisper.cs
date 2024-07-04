using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System;
using OpenAI;

public class Whisper : MonoBehaviour
{
    public Button recordButton;
    public Text responseText;
    public OpenAIManager openAIManager;
    private AudioClip recording;
    private string filePath;

    private void Start()
    {
        recordButton.onClick.AddListener(ToggleRecording);
    }

    private void ToggleRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            SaveRecording();
        }
        else
        {
            recording = Microphone.Start(null, false, 10, 44100);
        }
    }

    private void SaveRecording()
    {
        var fileName = "recording.wav";
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        SaveWav.Save(fileName, recording);
        StartCoroutine(UploadRecording());
    }

    private IEnumerator UploadRecording()
    {
        var config = new Configuration();
        var www = UnityWebRequest.PostWwwForm("https://api.openai.com/v1/audio/transcriptions", "");
        www.SetRequestHeader("Authorization", "Bearer " + config.ApiKey);
        www.uploadHandler = new UploadHandlerFile(filePath);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            var response = JsonUtility.FromJson<WhisperResponse>(www.downloadHandler.text);
            responseText.text = response.text;
            openAIManager.AskChatGPT(response.text);
        }
    }

    [Serializable]
    public class WhisperResponse
    {
        public string text;
    }
}
