using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payment : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public void ProcessPayment(string paymentMethod)
    {
        // Implement the logic to process the payment
        if (paymentMethod == "Card, please.")
        {
            // Handle card payment
            Debug.Log("Processing card payment...");
        }
        else if (paymentMethod == "Cash, please.")
        {
            // Handle cash payment
            Debug.Log("Processing cash payment...");
        }

        // Simulate payment completion
        StartCoroutine(CompletePayment());
    }

    private IEnumerator CompletePayment()
    {
        // Simulate a delay for payment processing
        yield return new WaitForSeconds(3.0f);

        // Resume the dialogue
        dialogueManager.ResumeDialogue();
    }
}
