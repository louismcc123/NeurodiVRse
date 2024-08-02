using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashPayment : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public CashManager cashManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cash"))
        {
            Debug.Log("Cash handed to barista");
            cashManager.OnCashHandedOver();
        }
    }
}
