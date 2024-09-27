using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationStats : MonoBehaviour
{
    public float totalScriptedConversationTime = 0f;
    public float totalScriptedScore = 0f;
    public int scriptedCount = 0;

    public float totalLLMConversationTime = 0f;
    public float totalLLMScore = 0f;
    public int llmCount = 0;

    public float averageScriptedConversationTime => scriptedCount > 0 ? totalScriptedConversationTime / scriptedCount : 0f; // Average conversation time for scripted conversations
    public float averageScriptedScore => scriptedCount > 0 ? totalScriptedScore / scriptedCount : 0f; // Average score for scripted conversations

    public float averageLLMConversationTime => llmCount > 0 ? totalLLMConversationTime / llmCount : 0f; // Average conversation time for LLM conversations
    public float averageLLMScore => llmCount > 0 ? totalLLMScore / llmCount : 0f; // Average score for LLM conversations

    public void RecordScriptedConversation(float conversationTime, float score)
    {
        totalScriptedConversationTime += conversationTime; // Add to total time
        totalScriptedScore += score; // Add to total score
        scriptedCount++; // Increment count
    }

    public void RecordLLMConversation(float conversationTime, float score)
    {
        totalLLMConversationTime += conversationTime; // Add to total time
        totalLLMScore += score; // Add to total score
        llmCount++; // Increment count
    }

    public void ResetStatistics() // Resets all recorded statistics to zero.
    {
        totalScriptedConversationTime = 0f;
        totalScriptedScore = 0f;
        scriptedCount = 0;

        totalLLMConversationTime = 0f;
        totalLLMScore = 0f;
        llmCount = 0;
    }

    public void LogStatistics()
    {
        Debug.Log($"Scripted Average Conversation Time: {averageScriptedConversationTime} seconds");
        Debug.Log($"Scripted Average Score: {averageScriptedScore}");
        Debug.Log($"LLM Average Conversation Time: {averageLLMConversationTime} seconds");
        Debug.Log($"LLM Average Score: {averageLLMScore}");
        Debug.Log($"Scripted Conversations Count: {scriptedCount}");
        Debug.Log($"LLM Conversations Count: {llmCount}");
    }
}
