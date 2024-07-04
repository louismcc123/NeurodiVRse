using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenAI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject npcCanvas;

    public InputField inputField;
    public TMP_Text outputText;
    public OpenAIManager openAIManager;

    public void OnSendButtonClicked()
    {
        string userInput = inputField.text;
        openAIManager.AskChatGPT(userInput);
    }
    public void OnCloseButtonClicked()
    {
        openAICanvas.SetActive(false);
        npcCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        openAIManager.OnResponse.AddListener(UpdateOutputText);
    }

    private void OnDisable()
    {
        openAIManager.OnResponse.RemoveListener(UpdateOutputText);
    }

    private void UpdateOutputText(string response)
    {
        outputText.text = response;
    }
}
