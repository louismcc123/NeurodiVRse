using UnityEngine;

public class CardMachine : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public CardManager cardManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            Debug.Log("Card tapped on the machine");
            dialogueManager.OnCardTapped();

            if (cardManager != null)
            {
                cardManager.OnCardTapped();
            }
        }
    }
}
