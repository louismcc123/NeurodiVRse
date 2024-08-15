using UnityEngine;
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

        /*private void AppendMessage(ChatMessage message)
        {
            Debug.Log($"Appending message: {message.Content}");

            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;

            Debug.Log($"Message appended. New content height: {height}");
        }*/

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
}
