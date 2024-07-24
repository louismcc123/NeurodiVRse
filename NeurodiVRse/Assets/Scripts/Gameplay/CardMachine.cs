using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMachine : MonoBehaviour
{
    public DialogueManager dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCard"))
        {
            Debug.Log("Card tapped on the machine");
            dialogueManager.OnCardTapped();
        }
    }
}
