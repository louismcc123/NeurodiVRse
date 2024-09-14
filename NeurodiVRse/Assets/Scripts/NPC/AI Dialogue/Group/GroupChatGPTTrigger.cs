using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChatGPTTrigger : MonoBehaviour
{
    private float timePlayerExited = -1f;
    public float resumeTimeThreshold = 5f;

    public GroupConversationManager groupConversationManager;
    public GroupChatGPT groupChatGPT;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (groupConversationManager != null)
            {
                if (Time.time - timePlayerExited > resumeTimeThreshold)
                {
                    groupConversationManager.StartConversation();
                }
                else
                {
                    groupChatGPT.ResumeActiveNPCDialogue();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timePlayerExited = Time.time;

            if (groupConversationManager != null)
            {
                groupChatGPT.PauseDialogue();
            }
        }
    }
}
