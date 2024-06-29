using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private GameObject chatUI;
    [SerializeField] private GameObject npcSpeechBubble;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatUI.SetActive(true);
            npcSpeechBubble.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatUI.SetActive(false);
            npcSpeechBubble.SetActive(false);
        }
    }
}
