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
    //[SerializeField] private BaristaBehaviour baristaBehaviour;
    [SerializeField] private InputActionReference aButton;
    
    public Canvas interactCanvas;
    public bool playerInRange = false;
    private bool isNpcTalking = false;

    private Animator animator;
    private ChatGPT chatGPT;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError(gameObject.name + ": Animator component not found on any child GameObject.");
        }

        HideInteractCanvas();
    }
    
    private void Update()
    {
        if (playerInRange)
        {
            if (this.gameObject.name == "Barista NPC")
            {
                Interaction();
            }
            else
            {
                ShowInteractCanvas();

                if (aButton.action.triggered)
                {                    
                    HideInteractCanvas();
                    Interaction();
                }
            }
        }
        else
        {
            HideInteractCanvas();
            NPCSpeechCanvas.SetActive(false);
            openAICanvas.SetActive(false);
        }
    }

    private void Interaction()
    {
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

    public void SetNpcTalking(bool talking)
    {
        isNpcTalking = talking;
    }
}