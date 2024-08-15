/*using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Threading.Tasks;

namespace LLM
{
    public class OpenAIManager : MonoBehaviour
    {
        public OnResponseEvent OnResponse;

        [System.Serializable]
        public class OnResponseEvent : UnityEvent<string> { }

        private OpenAIApi openAIApi;
        private Dictionary<AICharacter, List<ChatMessage>> characterMessages = new Dictionary<AICharacter, List<ChatMessage>>();

        private void Awake()
        {
            var config = new Configuration();
            if (!string.IsNullOrEmpty(config.ApiKey))
            {
                openAIApi = new OpenAIApi(config.ApiKey);
            }
            else
            {
                Debug.LogError("API key is missing.");
            }
        }

        public async void AskChatGPT(AICharacter aiCharacter, string newText, UnityAction<string> onResponseCallback)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                Debug.LogWarning("Input text is empty.");
                return;
            }

            if (!characterMessages.ContainsKey(aiCharacter))
            {
                characterMessages[aiCharacter] = new List<ChatMessage>
                {
                    new ChatMessage { Role = "system", Content = aiCharacter.introductionText }
                };
            }

            var messages = characterMessages[aiCharacter];
            var newMessage = new ChatMessage { Role = "user", Content = newText };
            messages.Add(newMessage);

            var request = new CreateChatCompletionRequest
            {
                Messages = messages,
                Model = "gpt-3.5-turbo"
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
            onResponseCallback?.Invoke(chatResponse.Content);
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
}*/

/*using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;
using UnityEditor;

namespace LLM
{
    public class OpenAIManager : MonoBehaviour
    {
        public OnResponseEvent OnResponse;
        
        [System.Serializable] 
        public class OnResponseEvent : UnityEvent<string> { }

        private OpenAIApi openAI;
        private List<ChatMessage> messages = new List<ChatMessage>();

        private void Awake()
        {
            Configuration config = new Configuration();
            openAI = new OpenAIApi(config);
        }

        public async void AskChatGPT(AICharacter aiCharacter, string newText, Action<string> callback)
        {
            ChatMessage newMessage = new ChatMessage();
            newMessage.Content = newText;
            newMessage.Role = "user";

            messages.Add(newMessage);

            CreateChatCompletionRequest request = new CreateChatCompletionRequest();
            request.Messages = messages;
            request.Model = "gpt-3.5-turbo";

            var response = await openAI.CreateChatCompletion(request);

            if(response.Choices!=null && response.Choices.Count > 0)
            {
                var chatResponse = response.Choices[0].Message;
                messages.Add(chatResponse);

                Debug.Log(chatResponse.Content);

                OnResponse.Invoke(chatResponse.Content);
            }
        }
    }
}*/