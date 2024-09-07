using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PartyLeavingDoor : MonoBehaviour
{
    public GameObject leaveCanvas;
    public Transform Player;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEnter);
        leaveCanvas.SetActive(false);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (Player)
        {
            float dist = Vector3.Distance(Player.position, transform.position);
            if (dist < 3)
            {
                print("UI is now showing");
                leaveCanvas.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEnter);
        }
    }
}
