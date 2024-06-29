/*using System.Collections;
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
        yield return recordAudio.StopRecording((transcription) =>
        {
            StartCoroutine(ProcessTranscription(transcription));
        });
    }

    private IEnumerator ProcessTranscription(string transcription)
    {
        yield return whisperIntegration.TranscribeAudio(transcription, (response) =>
        {
            StartCoroutine(ProcessResponse(response));
        });
    }

    private IEnumerator ProcessResponse(string response)
    {
        yield return openAIManager.GetOpenAIResponse(response, (aiResponse) =>
        {
            StartCoroutine(GenerateSpeech(aiResponse));
        });
    }

    private IEnumerator GenerateSpeech(string aiResponse)
    {
        yield return textToSpeech.ConvertTextToSpeech(aiResponse, (audioClip) =>
        {
            AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
        });
    }
}
*/