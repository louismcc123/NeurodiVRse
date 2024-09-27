using OpenAI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GroupChatGPT : ChatGPT
{
    [SerializeField] protected Button interruptButton;

    private List<GroupChatGPT> npcGroup = new List<GroupChatGPT>();

    protected string settingsInstruction = "You are in a house party setting talking in a group with some friends.";

    //public bool isCurrentlyActive = false;
    private bool isHandlingMessage = false;

    private GroupAIDialogueController groupAIDialogueController;
    private GroupConversationManager groupConversationManager;

    protected override void Start()
    {
        base.Start();

        groupAIDialogueController = GetComponent<GroupAIDialogueController>();
        groupConversationManager = GetComponentInParent<GroupConversationManager>();

        if (!npcGroup.Contains(this)) // If this NPC isn't already in the group list, add it.
        {
            npcGroup.Add(this);
        }
        
        interruptButton.onClick.AddListener(OnCancelResponse);
    }

    protected override async void SendReply() // Sends the player's input to OpenAI and processes the response
    {
        if (isDialoguePaused || activeNPC != this)
        {
            Debug.Log("Dialogue is paused or the active NPC is different. Reply not sent.");
            return;
        }

        //groupConversationManager.NotifyConversationUpdate(this);

        /*GroupChatGPT activeNPC = groupConversationManager.GetActiveNPC();

        if (activeNPC != this)
        {
            Debug.Log($"{gameObject.name} is not the active NPC. Reply not sent.");
            return;
        }*/

        var newMessage = new ChatMessage() // Create a new message from the player's input
        {
            Role = "user",
            Content = inputField.text
        };

        Debug.Log($"New player message: {newMessage.Content}");

        if (messages.Count == 0) // If it's the first message, include the prompt and instructions
        {
            newMessage.Content = prompt + settingsInstruction + characterInstruction + "\n" + inputField.text + maxMessageLengthinstruction;
        }

        messages.Add(newMessage); // Add the player's message to the list of messages
        PlayerResponded(true);
        GreetingPlayed(true); // set greeted player bool to true so that they are not greeted more than once

        // Disable the input and send buttons while waiting for the response
        send.enabled = false;
        enter.enabled = false;
        inputField.text = ""; // Clear the input field
        inputField.enabled = false;
        keyboard.SetActive(false);
        SetNpcTalking(true); // Indicate NPC is now talking for ui and animation
        groupConversationManager.NotifyNPCsToFaceSpeaker(); // tell npcs to face them

        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest() // Send the message to OpenAI's chat model
        {
            Model = "gpt-3.5-turbo",
            Messages = messages
        });

        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0) // Process the OpenAI response and update NPC dialogue
        {
            var message = completionResponse.Choices[0].Message;
            message.Content = message.Content.Trim();

            string filteredContent = FilterInstructionalText(message.Content); // Filter out any unwanted instructional text from the response
            Debug.Log($"Received response: {filteredContent}");

            if (!string.IsNullOrEmpty(filteredContent))
            {
                HandleResponse(filteredContent); // Apply NPC response logic e.g. barista payment sequence
                messages.Add(new ChatMessage { Role = "assistant", Content = filteredContent }); // Store response
                AppendMessage(new ChatMessage { Role = "assistant", Content = filteredContent }); // Display response

                onChatGPTMessageReceived?.Invoke(filteredContent); // Trigger event indicating a new NPC message was received
            }
            else
            {
                Debug.LogWarning("Filtered content is empty after removing instructional text.");
            }

            if (completionResponse.Choices.Count > 1) // If there's additional advice in the response, display it
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

        groupConversationManager.ChooseNextSpeaker(); // choose next npc to speak

        // Re-enable the input and send buttons after the response
        send.enabled = true;
        enter.enabled = true;
        inputField.enabled = true;
        PlayerResponded(false);
        //groupConversationManager.OnPlayerFinishedSpeaking();
    }

    protected override void HandleResponse(string responseContent)
    {
        Debug.Log($"{gameObject.name} received response: {responseContent}");
        NotifyGroupMembers(responseContent);
    }

    private void NotifyGroupMembers(string messageContent) // update all npcs of dialogue
    {
        Debug.Log($"{gameObject.name} is notifying group members about the message: {messageContent}");

        foreach (var npc in npcGroup)
        {
            if (npc != this)
            {
                Debug.Log($"{gameObject.name} notifying {npc.gameObject.name}");
                npc.ReceiveMessage(messageContent, this); // Send the message to the other NPCs.
            }
        }
    }

    public void ReceiveMessage(string messageContent, GroupChatGPT sender) // Method for an NPC to receive a message from another NPC.
    {
        Debug.Log($"{gameObject.name} received message from {sender.gameObject.name}: {messageContent}");

        if (isHandlingMessage || isDialoguePaused)// || activeNPC != this)
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
        groupConversationManager.NotifyNPCsToFaceSpeaker(); // Notify other NPCs to face this NPC.

        string greeting = "Hey! Glad you could join us.";

        var newGreeting = new ChatMessage()
        {
            Role = "assistant",
            Content = greeting
        };

        HandleResponse(newGreeting.Content); // Handle the greeting as a response.
        messages.Add(newGreeting); // Add the greeting to the conversation history.
        AppendMessage(newGreeting); // Display the greeting on the screen.
        onChatGPTMessageReceived?.Invoke(newGreeting.Content);// Trigger event for the greeting.

        //groupConversationManager.NotifyConversationUpdate(this);

        GreetingPlayed(true);// Update greeting played state.
    }

    /*public override void ResumeDialogue()
    {
        isDialoguePaused = false;
        openAICanvas.SetActive(true);
        SetNpcTalking(true);
        ActivateNPC();
    }*/

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

    private void OnCancelResponse() // for when the player interrupts the conversation
    {
        groupConversationManager.HandleInterruption(); // Call the interruption handler in the conversation manager.
    }
}