using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private Canvas outOfBoundsUI;

    private void Awake()
    {
        outOfBoundsUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outOfBoundsUI.gameObject.SetActive(true);
        }
    }
}
