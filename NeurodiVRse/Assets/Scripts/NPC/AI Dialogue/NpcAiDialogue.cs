using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcAiDialogue : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject NPCSpeechCanvas;
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private InputActionReference aButton;
    
    public bool playerInRange = false;
    public bool isNpcTalking = false;
    private bool isInteracting = false;

    private Animator animator;
    private ChatGPT chatGPT;

    public CharacterController characterController;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        chatGPT = GetComponent<ChatGPT>();
    }

    private void Update()
    {
        if (playerInRange && !isInteracting)
        {
            //Debug.Log(gameObject.name + ": Player is in range.");

            if (this.gameObject.name == "Barista NPC")
            {
                //Debug.Log("Interacting with Barista NPC.");

                Interact();
            }
            else
            {
                ShowInteractCanvas();

                if (aButton.action.triggered)
                {
                    Debug.Log(gameObject.name + ": A button pressed. Interacting with NPC.");

                    if (this.gameObject.name == "Leaving NPC")
                    {
                        characterController.PauseMovement();
                        characterController.FacePlayer();
                    }

                    HideInteractCanvas();
                    Interact();
                }
            }
        }
        else if(!playerInRange && isInteracting)
        {
            //Debug.Log(gameObject.name + ": Player is not in range.");
            EndInteraction();
        }
    }

    private void Interact()
    {
        //Debug.Log($"{gameObject.name}: Interact called at {Time.time}");
        isInteracting = true;
        chatGPT.enabled = true;
        chatGPT.ActivateNPC();
        openAICanvas.SetActive(true);

        if (isNpcTalking)
        {
            //Debug.Log(gameObject.name + ": NPC is talking, showing NPCSpeechCanvas.");

            NPCSpeechCanvas.SetActive(true);
            animator.SetBool("IsTalking", true);

            StartCoroutine(StopTalkingAfterDelay(8f));
        }
        else
        {
            NPCSpeechCanvas.SetActive(false);
            animator.SetBool("IsTalking", false);
            //Debug.Log(gameObject.name + ": NPC isnt talking, hiding NPCSpeechCanvas.");

        }
    }

    private void EndInteraction()
    {
        //Debug.Log($"{gameObject.name}: EndInteraction called at {Time.time}");
        isInteracting = false;
        HideInteractCanvas();
        NPCSpeechCanvas.SetActive(false);
        openAICanvas.SetActive(false);
        chatGPT.DeactivateNPC();
        chatGPT.enabled = false;
    }

    private void ShowInteractCanvas()
    {
        //Debug.Log(gameObject.name + ": Showing interact canvas.");

        interactCanvas.SetActive(true);
    }

    private void HideInteractCanvas()
    {
        //Debug.Log(gameObject.name + ": Hiding interact canvas.");

        interactCanvas.SetActive(false);
    }

    private IEnumerator StopTalkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Debug.Log(gameObject.name + ": NPC finished talking, hiding NPCSpeechCanvas.");

        isNpcTalking = false;
        NPCSpeechCanvas.SetActive(false);
        animator.SetBool("IsTalking", false);
    }
}