using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering.LookDev;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting.Antlr3.Runtime;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [Header("Player Dialogue")]
        [SerializeField] protected GameObject openAICanvas;
        [SerializeField] protected InputField inputField;
        [SerializeField] protected GameObject keyboard;
        [SerializeField] protected Button send;
        [SerializeField] protected Button enter;

        [Header("NPC Dialogue")]
        [SerializeField] protected GameObject npcDialogueCanvas;
        [SerializeField] protected TextMeshProUGUI messageText;
        [SerializeField] private string npcPrompt;

        [Header("Advice Management")]
        [SerializeField] protected AdviceManager adviceManager;

        [Header("TTS Integration")]
        [SerializeField] protected TTSBridge ttsBridge;
        [SerializeField] private VoicePreset voicePreset;

        [Header("Events")]
        public static Action<string> onChatGPTMessageReceived; // Event triggered when a message is received from OpenAI

        protected OpenAIApi openai = new OpenAIApi();
        protected List<ChatMessage> messages = new List<ChatMessage>(); // List of chat messages in the current conversation
        protected string prompt = "Act as an NPC in a VR environment. " +
            "Respond to the user as appropriate for your role. ";
        protected string characterInstruction = "Never break character or say that you are artificial intelligence.";
protected string maxMessageLengthinstruction = "Limit the length of your " +
            "responses so that they are never more than 100 characters. ";

        protected AIDialogueController aiDialogueController;

        protected static ChatGPT activeNPC;

        public bool isDialoguePaused = false;

        protected virtual void Start()
        {
            if (!string.IsNullOrEmpty(npcPrompt))
            {
                prompt = npcPrompt; // set custom npc prompt
            }

            inputField.enabled = true;

            send.onClick.AddListener(SendReply);
            enter.onClick.AddListener(SendReply);

            aiDialogueController = GetComponent<AIDialogueController>();
        }

        protected virtual async void SendReply() // Sends the player's input to OpenAI and processes the response
        {
            if (isDialoguePaused || activeNPC != this)
            {
                Debug.Log("Dialogue is paused or the active NPC is different. Reply not sent.");
                return;
            }

            var newMessage = new ChatMessage() // Create a new message from the player's input
            {
                Role = "user",
                Content = inputField.text
            };

            Debug.Log($"New player message: {newMessage.Content}");

            if (messages.Count == 0) // If it's the first message, include the prompt and instructions
            {
                newMessage.Content = prompt + characterInstruction + "\n" + inputField.text + maxMessageLengthinstruction;
            }

            messages.Add(newMessage); // Add the player's message to the list of messages
            // Disable the input and send buttons while waiting for the response
            send.enabled = false;
            enter.enabled = false;
            inputField.text = ""; // Clear the input field
            inputField.enabled = false;
            keyboard.SetActive(false);
            SetNpcTalking(true); // Indicate NPC is now talking for ui and animation

            Debug.Log("Sending request to OpenAI...");

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

            // Re-enable the input and send buttons after the response
            send.enabled = true;
            enter.enabled = true;
            inputField.enabled = true;
        }

        protected void AppendMessage(ChatMessage message) // Display response and call for animations
        {
            SetNpcTalking(true);
            messageText.text = string.Empty;
            messageText.text = message.Content;

            Debug.Log($"Appending message: {message.Content}");
        }

        protected virtual void HandleResponse(string responseContent)
        {
            // Can be overridden to handle specific responses in derived classes
        }

        protected string FilterInstructionalText(string content)
        {
            return System.Text.RegularExpressions.Regex.Replace(content, @"\([^)]*\)", "").Trim(); // Removes any instructional text in parentheses from the response
        }

        public void ActivateNPC() // Activates this NPC as the current active one for dialogue
        {
            activeNPC = this;

            if (ttsBridge != null)
            {
                ttsBridge.SetVoicePreset(voicePreset.ToString());
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} is missing ttsBridge.");
            }

            Debug.Log($"{gameObject.name}: NPC activated.");
        }

        public void DeactivateNPC() 
        {
            if (activeNPC == this)
            {
                activeNPC = null;
            }
        }

        protected void DisplayAdvice(string adviceContent)  // Displays any additional advice received from OpenAI
        {
            if (!string.IsNullOrEmpty(adviceContent))
            {
                adviceManager.DisplayAdvice(adviceContent);
            }
        }

        public virtual void PauseDialogue() // Pauses the dialogue, hiding the NPC's dialogue canvas and stopping interactions
        {
            isDialoguePaused = true;

            SetNpcTalking(false);
            npcDialogueCanvas.SetActive(false);
            openAICanvas.SetActive(false);
        }

        public virtual void ResumeDialogue() // Resumes the dialogue if it was paused
        {
            isDialoguePaused = false;
            SetNpcTalking(true); 
        }

        public virtual void SetNpcTalking(bool isTalking)
        {
            aiDialogueController.isNpcTalking = isTalking; // Trigger animations and ui
        }
    }
}