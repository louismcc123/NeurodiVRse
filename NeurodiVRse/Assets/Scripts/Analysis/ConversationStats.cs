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

    public float averageScriptedConversationTime => scriptedCount > 0 ? totalScriptedConversationTime / scriptedCount : 0f;
    public float averageScriptedScore => scriptedCount > 0 ? totalScriptedScore / scriptedCount : 0f;

    public float averageLLMConversationTime => llmCount > 0 ? totalLLMConversationTime / llmCount : 0f;
    public float averageLLMScore => llmCount > 0 ? totalLLMScore / llmCount : 0f;

    public void RecordScriptedConversation(float conversationTime, float score)
    {
        totalScriptedConversationTime += conversationTime;
        totalScriptedScore += score;
        scriptedCount++;
    }

    public void RecordLLMConversation(float conversationTime, float score)
    {
        totalLLMConversationTime += conversationTime;
        totalLLMScore += score;
        llmCount++;
    }

    public void ResetStatistics()
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
