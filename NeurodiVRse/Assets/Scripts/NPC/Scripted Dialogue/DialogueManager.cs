using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // NPC Dialogue
    public GameObject DialogueParent;
    public TextMeshProUGUI DialogueTitleText, DialogueBodyText;
    public GameObject responseButtonPrefab;
    public Transform responseButtonParent;
    private DialogueNode pausedNode;
    private string pausedTitle;

    // Player Responses
    public GameObject PlayerResponseCanvas; 
    public Transform playerResponseButtonParent; 

    // Advice
    public GameObject AdviceCanvas; 
    public TextMeshProUGUI adviceText; 

    // Scoring
    private int totalScore = 0;
    public TextMeshProUGUI finalScoreText;

    public PlayerStats playerStats;
    public BaristaController baristaController;

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

        foreach (Transform child in playerResponseButtonParent)
        {
            Destroy(child.gameObject);
        }

        List<DialogueResponse> shuffledResponses = new List<DialogueResponse>(node.responses);
        ShuffleList(shuffledResponses);

        foreach (DialogueResponse response in shuffledResponses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, playerResponseButtonParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title));
        }
    }

    public void SelectResponse(DialogueResponse response, string title)
    {
        totalScore += response.score;
        playerStats.SubtractScore(response.score);
        adviceText.text = response.adviceText;
        AdviceCanvas.SetActive(true);

        if (response.nextNode != null && !response.nextNode.IsLastNode())
        {
            if (response.responseText == "Card, please." || response.responseText == "Cash, please.")
            {
                PauseDialogue(response.nextNode, title);
                TriggerPaymentProcess(response.responseText);
            }
            else if (response.responseText == "Thank you." && title == "Barista 5")
            {
                PauseDialogue(response.nextNode, title);
                baristaController.StartCoffeePreparation();
            }
            else
            {
                StartDialogue(title, response.nextNode);
            }
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
        finalScoreText.text = "Final Score: " + totalScore; 
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

    private void PauseDialogue(DialogueNode nextNode, string title)
    {
        pausedNode = nextNode;
        pausedTitle = title;
        HideDialogue(); 
    }

    public void ResumeDialogue()
    {
        if (pausedNode != null)
        {
            StartDialogue(pausedTitle, pausedNode);
            pausedNode = null;
            pausedTitle = null;
        }
    }

    private void TriggerPaymentProcess(string paymentMethod)
    {
        if (paymentMethod == "Card, please.")
        {
            // instantiate card in players hand
            // wait for player to touch card reader with card
            Debug.Log("Processing card payment...");
        }
        else if (paymentMethod == "Cash, please.")
        {
            // instantiate cash in players hand
            // wait for player to place cash on counter or in servers hand
            Debug.Log("Processing cash payment...");
        }

        Invoke("PaymentCompleted", 3.0f);
    }

    private void PaymentCompleted()
    {
        baristaController.StartTakingPayment();
    }

    // Fisher-Yates shuffle algorithm
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
