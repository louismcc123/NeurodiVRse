using System.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform playerHandTransform;
    private GameObject instantiatedCard;

    public void InstantiateCard()
    {
        if (instantiatedCard == null && cardPrefab != null && playerHandTransform != null)
        {
            instantiatedCard = Instantiate(cardPrefab, playerHandTransform.position, playerHandTransform.rotation, playerHandTransform);
        }
    }

    public void OnCardTapped()
    {
        StartCoroutine(DestroyCardWithDelay());
    }

    private IEnumerator DestroyCardWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(instantiatedCard);
    }
}