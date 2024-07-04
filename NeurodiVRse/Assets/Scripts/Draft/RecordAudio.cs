using UnityEngine;
using System.Collections;

public class RecordAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public bool isTalking = false;


    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void StartRecording()
    {
        isTalking = true;
        audioSource.clip = Microphone.Start(null, false, 10, 44100);
    }

    public void StopRecording()
    {
        Microphone.End(null);
        isTalking = false;
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
