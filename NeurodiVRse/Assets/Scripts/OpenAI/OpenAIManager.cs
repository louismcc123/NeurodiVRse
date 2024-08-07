using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LLM
{
    public class OpenAIManager : MonoBehaviour
    {
        public AICharacter aiCharacter;
        public OnResponseEvent OnResponse;

        [System.Serializable]
        public class OnResponseEvent : UnityEvent<string> { }

        private OpenAIApi openAIApi;
        private List<ChatMessage> messages = new List<ChatMessage>();

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

            if (aiCharacter != null)
            {
                var introductionMessage = new ChatMessage
                {
                    Role = "system",
                    Content = $"{aiCharacter.introductionText}\nBackground: {aiCharacter.background}\nPersonality Traits: {aiCharacter.personalityTraits}\nBehaviors: {string.Join(", ", aiCharacter.behaviors)}"
                };
                messages.Add(introductionMessage);
            }
        }

        public async void AskChatGPT(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                Debug.LogWarning("Input text is empty.");
                return;
            }

            var newMessage = new ChatMessage { Role = "user", Content = $"{aiCharacter.roleName}: {newText}" };
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
