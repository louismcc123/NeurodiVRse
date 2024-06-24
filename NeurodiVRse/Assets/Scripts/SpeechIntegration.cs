using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechIntegration : MonoBehaviour
{
    public RecordAudio recordAudio;
    public WhisperIntegration whisperIntegration;
    public OpenAIManager openAIManager;
    public TextToSpeech textToSpeech;

    public void StartConversation()
    {
        StartCoroutine(HandleConversation());
    }

    private IEnumerator HandleConversation()
    {
        recordAudio.StartRecording();
        yield return new WaitForSeconds(5); // Record for 5 seconds
        StartCoroutine(recordAudio.StopRecording((audioFilePath) =>
        {
            yield return whisperIntegration.TranscribeAudio(audioFilePath, (transcription) =>
            {
                StartCoroutine(openAIManager.GetOpenAIResponse(transcription, (response) =>
                {
                    StartCoroutine(textToSpeech.ConvertTextToSpeech(response, (audioClip) =>
                    {
                        // Play the audio response
                        AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
                    }));
                }));
            });
        }));
    }
}
