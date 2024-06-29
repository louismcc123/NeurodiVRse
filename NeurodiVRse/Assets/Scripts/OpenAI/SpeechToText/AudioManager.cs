using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioClip recordedClip;

    public void StartRecording()
    {
        recordedClip = Microphone.Start(null, true, 10, 44100); // update this to know if player is interupting or talking too much
    }

    public void StopRecording()
    {
        Microphone.End(null);
    }
}
