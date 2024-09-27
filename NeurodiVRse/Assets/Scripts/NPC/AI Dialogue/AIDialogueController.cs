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
    [SerializeField] private InputActionReference interact;

    public bool playerInRange = false;
    public bool isNpcTalking = false;
    private bool isInteracting = false;

    [SerializeField] private TTSSpeaker ttsSpeaker;

    private Animator animator;
    private ChatGPT chatGPT;
    //private CafeChatGPT cafeChatGPT;
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
            
            ttsSpeaker.OnPlaybackCompleteEvent += HandlePlaybackComplete; // Subscribe to TTS completion event
        }
    }

    private void OnDisable()
    {
        if (ttsSpeaker != null)
        {
           
            ttsSpeaker.OnPlaybackCompleteEvent -= HandlePlaybackComplete; // Unsubscribe from TTS completion event
        }
    }

    private void Update()
    {
       
        if (playerInRange && !isInteracting) // If the player is within range and hasn't started interacting yet
        {
            if (gameObject.name == "Barista NPC")
            {
                Interact(); // Barista automatically interacts with player
            }
            else
            {
                ShowInteractCanvas(); // Show prompt to interact

                if (interact.action.triggered) // Player interacts with npc

                {
                    if (leavingNPCBehaviour != null) // if the npc is the leaving/texting npc, deactivate their phone, make them stop walking and face player
                    {
                        isInteracting = true;
                        leavingNPCBehaviour.DeactivatePhone();
                        characterController.PauseMovement();
                        characterController.FacePlayer();
                    }

                    HideInteractCanvas(); // hide interaction prompt
                    Interact(); // start dialogue
                }
            }
        }
        else if (!playerInRange && isInteracting) // If the player leaves range while interacting

        {
            if (leavingNPCBehaviour != null) // leaving/texting npc continues usual behaviours 
            {
                leavingNPCBehaviour.ActivatePhone();
            }

            EndInteraction(); // end dialogue when player leaves
        }

        UpdateTalking(); // Continuously check and update NPC's talking state
    }

    private void Interact() // Start the interaction: enable AI and show dialogue canvas
    {
        isInteracting = true;
        chatGPT.enabled = true;
        chatGPT.ActivateNPC();
        openAICanvas.SetActive(true);

        UpdateTalking(); // Update NPC talking state
    }

    private void EndInteraction() // End the interaction: hide UI, disable AI, reset talking state
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

    public void UpdateTalking() // Set NPC's talking state and corresponding UI/animation updates
    {
        if (isNpcTalking)
        {
            NPCDialogueCanvas.SetActive(true);
            animator.SetBool("IsTalking", true);
        }
        else
        {
            NPCDialogueCanvas.SetActive(false);
            animator.SetBool("IsTalking", false);
        }
    }

    private void HandlePlaybackComplete() // Called when TTS playback is done, updates NPC state
    {
        isNpcTalking = false;
        UpdateTalking();
    }
}