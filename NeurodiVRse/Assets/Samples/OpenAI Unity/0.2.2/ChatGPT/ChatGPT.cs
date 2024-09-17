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
        public static Action<string> onChatGPTMessageReceived;

        protected OpenAIApi openai = new OpenAIApi();
        protected List<ChatMessage> messages = new List<ChatMessage>();
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
                prompt = npcPrompt;
            }

            inputField.enabled = true;

            send.onClick.AddListener(SendReply);
            enter.onClick.AddListener(SendReply);

            aiDialogueController = GetComponent<AIDialogueController>();
        }

        protected virtual async void SendReply()
        {
            if (isDialoguePaused || activeNPC != this)
            {
                Debug.Log("Dialogue is paused or the active NPC is different. Reply not sent.");
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
                newMessage.Content = prompt + characterInstruction + "\n" + inputField.text + maxMessageLengthinstruction;
            }

            messages.Add(newMessage);
            send.enabled = false;
            enter.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            keyboard.SetActive(false);
            SetNpcTalking(true);

            Debug.Log("Sending request to OpenAI...");

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

            send.enabled = true;
            enter.enabled = true;
            inputField.enabled = true;
        }

        protected void AppendMessage(ChatMessage message)
        {
            SetNpcTalking(true);
            messageText.text = string.Empty;
            messageText.text = message.Content;

            Debug.Log($"Appending message: {message.Content}");
        }

        protected virtual void HandleResponse(string responseContent)
        {

        }

        protected string FilterInstructionalText(string content)
        {
            return System.Text.RegularExpressions.Regex.Replace(content, @"\([^)]*\)", "").Trim();
        }

        public void ActivateNPC()
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

            //Debug.Log($"{gameObject.name}: NPC deactivated.");
        }

        protected void DisplayAdvice(string adviceContent)
        {
            if (!string.IsNullOrEmpty(adviceContent))
            {
                adviceManager.DisplayAdvice(adviceContent);
            }
        }

        public virtual void PauseDialogue()
        {
            isDialoguePaused = true;
            //Debug.Log($"Dialogue paused.");

            SetNpcTalking(false);
            npcDialogueCanvas.SetActive(false);
            openAICanvas.SetActive(false);
        }

        public virtual void ResumeDialogue()
        {
            isDialoguePaused = false;
            //Debug.Log($"Dialogue resumed.");

            SetNpcTalking(true);
        }

        public virtual void SetNpcTalking(bool isTalking)
        {
            aiDialogueController.isNpcTalking = isTalking;
        }
    }
}