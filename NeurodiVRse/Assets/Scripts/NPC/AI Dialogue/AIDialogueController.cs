using Meta.WitAi.TTS.Utilities;
using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject NPCDialogueCanvas;
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private InputActionReference aButton;

    public bool playerInRange = false;
    public bool isNpcTalking = false;
    private bool isInteracting = false;

    [SerializeField] private TTSSpeaker ttsSpeaker;

    private Animator animator;
    private ChatGPT chatGPT;
    private LeavingNPCBehaviour leavingNPCBehaviour;

    public CharacterController characterController;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        chatGPT = GetComponent<ChatGPT>();
        leavingNPCBehaviour = GetComponent<LeavingNPCBehaviour>();
    }

    private void OnEnable()
    {
        if (ttsSpeaker != null)
        {
            ttsSpeaker.OnPlaybackCompleteEvent += HandlePlaybackComplete;
        }
    }

    private void OnDisable()
    {
        if (ttsSpeaker != null)
        {
            ttsSpeaker.OnPlaybackCompleteEvent -= HandlePlaybackComplete;
        }
    }

    private void Update()
    {
        if (playerInRange && !isInteracting)
        {
            if (this.gameObject.name == "Barista NPC")
            {
                Interact();
            }
            else
            {
                ShowInteractCanvas();

                if (aButton.action.triggered)
                {
                    if (leavingNPCBehaviour != null)
                    {
                        isInteracting = true;
                        leavingNPCBehaviour.DeactivatePhone();
                        characterController.PauseMovement();
                        characterController.FacePlayer();
                    }

                    HideInteractCanvas();
                    Interact();
                }
            }
        }
        else if (!playerInRange && isInteracting)
        {
            if (leavingNPCBehaviour != null)
            {
                leavingNPCBehaviour.ActivatePhone();
            }

            EndInteraction();
        }

        UpdateTalking();
    }

    private void Interact()
    {
        isInteracting = true;
        chatGPT.enabled = true;
        chatGPT.ActivateNPC();
        openAICanvas.SetActive(true);

        UpdateTalking();
    }

    private void EndInteraction()
    {
        isInteracting = false;
        HideInteractCanvas();
        NPCDialogueCanvas.SetActive(false);
        openAICanvas.SetActive(false);
        chatGPT.DeactivateNPC();
        chatGPT.enabled = false;
    }

    private void ShowInteractCanvas()
    {
        interactCanvas.SetActive(true);
    }

    private void HideInteractCanvas()
    {
        interactCanvas.SetActive(false);
    }

    public void UpdateTalking()
    {
        if (isNpcTalking)
        {
            NPCDialogueCanvas.SetActive(true);
            animator.SetBool("IsTalking", true);

            //StartCoroutine(StopTalkingAfterDelay(8f));
        }
        else
        {
            NPCDialogueCanvas.SetActive(false);
            animator.SetBool("IsTalking", false);
        }
    }

    private void HandlePlaybackComplete()
    {
        isNpcTalking = false;
        UpdateTalking();
    }
}