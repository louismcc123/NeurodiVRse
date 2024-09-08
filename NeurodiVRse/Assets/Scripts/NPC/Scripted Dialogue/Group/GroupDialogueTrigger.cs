using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupDialogueTrigger : MonoBehaviour
{
    private float timePlayerExited = -1f;
    public float resumeTimeThreshold = 5f;

    public GroupDialogueManager groupDialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //npcBehaviours.playerInRange = true;

            if (groupDialogueManager != null)
            {
                if (Time.time - timePlayerExited > resumeTimeThreshold)
                {
                    groupDialogueManager.StartGroupDialogue();
                }
                else
                {
                    groupDialogueManager.ResumeGroupDialogue();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //npcBehaviours.playerInRange = false;

            timePlayerExited = Time.time;

            if (groupDialogueManager != null)
            {
                groupDialogueManager.PauseGroupDialogue();
            }
        }
    }
}
