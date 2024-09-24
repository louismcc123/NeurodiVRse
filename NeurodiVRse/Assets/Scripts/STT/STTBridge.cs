using Meta.WitAi.Json;
using Meta.WitAi;
using Oculus.Voice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class STTBridge : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InputField inputField;
    [SerializeField] private GameObject speakButton;
    [SerializeField] private GameObject stopButton;
    [SerializeField] private GameObject sendButton;

    [Header("Voice")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;

    public bool IsActive => _active;
    private bool _active = false;
    private Coroutine _deactivationCoroutine;

    private void OnEnable()
    {
        appVoiceExperience.VoiceEvents.OnRequestCreated.AddListener(OnRequestStarted);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnStartListening.AddListener(OnListenStart);
        appVoiceExperience.VoiceEvents.OnStoppedListening.AddListener(OnListenStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToDeactivation.AddListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToInactivity.AddListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnResponse.AddListener(OnRequestResponse);
        appVoiceExperience.VoiceEvents.OnError.AddListener(OnRequestError);
    }

    private void OnDisable()
    {
        appVoiceExperience.VoiceEvents.OnRequestCreated.RemoveListener(OnRequestStarted);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnStartListening.RemoveListener(OnListenStart);
        appVoiceExperience.VoiceEvents.OnStoppedListening.RemoveListener(OnListenStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToDeactivation.RemoveListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToInactivity.RemoveListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnResponse.RemoveListener(OnRequestResponse);
        appVoiceExperience.VoiceEvents.OnError.RemoveListener(OnRequestError);
    }

    private void OnRequestStarted(WitRequest r)
    {
        _active = true;
    }

    private void OnRequestTranscript(string transcript)
    {
        inputField.text = transcript;
    }

    private void OnListenStart()
    {
        //inputField.text = "Listening...";
    }

    private void OnListenStop()
    {
        //inputField.text = "Processing...";
    }

    private void OnListenForcedStop()
    {
        OnRequestComplete();
    }

    private void OnRequestResponse(WitResponseNode response)
    {
        if (!string.IsNullOrEmpty(response["text"]))
        {
            inputField.text = response["text"];
        }

        OnRequestComplete();
    }

    private void OnRequestError(string error, string message)
    {
        // Optionally handle errors
        OnRequestComplete();
    }

    private void OnRequestComplete()
    {
        _active = false;
    }

    public void OnSpeakButtonClicked()
    {
        SetActivation(true);
    }

    public void OnStopSpeakingButtonClicked()
    {
        SetActivation(false);
    }

    private void SetActivation(bool toActivated)
    {
        if (_active != toActivated)
        {
            _active = toActivated;
            if (_active)
            {
                appVoiceExperience.Activate();
                StartDeactivationTimer();
            }
            else
            {
                appVoiceExperience.Deactivate();
                if (_deactivationCoroutine != null)
                {
                    StopCoroutine(_deactivationCoroutine);
                    _deactivationCoroutine = null;
                }
            }
        }
            UpdateButtonStates();


    }

    private void StartDeactivationTimer()
    {
        if (_deactivationCoroutine != null)
        {
            StopCoroutine(_deactivationCoroutine);
        }
        _deactivationCoroutine = StartCoroutine(DeactivationTimer());
    }

    private IEnumerator DeactivationTimer()
    {
        yield return new WaitForSeconds(10f);
        SetActivation(false);
    }

    private void UpdateButtonStates()
    {
        if (_active)
        {
            speakButton.SetActive(false);
            sendButton.SetActive(false);
            stopButton.SetActive(true);
        }
        else
        {
            speakButton.SetActive(true);
            sendButton.SetActive(true);
            stopButton.SetActive(false);
        }
    }
}
