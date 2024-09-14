using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference aButton;
    public Canvas interactCanvas;
    public bool playerInRange = false;

    private bool isInteracting = false;

    private Actor actor;
    private LeavingNPCBehaviour leavingNPCBehaviour;

    private void Awake()
    {
        actor = GetComponent<Actor>();

        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(false);
        }

        leavingNPCBehaviour = GetComponent<LeavingNPCBehaviour>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (actor.dialogueManager.IsDialogueActive() || actor.dialogueManager.IsDialoguePaused() || actor.dialogueManager.IsConversationFinished())
            {
                return;
            }

            if (actor.dialogueManager is BaristaDialogueManager)
            {
                actor.SpeakTo();
            }
            else
            {
                ShowInteractCanvas();

                if (aButton.action.triggered)
                {
                    if (leavingNPCBehaviour != null)
                    {
                        leavingNPCBehaviour.DeactivatePhone();
                        isInteracting = true;
                    }

                    actor.SpeakTo();
                    HideInteractCanvas();
                }
            }
        }
        else
        {
            if (isInteracting && leavingNPCBehaviour != null)
            {
                leavingNPCBehaviour.ActivatePhone();
                isInteracting = false;
            }
        }
    }

    private void ShowInteractCanvas()
    {
        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(true);
        }
    }

    private void HideInteractCanvas()
    {
        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(false);
        }
    }
}
