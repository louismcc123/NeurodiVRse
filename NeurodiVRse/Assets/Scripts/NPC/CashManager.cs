using System.Collections;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public GameObject cashPrefab;
    public Transform playerHandTransform;
    private GameObject instantiatedCash;

    public void InstantiateCash()
    {
        if (instantiatedCash == null && cashPrefab != null && playerHandTransform != null)
        {
            instantiatedCash = Instantiate(cashPrefab, playerHandTransform.position, playerHandTransform.rotation, playerHandTransform);
        }
    }

    public void OnCashHandedOver()
    {
        StartCoroutine(DestroyCashWithDelay());
    }

    private IEnumerator DestroyCashWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(instantiatedCash);
    }
}
