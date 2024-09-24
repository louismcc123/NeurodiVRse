using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    private AIDialogueController aiDialogueController;
    private NPCInteraction npcInteraction;

    private void Awake()
    {
        aiDialogueController = GetComponent<AIDialogueController>();
        if (aiDialogueController != null)
        {
            Debug.LogWarning("ChatTrigger could not find aiDialogueController");
        }

        npcInteraction = GetComponent<NPCInteraction>();
        if (npcInteraction != null)
        {
            Debug.LogWarning("ChatTrigger could not find npcInteraction");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (aiDialogueController != null)
            {
                aiDialogueController.playerInRange = true;
            }
            if (npcInteraction != null)
            {
                npcInteraction.playerInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (aiDialogueController != null)
            {
                aiDialogueController.playerInRange = false;
            }
            if (npcInteraction != null)
            {
                npcInteraction.playerInRange = false;
            }
        }
    }
}
