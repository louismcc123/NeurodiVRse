using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{    
    [SerializeField] private GameObject canvas;

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
        canvas.SetActive(false);
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
