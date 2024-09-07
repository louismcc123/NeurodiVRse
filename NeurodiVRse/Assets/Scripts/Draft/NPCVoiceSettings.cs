/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;

[System.Serializable]
public class NPCVoiceSettings
{
    public string voice = TTSWitVoiceSettings.DEFAULT_VOICE;
    public string style = TTSWitVoiceSettings.DEFAULT_STYLE;
    public int speed = 100;
    public int pitch = 100;
}

public class NPCVoiceManager : MonoBehaviour
{
    [SerializeField] private NPCVoiceSettings voiceSettings;

    public NPCVoiceSettings VoiceSettings => voiceSettings;

    // Method to apply voice settings to a TTSSpeaker
    public void ApplyVoiceSettings(TTSSpeaker ttsSpeaker)
    {
        if (ttsSpeaker != null)
        {
            TTSWitVoiceSettings settings = new TTSWitVoiceSettings
            {
                voice = voiceSettings.voice,
                style = voiceSettings.style,
                speed = voiceSettings.speed,
                pitch = voiceSettings.pitch
            };

            // Assuming TTSSpeaker has a method to apply settings
            // You may need to modify the TTSSpeaker component to support this
            ttsSpeaker.SetVoiceSettings(settings); // Modify TTSSpeaker to support this if necessary
        }
    }
}
*/