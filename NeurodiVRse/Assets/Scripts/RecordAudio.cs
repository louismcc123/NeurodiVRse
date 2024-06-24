using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class RecordAudio : MonoBehaviour
{
    private const string apiKey = "YOUR_GOOGLE_CLOUD_API_KEY";
    private const string apiUrl = "https://speech.googleapis.com/v1/speech:recognize?key=" + apiKey;

    private AudioSource audioSource;
    private AudioClip recordedClip;
    private bool isRecording = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            recordedClip = Microphone.Start(null, false, 10, 44100);
        }
    }

    public IEnumerator StopRecording(System.Action<string> callback)
    {
        if (isRecording)
        {
            isRecording = false;
            Microphone.End(null);
            yield return StartCoroutine(ProcessAudio(callback));
        }
    }

    private IEnumerator ProcessAudio(System.Action<string> callback)
    {
        var samples = new float[recordedClip.samples * recordedClip.channels];
        recordedClip.GetData(samples, 0);
        byte[] audioData = WavUtility.FromAudioClip(recordedClip);

        var requestJson = new
        {
            config = new
            {
                encoding = "LINEAR16",
                sampleRateHertz = 44100,
                languageCode = "en-US"
            },
            audio = new
            {
                content = System.Convert.ToBase64String(audioData)
            }
        };

        string jsonData = JsonUtility.ToJson(requestJson);

        var request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            // Handle response and extract text
            // Assuming the extracted text is stored in a variable named 'extractedText'
            callback(extractedText);
        }
    }
}
