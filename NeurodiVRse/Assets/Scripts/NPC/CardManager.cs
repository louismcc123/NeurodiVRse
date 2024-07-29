using System.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform spawnTransform;

    private GameObject instantiatedCard;

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
