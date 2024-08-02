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
    private DialogueNode pausedNode;
    private DialogueNode currentDialogueNode;
    private string pausedTitle;
    private bool isDialogueActive = false;
    private bool isDialoguePaused = false;

    // Player Responses
    public GameObject PlayerResponseCanvas;
    public Transform playerResponseButtonParent;
    public GameObject responseButtonPrefab;

    // Advice
    public GameObject AdviceCanvas;
    public TextMeshProUGUI adviceText;

    // Scoring
    private int totalScore = 0;
    public TextMeshProUGUI finalScoreText;

    // Payment
    public CardManager cardManager;
    public CashManager cashManager;

    // Coffee Prep
    public Transform coffeeCupSpawnPosition;
    public GameObject coffeeCupPrefab;
    public GameObject risingSteam;

    // Audio
    private AudioSource baristaAudioSource;
    private AudioSource strangerAudioSource;
    private AudioSource playerAudioSource;

    public PlayerStats playerStats;
    public BaristaController baristaController;

    private void Awake()
    {
        HideDialogue();
        baristaAudioSource = gameObject.AddComponent<AudioSource>();
        strangerAudioSource = gameObject.AddComponent<AudioSource>();
        playerAudioSource = gameObject.AddComponent<AudioSource>();
    }
    public void StartDialogue(string title, DialogueNode node)
    {
        if (currentDialogueNode == node)
        {
            Debug.Log("Dialogue is already active for this node. Ignoring StartDialogue call.");
            return;
        }

        isDialogueActive = true;
        isDialoguePaused = false;
        Debug.Log("Starting dialogue: " + node.dialogueText);
        ShowDialogue();
        DialogueBodyText.text = node.dialogueText;
        DialogueTitleText.text = title;
        adviceText.text = "";
        AdviceCanvas.SetActive(false);

        PlayNodeAudioClip(node.dialogueAudio);
        StartCoroutine(DisplayResponsesWithDelay(title, node));

        currentDialogueNode = node;
    }

    private IEnumerator DisplayResponsesWithDelay(string title, DialogueNode node)
    {
        foreach (Transform child in playerResponseButtonParent)
        {
            child.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(node.dialogueAudio != null ? node.dialogueAudio.length : 1f);

        List<DialogueResponse> shuffledResponses = new List<DialogueResponse>(node.responses);
        ShuffleList(shuffledResponses);
        int responseCount = Mathf.Min(shuffledResponses.Count, 3);

        for (int i = 0; i < responseCount; i++)
        {
            DialogueResponse response = shuffledResponses[i];
            GameObject buttonObj;

            if (i < playerResponseButtonParent.childCount)
            {
                buttonObj = playerResponseButtonParent.GetChild(i).gameObject;
                buttonObj.SetActive(true);
            }
            else
            {
                buttonObj = Instantiate(responseButtonPrefab, playerResponseButtonParent);
            }

            TextMeshProUGUI responseText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (responseText == null)
            {
                Debug.LogError("TextMeshProUGUI component not found in the button prefab.");
            }
            else
            {
                responseText.text = response.responseText;
                Debug.Log("Set response text to: " + response.responseText);
            }

            Button button = buttonObj.GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("Button component not found in the button prefab.");
            }
            else
            {
                button.onClick.RemoveAllListeners(); 
                button.onClick.AddListener(() => SelectResponse(response, title));
            }
        }

        for (int i = responseCount; i < playerResponseButtonParent.childCount; i++)
        {
            playerResponseButtonParent.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("Responses updated.");
    }

    public void SelectResponse(DialogueResponse response, string title)
    {
        totalScore += response.score;
        playerStats.SubtractScore(response.score);
        adviceText.text = response.adviceText;

        Debug.Log("Response selected: " + response.responseText);
        Debug.Log("Total Score: " + totalScore);

        if (!string.IsNullOrEmpty(response.adviceText))
        {
            AdviceCanvas.SetActive(true);
        }
        else
        {
            AdviceCanvas.SetActive(false);
        }

        PlayResponseAudioClip(response.responseAudio);
        StartCoroutine(HandleResponseWithDelay(response, title));
    }

    private IEnumerator HandleResponseWithDelay(DialogueResponse response, string title)
    {
        if (response.responseAudio != null)
        {
            Debug.Log("Waiting for audio clip to finish: " + response.responseAudio.name);
            yield return new WaitForSeconds(response.responseAudio.length);
        }
        else
        {
            Debug.Log("No audio clip, waiting default time.");
            yield return new WaitForSeconds(1f);
        }

        if (response.nextNode != null && !response.nextNode.IsLastNode())
        {
            Debug.Log("Transitioning to next node: " + response.nextNode.dialogueText);

            if (response.responseText == "Card, please.")
            {
                PauseDialogue(response.nextNode, title);
                cardManager.InstantiateCard();
            }
            else if (response.responseText == "Cash, please.")
            {
                PauseDialogue(response.nextNode, title);
                cashManager.InstantiateCash();
            }
            else if (response.responseText == "Thank you.")
            {
                PauseDialogue(response.nextNode, title);
                StartCoroutine(StartCoffeePreparationSequence());
            }
            else
            {
                StartDialogue(title, response.nextNode);
            }
        }
        else
        {
            Debug.Log("Ending dialogue. Displaying final score.");
            DisplayFinalScore();
            HideDialogue();
            isDialogueActive = false;
            currentDialogueNode = null;
        }
    }

    private IEnumerator StartCoffeePreparationSequence()
    {
        baristaController.MoveToWaypoint(2);
        yield return new WaitUntil(() => !baristaController.IsMoving());
        StartCoroutine(StartCoffeePreparation());
        yield return new WaitForSeconds(8f);
        baristaController.MoveToWaypoint(3);
        yield return new WaitUntil(() => !baristaController.IsMoving());
        Instantiate(coffeeCupPrefab, coffeeCupSpawnPosition.position, coffeeCupSpawnPosition.rotation);
        Debug.Log("Coffee cup instantiated. now dialogue should resume");
        ResumeDialogue();
    }

    private IEnumerator StartCoffeePreparation()
    {
        Debug.Log("StartCoffeePreparation");

        risingSteam.SetActive(true);
        yield return new WaitForSeconds(5f);
        risingSteam.SetActive(false);
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

    private void ShowDialogue()
    {
        DialogueParent.SetActive(true);
        PlayerResponseCanvas.SetActive(true);
    }

    public void HideDialogue()
    {
        DialogueParent.SetActive(false);
        PlayerResponseCanvas.SetActive(false);
        AdviceCanvas.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
    }
    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf || PlayerResponseCanvas.activeSelf;
    }

    private void PauseDialogue(DialogueNode nextNode, string title)
    {
        pausedNode = nextNode;
        pausedTitle = title;
        isDialoguePaused = true;
        HideDialogue();
    }

    public void ResumeDialogue()
    {
        Debug.Log("Resume dialogue called");

        if (pausedNode != null)
        {
            isDialoguePaused = false;
            isDialogueActive = true;
            Debug.Log("Resuming dialogue with node: " + pausedNode.dialogueText);
            StartDialogue(pausedTitle, pausedNode);
            pausedNode = null;
            pausedTitle = null;
        }
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