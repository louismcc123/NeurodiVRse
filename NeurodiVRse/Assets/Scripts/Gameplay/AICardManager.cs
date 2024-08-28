using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform spawnTransform;

    private GameObject instantiatedCard;

    private BaristaChatGPT baristaChatGPT;

    private void Start()
    {
        baristaChatGPT = GetComponent<BaristaChatGPT>();
    }

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
        if (baristaChatGPT == null)
        {
            Debug.LogError("ChatGPT is not assigned!");
            return;
        }

        //chatGPT.ResumeDialogue();
        StartCoroutine(DestroyInstantiatedCard());
        baristaChatGPT.StartCoroutine(baristaChatGPT.SayThankYou());
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