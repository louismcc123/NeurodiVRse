using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class AICardMachine : MonoBehaviour
{
    [SerializeField] private AICardManager cardManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            Debug.Log("Card tapped on the machine.");
            cardManager.OnCardTapped();
        }
    }
}
