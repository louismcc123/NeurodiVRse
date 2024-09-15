using OpenAI;
using System.Collections.Generic;
using UnityEngine;

public class GroupChatGPT : ChatGPT
{
    private List<GroupChatGPT> npcGroup = new List<GroupChatGPT>();

    private GroupAIDialogueController groupAIDialogueController; 

    protected override void Start()
    {
        base.Start();

        groupAIDialogueController = GetComponent<GroupAIDialogueController>();

        if (!npcGroup.Contains(this))
        {
            npcGroup.Add(this);
        }
        Debug.Log($"GroupChatGPT started for NPC: {gameObject.name}");
    }

    protected override async void SendReply()
    {
        Debug.Log("GroupChatGPT SendReply method called.");

        if (isDialoguePaused)
        {
            Debug.Log("Dialogue is paused. Reply not sent.");
            return;
        }

        // Choose a random NPC from the conversation manager if this isn't already the active NPC
        GroupConversationManager conversationManager = FindObjectOfType<GroupConversationManager>();
        if (conversationManager != null)
        {
            activeNPC = conversationManager.ChooseRandomNPC();
        }

        if (activeNPC != this)
        {
            Debug.Log($"{gameObject.name} is not the active NPC. Reply not sent.");
            return;
        }

        base.SendReply();
    }

    protected override void HandleResponse(string responseContent)
    {
        base.HandleResponse(responseContent);
        Debug.Log($"{gameObject.name} received response: {responseContent}");
        NotifyGroupMembers(responseContent);
    }

    private void NotifyGroupMembers(string messageContent)
    {
        Debug.Log($"{gameObject.name} is notifying group members about the message: {messageContent}");

        foreach (var npc in npcGroup)
        {
            if (npc != this)
            {
                Debug.Log($"{gameObject.name} notifying {npc.gameObject.name}");
                npc.ReceiveMessage(messageContent, this);
            }
        }
    }

    public void ReceiveMessage(string messageContent, GroupChatGPT sender)
    {
        Debug.Log($"{gameObject.name} received message from {sender.gameObject.name}: {messageContent}");

        if (isDialoguePaused || activeNPC != this)
        {
            Debug.Log($"{gameObject.name} is not the active NPC or dialogue is paused.");
            return;
        }

        var newMessage = new ChatMessage()
        {
            Role = "user",
            Content = messageContent
        };

        messages.Add(newMessage);
        SendReply();
    }

    public void GreetPlayer()
    {
        string greeting = "Hey! Welcome to the party!";
        Debug.Log($"{gameObject.name} is greeting the player with message: {greeting}");

        groupAIDialogueController.NPCTalking();

        ChatMessage message = new ChatMessage { Role = "npc", Content = greeting };
        AppendMessage(message);
        ttsBridge.Speak(messageText.text);
    }

    protected override void SetNpcTalking(bool isTalking)
    {
        if (isTalking)
        {
            groupAIDialogueController.NPCTalking();
        }
        else
        {
            groupAIDialogueController.NPCStopTalking();
        }
    }
}
