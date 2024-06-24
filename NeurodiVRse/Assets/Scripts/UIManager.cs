using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public InputField inputField;
    public TMP_Text outputText;
    public OpenAIManager openAIManager;

    public void OnSendButtonClicked()
    {
        string userInput = inputField.text;
        StartCoroutine(openAIManager.GetOpenAIResponse(userInput, UpdateOutputText));
    }

    private void UpdateOutputText(string response)
    {
        outputText.text = response;
    }
}
