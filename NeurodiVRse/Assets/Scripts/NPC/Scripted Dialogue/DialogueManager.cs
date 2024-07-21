using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Dialogue UI
    public GameObject DialogueParent;
    public GameObject PlayerResponseCanvas; 
    public GameObject responseButtonPrefab;
    public Transform responseButtonParent;
    public TextMeshProUGUI DialogueTitleText, DialogueBodyText;

    // Scoring
    public TextMeshProUGUI finalScoreText;
    private int totalScore = 0;

    // Advice
    public GameObject AdviceCanvas; 
    public TextMeshProUGUI adviceText;

    public PlayerStats playerStats;

    private void Awake()
    {
        HideDialogue();
    }

    public void StartDialogue(string title, DialogueNode node)
    {
        ShowDialogue();

        DialogueTitleText.text = title;
        DialogueBodyText.text = node.dialogueText;
        adviceText.text = ""; 
        AdviceCanvas.SetActive(false);

        foreach (Transform child in responseButtonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in responseButtonParent)
        {
            Destroy(child.gameObject);
        }

        List<DialogueResponse> shuffledResponses = new List<DialogueResponse>(node.responses);
        ShuffleList(shuffledResponses);

        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title));
        }
    }

    public void SelectResponse(DialogueResponse response, string title)
    {
        totalScore += response.score; 
        playerStats.SubtactScore(response.score);
        adviceText.text = response.adviceText;
        AdviceCanvas.SetActive(true);

        if (response.nextNode != null && !response.nextNode.IsLastNode())
        {
            StartDialogue(title, response.nextNode);
        }
        else
        {
            DisplayFinalScore(); 
            HideDialogue();
        }
    }

    public void DisplayFinalScore()
    {
        PlayerResponseCanvas.SetActive(false); 
        finalScoreText.text = "Final Score: " + totalScore + "/100"; 
        finalScoreText.gameObject.SetActive(true); 
    }

    public void HideDialogue()
    {
        DialogueParent.SetActive(false);
        PlayerResponseCanvas.SetActive(false);
        AdviceCanvas.SetActive(false);
        finalScoreText.gameObject.SetActive(false); 
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

    private void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}