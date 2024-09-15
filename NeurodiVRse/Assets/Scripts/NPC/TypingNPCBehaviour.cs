using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingNPCBehaviour : MonoBehaviour
{
    public float typingDuration = 30f; 
    public float idleDuration = 10f;

    private Animator animator;
    private AIDialogueController npcAiDialogue;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError(gameObject.name + ": Animator component not found on any child GameObject.");
        }
    }

    private void Start()
    {
        StartCoroutine(TypingIdleCycle());
    }

    private IEnumerator TypingIdleCycle()
    {
        while (true)
        {
            animator.SetBool("IsTyping", true);
            yield return new WaitForSeconds(typingDuration);

            animator.SetBool("IsTyping", false);
            yield return new WaitForSeconds(idleDuration);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (npcAiDialogue != null)
            {
                npcAiDialogue.playerInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (npcAiDialogue != null)
            {
                npcAiDialogue.playerInRange = false;
            }
        }
    }
}
