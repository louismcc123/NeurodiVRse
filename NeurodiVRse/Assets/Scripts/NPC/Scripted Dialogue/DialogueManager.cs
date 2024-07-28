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
    public GameObject cardPrefab;
    public Transform playerHandTransform;
    private GameObject instantiatedCard;

    // Audio
    private AudioSource baristaAudioSource;
    private AudioSource strangerAudioSource;
    private AudioSource playerAudioSource;

    private void Awake()
    {
        HideDialogue();
        baristaAudioSource = gameObject.AddComponent<AudioSource>();
        strangerAudioSource = gameObject.AddComponent<AudioSource>();
        playerAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartDialogue(string title, DialogueNode node)
    {
        ShowDialogue();
        DialogueTitleText.text = title;
        DialogueBodyText.text = node.dialogueText;
        adviceText.text = "";
        AdviceCanvas.SetActive(false);

        // Play NPC dialogue audio
        PlayNodeAudioClip(node.dialogueAudio);
        StartCoroutine(DisplayResponsesWithDelay(title, node));
    }

    private IEnumerator DisplayResponsesWithDelay(string title, DialogueNode node)
    {
        // Destroy old buttons
        foreach (Transform child in playerResponseButtonParent)
        {
            Destroy(child.gameObject);
        }

        // Wait for the duration of the dialogue audio or a fixed time before showing responses
        yield return new WaitForSeconds(node.dialogueAudio != null ? node.dialogueAudio.length : 1f);

        // Shuffle and display responses
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

        if (!string.IsNullOrEmpty(response.adviceText))
        {
            AdviceCanvas.SetActive(true);
        }
        else
        {
            AdviceCanvas.SetActive(false);
        }

        // Play response audio
        PlayResponseAudioClip(response.responseAudio);
        StartCoroutine(HandleResponseWithDelay(response, title));
    }

    private IEnumerator HandleResponseWithDelay(DialogueResponse response, string title)
    {
        // Wait for the response audio or a fixed time before proceeding
        yield return new WaitForSeconds(response.responseAudio != null ? response.responseAudio.length : 1f);

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

    private void PlayNodeAudioClip(AudioClip clip)
    {
        if (clip != null)
        {
            baristaAudioSource.clip = clip;
            baristaAudioSource.Play();
            strangerAudioSource.clip = clip;
            strangerAudioSource.Play();
        }
    }

    private void PlayResponseAudioClip(AudioClip clip)
    {
        if (clip != null)
        {
            playerAudioSource.clip = clip;
            playerAudioSource.Play();
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
            Debug.Log("Processing card payment...");
            InstantiateCard();
        }
        else if (paymentMethod == "Cash, please.")
        {
            Debug.Log("Processing cash payment...");
            // Instantiate cash in player's hand and wait for player to hand cash to barista
        }
    }

    private void InstantiateCard()
    {
        if (instantiatedCard == null && cardPrefab != null && playerHandTransform != null)
        {
            instantiatedCard = Instantiate(cardPrefab, playerHandTransform.position, playerHandTransform.rotation, playerHandTransform);
        }
    }

    public void OnCardTapped()
    {
        Destroy(instantiatedCard);
        PaymentCompleted();
    }

    private void PaymentCompleted()
    {
        baristaController.StartTakingPayment();
    }

    public void OnBaristaAtTill()
    {
        ResumeDialogue();
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
