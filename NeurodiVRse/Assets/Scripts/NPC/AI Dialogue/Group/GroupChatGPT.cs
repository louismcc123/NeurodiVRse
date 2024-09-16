using OpenAI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupChatGPT : ChatGPT
{
    [SerializeField] protected Button interruptButton;
    
    private List<GroupChatGPT> npcGroup = new List<GroupChatGPT>();

    //public bool isCurrentlyActive = false;
    private bool isHandlingMessage = false;

    private GroupAIDialogueController groupAIDialogueController;
    private GroupConversationManager groupConversationManager;

    protected override void Start()
    {
        base.Start();

        groupAIDialogueController = GetComponent<GroupAIDialogueController>();
        groupConversationManager = GetComponentInParent<GroupConversationManager>();

        if (!npcGroup.Contains(this))
        {
            npcGroup.Add(this);
        }
        
        interruptButton.onClick.AddListener(OnCancelResponse);
    }

    protected override async void SendReply()
    {
        if (isDialoguePaused)
        {
            Debug.Log("Dialogue is paused. Reply not sent.");
            return;
        }

        groupConversationManager.NotifyConversationUpdate(this);

        GroupChatGPT activeNPC = groupConversationManager.GetActiveNPC();

        if (activeNPC != this)
        {
            Debug.Log($"{gameObject.name} is not the active NPC. Reply not sent.");
            return;
        }

        var newMessage = new ChatMessage()
        {
            Role = "user",
            Content = inputField.text
        };

        Debug.Log($"New player message: {newMessage.Content}");

        if (messages.Count == 0)
        {
            newMessage.Content = prompt + "\n" + inputField.text + maxMessageLengthinstruction;
        }

        messages.Add(newMessage);
        PlayerResponded(true);
        GreetingPlayed(true);
        send.enabled = false;
        enter.enabled = false;
        inputField.text = "";
        inputField.enabled = false;
        keyboard.SetActive(false);
        SetNpcTalking(true);
        groupConversationManager.NotifyNPCsToFaceSpeaker();

        //Debug.Log("Sending request to OpenAI...");

        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo",
            Messages = messages
        });

        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
        {
            var message = completionResponse.Choices[0].Message;
            message.Content = message.Content.Trim();

            string filteredContent = FilterInstructionalText(message.Content);
            Debug.Log($"Received response: {filteredContent}");

            if (!string.IsNullOrEmpty(filteredContent))
            {
                HandleResponse(filteredContent);
                messages.Add(new ChatMessage { Role = "assistant", Content = filteredContent });
                AppendMessage(new ChatMessage { Role = "assistant", Content = filteredContent });

                onChatGPTMessageReceived?.Invoke(filteredContent);
            }
            else
            {
                Debug.LogWarning("Filtered content is empty after removing instructional text.");
            }

            if (completionResponse.Choices.Count > 1)
            {
                var advice = completionResponse.Choices[1].Message;
                advice.Content = advice.Content.Trim();
                DisplayAdvice(advice.Content);
                Debug.Log($"Received advice: {advice.Content}");
            }
            else
            {
                Debug.Log("No advice message received.");
            }
        }
        else
        {
            Debug.LogWarning("No text was generated from this prompt.");
        }

        groupConversationManager.ChooseNextSpeaker();
        send.enabled = true;
        enter.enabled = true;
        inputField.enabled = true;
        PlayerResponded(false);
        //groupConversationManager.OnPlayerFinishedSpeaking();
    }

    protected override void HandleResponse(string responseContent)
    {
        //base.HandleResponse(responseContent);
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

        if (isHandlingMessage || isDialoguePaused || activeNPC != this)
        {
            return;
        }
        isHandlingMessage = true;

        var newMessage = new ChatMessage()
        {
            Role = "user",
            Content = messageContent
        };

        messages.Add(newMessage);
        SendReply();
        isHandlingMessage = false;
    }

    public void GreetPlayer()
    {
        string greeting = "Hey! Glad you could join us.";
        HandleResponse(greeting);
        messages.Add(new ChatMessage { Role = "assistant", Content = greeting });
        onChatGPTMessageReceived?.Invoke(greeting);

        groupConversationManager.NotifyConversationUpdate(this);
        groupConversationManager.NotifyNPCsToFaceSpeaker();
        GreetingPlayed(true);
    }

    public override void ResumeDialogue()
    {
        isDialoguePaused = false;
        openAICanvas.SetActive(true);
        SetNpcTalking(true);
        ActivateNPC();
    }

    public override void SetNpcTalking(bool isTalking)
    {
        groupAIDialogueController.isThisNPCTalking = isTalking;
        //Debug.Log($"{gameObject.name} has been set to is talking: {isTalking}");
    }

    public void PlayerResponded(bool responded)
    {
        groupConversationManager.playerResponded = responded;
    }

    public void GreetingPlayed(bool greetingPlayed)
    {
        groupConversationManager.greetingPlayed = greetingPlayed;
    }

    private void OnCancelResponse()
    {
        groupConversationManager.HandleInterruption();
    }
}
