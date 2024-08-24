/*using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button send;
        [SerializeField] private Button enter;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private NpcAiDialogue npcAiDialogue;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "Act as a barista working in a cafe called \"NeurodiVRse Cafe\". Don't break character. Don't ever mention that you are an AI model.";

        private void Start()
        {
            Debug.Log("ChatGPT script started.");

            send.onClick.AddListener(SendReply);
            enter.onClick.AddListener(SendReply);
        }

        private void AppendMessage(ChatMessage message)
        {
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
        }


        private async void SendReply()
        {
            Debug.Log("SendReply triggered.");

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

            Debug.Log("Sending request to OpenAI...");

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o-mini",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                npcAiDialogue.SetNpcTalking(true);

                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                Debug.Log($"Received response: {message.Content}");

                messages.Add(message);
                AppendMessage(message);
            }
            else
            {
                npcAiDialogue.SetNpcTalking(false);

                Debug.LogWarning("No text was generated from this prompt.");
            }

            npcAiDialogue.SetNpcTalking(false);

            send.enabled = true;
            enter.enabled = true;
            inputField.enabled = true;
        }
    }
}*/

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
        [SerializeField] protected Button send;
        [SerializeField] protected Button enter;
        [SerializeField] protected ScrollRect scroll;

        [SerializeField] protected RectTransform sent;
        [SerializeField] protected RectTransform received;

        [SerializeField] private string npcPrompt;

        [Header("NPC AI Dialogue")]
        [SerializeField] protected NpcAiDialogue npcAiDialogue;
        [SerializeField] protected AdviceManager adviceManager;

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

            prompt = npcPrompt;

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

            Debug.Log("Sending request to OpenAI...");

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                npcAiDialogue.isNpcTalking = true;

                var message = completionResponse.Choices[0].Message;         
                message.Content = message.Content.Trim();
                Debug.Log($"Received response: {message.Content}");

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

                Debug.Log($"Received message: {message.Content}");

                HandleResponse(message.Content);

                messages.Add(message);
                AppendMessage(message);

                onChatGPTMessageReceived?.Invoke(message.Content);
                //ttsBridge.Speak(message.Content);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            //npcAiDialogue.SetNpcTalking(false);
            npcAiDialogue.isNpcTalking = false;

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
        }

        protected virtual void HandleResponse(string responseContent)
        {

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

            //npcAiDialogue.SetNpcTalking(false);
            npcAiDialogue.isNpcTalking = false;

            openAICanvas.SetActive(false);
            npcDialogueCanvas.SetActive(false);
        }

        public void ResumeDialogue()
        {
            isDialoguePaused = false;
            Debug.Log($"Dialogue resumed at {Time.time}");

            //npcAiDialogue.SetNpcTalking(true);
            npcAiDialogue.isNpcTalking = true;

            openAICanvas.SetActive(true);
            npcDialogueCanvas.SetActive(true);
        }

        public void ActivateNPC()
        {
            activeNPC = this;
        }

        public void DeactivateNPC()
        {
            if (activeNPC == this)
            {
                activeNPC = null;
            }
        }
    }
}