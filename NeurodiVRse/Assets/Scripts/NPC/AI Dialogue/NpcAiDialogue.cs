using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAiDialogue : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject NPCSpeechCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openAICanvas.SetActive(true);
            NPCSpeechCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openAICanvas.SetActive(false);
            NPCSpeechCanvas.SetActive(false);
        }
    }
}