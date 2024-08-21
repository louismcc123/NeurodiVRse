using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTimer : MonoBehaviour
{
    private float conversationStartTime;
    private float conversationEndTime;
    private bool isInConversation = false;

    public void StartConversation()
    {
        if (!isInConversation)
        {
            conversationStartTime = Time.time;
            isInConversation = true;
            Debug.Log("Conversation started at: " + conversationStartTime);
        }
    }

    public void EndConversation()
    {
        if (isInConversation)
        {
            conversationEndTime = Time.time;
            isInConversation = false;

            float conversationDuration = conversationEndTime - conversationStartTime;
            Debug.Log("Conversation ended at: " + conversationEndTime);
            Debug.Log("Conversation duration: " + conversationDuration + " seconds");
        }
    }
}
