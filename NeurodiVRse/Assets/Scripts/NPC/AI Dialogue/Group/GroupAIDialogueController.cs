using Meta.WitAi.TTS.Utilities;
using OpenAI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GroupAIDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject nPCDialogueCanvas;
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
            nPCDialogueCanvas.SetActive(true);
            animator.SetBool("IsTalking", true);
        }
        else
        {
            nPCDialogueCanvas.SetActive(false);
            animator.SetBool("IsTalking", false);
        }
    }

    private void HandlePlaybackComplete() // when npc finishes speaking
    {
        isThisNPCTalking = false;
        Debug.Log("HandlePlaybackComplete");
        //groupConversationManager.OnNPCFinishedSpeaking();
    }
}
