using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using OpenAI;

namespace OpenAI
{
    public class OpenAIManager : MonoBehaviour
    {
        public OnResponseEvent OnResponse;

        [System.Serializable]
        public class OnResponseEvent : UnityEvent<string> { }

        private OpenAIApi openAIApi;
        private List<ChatMessage> messages = new List<ChatMessage>();

        private void Awake()
        {
            var config = new Configuration();
            openAIApi = new OpenAIApi(config.ApiKey);
        }

        public async void AskChatGPT(string newText)
        {
            var newMessage = new ChatMessage { Role = "user", Content = newText };
            messages.Add(newMessage);

            var request = new CreateChatCompletionRequest
            {
                Messages = messages,
                Model = "gpt-3.5-turbo-0613"
            };

            var response = await openAIApi.CreateChatCompletion(request);
            if (response == null || response.Choices == null || response.Choices.Count == 0)
            {
                Debug.LogError("Failed to get a valid response.");
                return;
            }

            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            Debug.Log(chatResponse.Content);
            OnResponse.Invoke(chatResponse.Content);
        }
    }

    [System.Serializable]
    public class CreateChatCompletionRequest
    {
        public string Model;
        public List<ChatMessage> Messages;
    }

    [System.Serializable]
    public class CreateChatCompletionResponse
    {
        public List<ChatChoice> Choices;
    }

    [System.Serializable]
    public class ChatMessage
    {
        public string Role;
        public string Content;
    }

    [System.Serializable]
    public class ChatChoice
    {
        public ChatMessage Message;
    }
}
