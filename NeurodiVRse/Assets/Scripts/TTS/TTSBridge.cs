using Meta.WitAi.TTS.Utilities;
using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTSBridge : MonoBehaviour
{
    [SerializeField] private TTSSpeaker ttsSpeaker;
    [SerializeField] private AdviceManager adviceManager;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Button sendButton;
    [SerializeField] private Button recordButton;
    [SerializeField] private Button interruptButton;

    void Start()
    {
        ChatGPT.onChatGPTMessageReceived += Speak;
        interruptButton.onClick.AddListener(Interrupt);
    }

    private void OnDestroy()
    {
        ChatGPT.onChatGPTMessageReceived -= Speak;
    }

    public void SetVoicePreset(string presetVoiceID)
    {
        if (ttsSpeaker != null && !string.IsNullOrEmpty(presetVoiceID))
        {
            ttsSpeaker.presetVoiceID = presetVoiceID;
            //Debug.Log($"Voice preset changed to {presetVoiceID} for active NPC.");
        }
        else
        {
            Debug.LogWarning("TTS Speaker is null or presetVoiceID is invalid.");
        }
    }

    public void Speak(string message)
    {
        sendButton.gameObject.SetActive(false);
        recordButton.gameObject.SetActive(false);
        interruptButton.gameObject.SetActive(true);

        string[] messages = message.Split('.');
        ttsSpeaker.StartCoroutine(ttsSpeaker.SpeakQueuedAsync(messages));
        OnSpeechComplete();
    }

    private void OnSpeechComplete()
    {
        sendButton.gameObject.SetActive(true);
        recordButton.gameObject.SetActive(true);
        interruptButton.gameObject.SetActive(false);
    }

    private void Interrupt()
    {
        if (ttsSpeaker.IsSpeaking)
        {
            Debug.Log("Interrupting the speaker");
            ttsSpeaker.Stop();

            playerStats.SubtractScore(1f);
            adviceManager.DisplayAdvice("It is rude to interrupt someone when they are speaking.");

            sendButton.gameObject.SetActive(true);
            recordButton.gameObject.SetActive(true);
            interruptButton.gameObject.SetActive(false);
        }
    }
}