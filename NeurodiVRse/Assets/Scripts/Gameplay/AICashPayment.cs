using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class AICashPayment : MonoBehaviour
{
    [SerializeField] private AICashManager cashManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cash"))
        {
            Debug.Log("Cash handed to AI.");
            cashManager.OnCashHandedOver();
        }
    }
}
