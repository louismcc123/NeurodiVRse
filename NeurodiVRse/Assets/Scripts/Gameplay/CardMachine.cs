using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMachine : MonoBehaviour
{
    public CardManager cardManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            Debug.Log("Card tapped on the machine");
            cardManager.OnCardTapped(); // Resume dialogue and destroy card
        }
    }
}
