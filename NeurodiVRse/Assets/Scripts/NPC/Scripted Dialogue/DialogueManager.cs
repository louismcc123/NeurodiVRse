using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueParent;
    public TextMeshProUGUI DialogueTitleText, DialogueBodyText;
    public GameObject PlayerResponseCanvas; 
    public Transform responseButtonParent;
    public GameObject responseButtonPrefab;

    private void Awake()
    {
        HideDialogue();
    }

    public void StartDialogue(string title, DialogueNode node)
    {
        ShowDialogue();

        DialogueTitleText.text = title;
        DialogueBodyText.text = node.dialogueText;

        foreach (Transform child in responseButtonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title));
        }
    }

    public void SelectResponse(DialogueResponse response, string title)
    {
        if (!response.nextNode.IsLastNode())
        {
            StartDialogue(title, response.nextNode);
        }
        else
        {
            HideDialogue();
        }
    }

    public void HideDialogue()
    {
        DialogueParent.SetActive(false);
        PlayerResponseCanvas.SetActive(false);
    }

    private void ShowDialogue()
    {
        DialogueParent.SetActive(true);
        PlayerResponseCanvas.SetActive(true);
    }

    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf || PlayerResponseCanvas.activeSelf;
    }
}
