using System.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform spawnTransform;

    private GameObject instantiatedCard;

    private DialogueManager dialogueManager;

    public void InstantiateCard()
    {
        if (instantiatedCard == null && cardPrefab != null && spawnTransform != null)
        {
            instantiatedCard = Instantiate(cardPrefab, spawnTransform.position, spawnTransform.rotation);

            Rigidbody rb = instantiatedCard.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = false; 
            }

            SpinningMoney spinningMoney = instantiatedCard.GetComponent<SpinningMoney>();
            if (spinningMoney != null)
            {
                spinningMoney.enabled = true;
            }
        }
    }

    public void OnCardTapped()
    {
        StartCoroutine(DestroyCardWithDelay());
        dialogueManager.isPaymentComplete = true;
        dialogueManager.ResumeDialogue();
    }

    private IEnumerator DestroyCardWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        if (instantiatedCard != null)
        {
            Destroy(instantiatedCard);
        }
    }
}
