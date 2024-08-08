using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LLM
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject openAICanvas;
        [SerializeField] private GameObject npcCanvas;
        [SerializeField] private GameObject keyboard;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_Text outputText;
        [SerializeField] private OpenAIManager openAIManager;
        [SerializeField] private AICharacter aiCharacter; // Add this line

        private void OnEnable()
        {
            openAIManager.OnResponse.AddListener(UpdateOutputText);
            inputField.onSelect.AddListener(OnInputFieldSelected);
        }

        private void OnDisable()
        {
            openAIManager.OnResponse.RemoveListener(UpdateOutputText);
            inputField.onSelect.RemoveListener(OnInputFieldSelected);
        }

        public void OnSendButtonClicked()
        {
            string userInput = inputField.text;
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Debug.LogWarning("User input is empty.");
                return;
            }

            outputText.text = "Loading...";
            openAIManager.AskChatGPT(aiCharacter, userInput, UpdateOutputText); // Modify this line
        }

        public void OnCloseButtonClicked()
        {
            openAICanvas.SetActive(false);
            npcCanvas.SetActive(false);
        }

        private void UpdateOutputText(string response)
        {
            outputText.text = response;
        }

        private void OnInputFieldSelected(string text)
        {
            keyboard.SetActive(true);
        }

        public void OnKeyboardCloseButtonClicked()
        {
            keyboard.SetActive(false);
        }
    }
}
