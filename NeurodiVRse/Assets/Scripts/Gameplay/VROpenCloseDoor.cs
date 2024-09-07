using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VROpenCloseDoor : MonoBehaviour
{
    public Animator openandclose;
    public bool open;
    public Transform Player;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        open = false;
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to XR interaction events
        grabInteractable.selectEntered.AddListener(OnSelectEnter);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (Player)
        {
            float dist = Vector3.Distance(Player.position, transform.position);
            if (dist < 3)
            {
                if (open == false)
                {
                    StartCoroutine(opening());
                }
                else
                {
                    StartCoroutine(closing());
                }
            }
        }
    }

    IEnumerator opening()
    {
        print("you are opening the door");
        openandclose.Play("Opening");
        open = true;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator closing()
    {
        print("you are closing the door");
        openandclose.Play("Closing");
        open = false;
        yield return new WaitForSeconds(.5f);
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnSelectEnter);
    }
}
