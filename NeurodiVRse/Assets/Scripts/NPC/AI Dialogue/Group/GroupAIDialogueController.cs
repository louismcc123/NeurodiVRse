using Meta.WitAi.TTS.Utilities;
using OpenAI;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GroupAIDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject NPCDialogueCanvas;
    [SerializeField] private TTSSpeaker ttsSpeaker;

    public bool isThisNPCTalking = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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
        if (isThisNPCTalking)
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

    private void HandlePlaybackComplete()
    {
        isThisNPCTalking = false;
        //groupConversationManager.OnNPCFinishedSpeaking();
    }
}
