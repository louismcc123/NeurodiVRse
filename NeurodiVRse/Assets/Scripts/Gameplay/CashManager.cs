using System.Collections;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public GameObject cashPrefab;
    public Transform spawnTransform;

    private GameObject instantiatedCash;

    public BaristaDialogueManager dialogueManager;

    public void InstantiateCash()
    {
        if (instantiatedCash == null && cashPrefab != null && spawnTransform != null)
        {
            instantiatedCash = Instantiate(cashPrefab, spawnTransform.position, spawnTransform.rotation); // Instantiate the cash at spawn transform

            Rigidbody rb = instantiatedCash.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // disable physics
                rb.useGravity = false;
                rb.isKinematic = false;
            }

            SpinningMoney spinningMoney = instantiatedCash.GetComponent<SpinningMoney>();
            if (spinningMoney != null)
            {
                spinningMoney.enabled = true; // Spin the cash in the air
            }
        }
    }

    public void OnCashHandedOver() // When the cash is handed to barista, resume dialogue and destroy cash
    {
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager is not assigned!");
            return;
        }

        dialogueManager.ResumeDialogue();
        StartCoroutine(DestroyInstantiatedCash()); // Coroutine is used to make sure dialogue is resumed before the cash is destoryed
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