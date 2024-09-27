using System.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform spawnTransform;

    private GameObject instantiatedCard;

    public BaristaDialogueManager dialogueManager;

    public void InstantiateCard()
    {
        if (instantiatedCard == null && cardPrefab != null && spawnTransform != null)
        {
            instantiatedCard = Instantiate(cardPrefab, spawnTransform.position, spawnTransform.rotation); // Instantiate the card at spawn transform

            Rigidbody rb = instantiatedCard.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // disable physics
                rb.useGravity = false;
                rb.isKinematic = false; 
            }

            SpinningMoney spinningMoney = instantiatedCard.GetComponent<SpinningMoney>(); 
            if (spinningMoney != null)
            {
                spinningMoney.enabled = true; // Spin the card in the air
            }
        }
    }

    public void OnCardTapped() // When the card is tapped, resume dialogue and destroy card
    {
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager is not assigned!");
            return;
        }

        dialogueManager.ResumeDialogue();
        StartCoroutine(DestroyInstantiatedCard()); // Coroutine is used to make sure dialogue is resumed before the card is destoryed
    }
    private IEnumerator DestroyInstantiatedCard() 
    {
        yield return new WaitForSeconds(0.1f);
        if (instantiatedCard != null)
        {
            Destroy(instantiatedCard);
        }
    }
}
