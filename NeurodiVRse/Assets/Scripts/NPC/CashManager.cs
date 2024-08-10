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
    }

    public void OnCashHandedOver()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager is not assigned!");
            return;
        }

        dialogueManager.ResumeDialogue();
        StartCoroutine(DestroyInstantiatedCash());
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