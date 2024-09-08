using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VROpenCloseDoor : MonoBehaviour
{
    public Animator openandclose;
    public bool open;
    public Transform player;

    private XRSimpleInteractable interactable;

    void Start()
    {
        open = false;
        interactable = GetComponent<XRSimpleInteractable>();

        if (interactable != null)
        {
            interactable.onSelectEntered.AddListener(OnSelectEnter);
            //interactable.onActivate.AddListener(OnActivate);
        }
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
        HandleInteraction();
    }

    /*private void OnActivate(ActivateEventArgs args)
    {
        HandleInteraction();
    }*/

    private void HandleInteraction()
    {
        if (player)
        {
            float dist = Vector3.Distance(player.position, transform.position);

            if (dist < 3)
            {
                if (!open)
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
        openandclose.Play("Opening");
        open = true;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator closing()
    {
        openandclose.Play("Closing");
        open = false;
        yield return new WaitForSeconds(0.5f);
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
