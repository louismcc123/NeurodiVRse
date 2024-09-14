using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupDialogueTrigger : MonoBehaviour
{
    private float timePlayerExited = -Mathf.Infinity;
    public float resumeTimeThreshold = 5f;

    public GroupDialogueManager groupDialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range");
            if (groupDialogueManager != null)
            {
                if (Time.time - timePlayerExited > resumeTimeThreshold)
                {
                    Debug.Log("Starting new group dialogue.");
                    groupDialogueManager.StartGroupDialogue();
                }
                else
                {
                    Debug.Log("Resuming group dialogue.");
                    groupDialogueManager.ResumeGroupDialogue();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player no longer in range");

            timePlayerExited = Time.time;

            if (groupDialogueManager != null)
            {
                groupDialogueManager.PauseGroupDialogue();
            }
        }
    }
}
