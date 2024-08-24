using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    public NpcAiDialogue NpcAiDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (NpcAiDialogue != null)
            {
                NpcAiDialogue.playerInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (NpcAiDialogue != null)
            {
                NpcAiDialogue.playerInRange = false;
            }
        }
    }
}
