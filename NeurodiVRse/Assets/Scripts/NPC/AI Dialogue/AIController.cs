/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLM
{
    public class AIController : MonoBehaviour
    {
        public AICharacter aiCharacter; // Reference to the AICharacter ScriptableObject
        private OpenAIManager openAIManager;

        private void Start()
        {
            // Find the OpenAIManager in the scene
            openAIManager = FindObjectOfType<OpenAIManager>();
        }

        public void Interact(string userInput)
        {
            if (openAIManager != null && aiCharacter != null)
            {
                openAIManager.AskChatGPT(aiCharacter, userInput, OnResponseReceived);
            }
            else
            {
                Debug.LogError("OpenAIManager or AICharacter is not assigned.");
            }
        }

        private void OnResponseReceived(string response)
        {
            // Handle the response, e.g., display it in the UI or make the NPC speak
            Debug.Log(response);
        }
    }
}*/