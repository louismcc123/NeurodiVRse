using Meta.WitAi.TTS.Utilities;
using OpenAI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroupAIDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject NPCDialogueCanvas;
    [SerializeField] private TTSSpeaker ttsSpeaker;

    public bool isThisNPCTalking = false;

    private Animator animator;
    private GroupChatGPT groupChatGPT;
    private GroupConversationManager groupConversationManager;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        groupChatGPT = GetComponent<GroupChatGPT>();
        groupConversationManager = GetComponentInParent<GroupConversationManager>();
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

    public void NPCTalking()
    {
        NPCDialogueCanvas.SetActive(true);
        animator.SetBool("IsTalking", true);
    }

    public void NPCStopTalking()
    {
        NPCDialogueCanvas.SetActive(false);
        animator.SetBool("IsTalking", false);
    }

    private void HandlePlaybackComplete()
    {
        NPCStopTalking();
        groupConversationManager.OnNPCFinishedSpeaking();
    }
}
