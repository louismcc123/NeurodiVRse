using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference aButton;
    public Canvas interactCanvas;
    private Actor actor;
    public bool playerInRange = false;

    private void Awake()
    {
        actor = GetComponent<Actor>();
        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(false);
        }
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
                    actor.SpeakTo();
                    HideInteractCanvas();
                }
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
