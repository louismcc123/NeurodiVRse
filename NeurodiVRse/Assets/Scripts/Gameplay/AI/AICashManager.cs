using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICashManager : MonoBehaviour
{
    [SerializeField] private GameObject cashPrefab;
    [SerializeField] private Transform spawnTransform;

    private GameObject instantiatedCash;

    private BaristaChatGPT baristaChatGPT;

    private void Start()
    {
        baristaChatGPT = GetComponent<BaristaChatGPT>();
    }

    public void InstantiateCash()
    {
        if (instantiatedCash == null && cashPrefab != null && spawnTransform != null)
        {
            instantiatedCash = Instantiate(cashPrefab, spawnTransform.position, spawnTransform.rotation);

            Rigidbody rb = instantiatedCash.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false; 
                rb.isKinematic = false; 
            }

            SpinningMoney spinningMoney = instantiatedCash.GetComponent<SpinningMoney>();
            if (spinningMoney != null)
            {
                spinningMoney.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("CashPrefab or SpawnTransform is not assigned.");
        }
    }

    public void OnCashHandedOver()
    {
        if (baristaChatGPT == null)
        {
            Debug.LogError("ChatGPT is not assigned!");
            return;
        }

        //baristaChatGPT.ResumeDialogue();
        StartCoroutine(DestroyInstantiatedCash());
        baristaChatGPT.StartCoroutine(baristaChatGPT.SayThankYou());
    }

    private IEnumerator DestroyInstantiatedCash()
    {
        yield return new WaitForSeconds(0.1f);
        if (instantiatedCash != null)
        {
            Destroy(instantiatedCash);
        }
    }
}
