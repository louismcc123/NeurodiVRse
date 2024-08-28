using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [Header("Canvas References")]
        [SerializeField] private GameObject openAICanvas;
        [SerializeField] private GameObject npcDialogueCanvas;

        [Header("UI Elements")]
        [SerializeField] protected InputField inputField;
        [SerializeField] protected GameObject keyboard;
        [SerializeField] protected Button send;
        [SerializeField] protected Button enter;
        [SerializeField] protected ScrollRect scroll;
        [SerializeField] protected RectTransform sent;
        [SerializeField] protected RectTransform received;

        [Header("NPC AI Dialogue")]
        [SerializeField] protected NpcAiDialogue npcAiDialogue;
        [SerializeField] protected AdviceManager adviceManager;
        [SerializeField] private string npcPrompt;

        [Header("TTS Integration")]
        [SerializeField] private TTSBridge ttsBridge;

        [Header("Events")]
        public static Action<string> onChatGPTMessageReceived;

        protected float height;
        protected OpenAIApi openai = new OpenAIApi();

        protected List<ChatMessage> messages = new List<ChatMessage>();
        protected string prompt = "Act as an NPC in a VR environment. Respond to the user as appropriate for your role. Never break character or say that you are artificial intelligence.";

        public static Action<string> onChatGPTMessageRecieved;

        private bool isDialoguePaused = false;
        private static ChatGPT activeNPC;


        protected virtual void Start()
        {
            Debug.Log("ChatGPT script started.");

            if (!string.IsNullOrEmpty(npcPrompt))
            {
                prompt = npcPrompt;
            }

            inputField.enabled = true;

            send.onClick.AddListener(SendReply);
            enter.onClick.AddListener(SendReply);
        }

        protected async void SendReply()
        {
            Debug.Log($"SendReply triggered at {Time.time}");

            if (isDialoguePaused || activeNPC != this)
            {
                Debug.Log($"isDialoguePaused || activeNPC != this at {Time.time}");
                return;
            }

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };

            AppendMessage(newMessage);
            Debug.Log($"User message added: {newMessage.Content}");

            if (messages.Count == 0)
            {
                newMessage.Content = prompt + "\n" + inputField.text;
                Debug.Log("Initial prompt combined with user input.");
            }

            messages.Add(newMessage);
            Debug.Log($"Total messages count: {messages.Count}");

            send.enabled = false;
            enter.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            keyboard.SetActive(false);
            npcAiDialogue.isNpcTalking = true;

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
            npcAiDialogue.isNpcTalking = true;

            foreach (Transform child in scroll.content)
            {
                Destroy(child.gameObject);
            }

            Debug.Log($"Appending message: {message.Content}");

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;

            height = item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            LayoutRebuilder.ForceRebuildLayoutImmediate(scroll.content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);

            scroll.verticalNormalizedPosition = 1;

            Debug.Log($"Message appended. New content height: {height}");
            //npcAiDialogue.isNpcTalking = false;
        }

        protected virtual void HandleResponse(string responseContent)
        {

        }

        private string FilterInstructionalText(string content)
        {
            return System.Text.RegularExpressions.Regex.Replace(content, @"\([^)]*\)", "").Trim();
        }

        public void ActivateNPC()
        {
            if (activeNPC != null)
            {
                activeNPC.DeactivateNPC();
            }

            activeNPC = this;
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

        private void DisplayAdvice(string adviceContent)
        {
            if (!string.IsNullOrEmpty(adviceContent))
            {
                adviceManager.DisplayAdvice(adviceContent);
            }
        }

        public void PauseDialogue()
        {
            isDialoguePaused = true;
            Debug.Log($"Dialogue paused at {Time.time}");

            npcAiDialogue.isNpcTalking = false;

            openAICanvas.SetActive(false);
            npcDialogueCanvas.SetActive(false);
        }

        public void ResumeDialogue()
        {
            isDialoguePaused = false;
            Debug.Log($"Dialogue resumed at {Time.time}");

            npcAiDialogue.isNpcTalking = true;

            openAICanvas.SetActive(true);
            npcDialogueCanvas.SetActive(true);
        }
    }
}