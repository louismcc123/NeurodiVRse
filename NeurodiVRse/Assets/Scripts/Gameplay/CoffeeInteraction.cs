using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CoffeeInteraction : MonoBehaviour
{
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found in the scene.");
        }
    }

    private void OnEnable()
    {
        GetComponent<XRGrabInteractable>().onSelectEntered.AddListener(OnPickup);
        GetComponent<XRGrabInteractable>().onSelectExited.AddListener(OnDrop);
    }

    private void OnDisable()
    {
        GetComponent<XRGrabInteractable>().onSelectEntered.RemoveListener(OnPickup);
        GetComponent<XRGrabInteractable>().onSelectExited.RemoveListener(OnDrop);
    }

    private void OnPickup(XRBaseInteractor interactor) // Detect when player has coffee
    {
        if (playerStats != null)
        {
            playerStats.hasCoffee = true;
            Debug.Log("Coffee picked up.");
        }
    }

    private void OnDrop(XRBaseInteractor interactor)
    {
        if (playerStats != null)
        {
            playerStats.hasCoffee = false;
            Debug.Log("Coffee dropped.");
        }
    }
}
