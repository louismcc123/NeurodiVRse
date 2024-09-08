using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PartyLeavingDoor : MonoBehaviour
{
    public GameObject leaveCanvas;
    public Transform player;

    private XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        if (interactable != null)
        {
            interactable.onSelectEntered.AddListener(OnSelectEnter);
            //interactable.onActivate.AddListener(OnActivate);
        }

        leaveCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        if (interactable != null)
        {
            interactable.onSelectEntered.AddListener(OnSelectEnter);
            //interactable.onActivate.AddListener(OnActivate);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
        {
            interactable.onSelectEntered.RemoveListener(OnSelectEnter);
            //interactable.onActivate.RemoveListener(OnActivate);
        }
    }

    private void OnSelectEnter(XRBaseInteractor interactor)
    {
        if (player)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist < 3)
            {
                leaveCanvas.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.onSelectEntered.RemoveListener(OnSelectEnter);
            //interactable.onActivate.RemoveListener(OnActivate);
        }
    }
}
