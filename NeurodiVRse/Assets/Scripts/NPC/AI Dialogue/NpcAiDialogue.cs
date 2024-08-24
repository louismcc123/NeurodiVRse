/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAiDialogue : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject NPCSpeechCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openAICanvas.SetActive(true);
            NPCSpeechCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openAICanvas.SetActive(false);
            NPCSpeechCanvas.SetActive(false);
        }
    }
}*/

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
        if (playerInRange)
        {
            //Debug.Log(gameObject.name + ": Player is in range.");

            if (this.gameObject.name == "Barista NPC")
            {
                Debug.Log("Interacting with Barista NPC.");

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
        else
        {
            //Debug.Log(gameObject.name + ": Player is not in range.");
            EndInteraction();
        }
    }

    private void Interact()
    {
        Debug.Log($"{gameObject.name}: Interact called at {Time.time}");

        openAICanvas.SetActive(true);

        if (isNpcTalking)
        {
            NPCSpeechCanvas.SetActive(true);
            animator.SetBool("IsTalking", true);

        }
        else
        {
            NPCSpeechCanvas.SetActive(false);
            animator.SetBool("IsTalking", false);
            Debug.Log(gameObject.name + ": NPC is talking, showing NPCSpeechCanvas.");

        }
    }

    private void EndInteraction()
    {
        Debug.Log($"{gameObject.name}: EndInteraction called at {Time.time}");

        HideInteractCanvas();
        NPCSpeechCanvas.SetActive(false);
        openAICanvas.SetActive(false);
        chatGPT.DeactivateNPC();
    }

    private void ShowInteractCanvas()
    {
        Debug.Log(gameObject.name + ": Showing interact canvas.");

        interactCanvas.SetActive(true);
    }

    private void HideInteractCanvas()
    {
        //Debug.Log(gameObject.name + ": Hiding interact canvas.");

        interactCanvas.SetActive(false);
    }
}