using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("NPC Dialogue")]
    public GameObject NPCDialogueCanvas;
    public GameObject DialogueParent;
    public TextMeshProUGUI DialogueTitleText, DialogueBodyText;
    private DialogueNode pausedNode;
    private DialogueNode currentDialogueNode;
    private string pausedTitle;
    private bool isDialoguePaused = false;
    private bool isConversationFinished = false;

    [Header("Player Responses")]
    public GameObject PlayerResponseCanvas;
    public Transform playerResponseButtonParent;
    public GameObject responseButtonPrefab;

    [Header("Advice")]
    public GameObject adviceCanvas;
    public TextMeshProUGUI adviceText;

    [Header("Scoring")]
    public PlayerStats playerStats;
    public TextMeshProUGUI finalScoreText;
    private int totalScore = 0;

    [Header("Audio")]
    private AudioSource playerAudioSource;

    [Header("NPC Control")]
    public CharacterController characterController;
    private Animator animator;

    private void Awake()
    {
        HideDialogue();
        playerAudioSource = gameObject.AddComponent<AudioSource>();

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError(gameObject.name + ": Animator component not found on any child GameObject.");
        }
        else
        {
            animator.applyRootMotion = false;
        }
    }

    public void StartDialogue(string title, DialogueNode node)
    {
        if (currentDialogueNode == node)
        {
            Debug.Log(gameObject.name + ": Dialogue is already active for this node. Ignoring StartDialogue call.");
            return;
        }

        isDialoguePaused = false;
        isConversationFinished = false;
        Debug.Log(gameObject.name + ": Starting dialogue: " + node.dialogueText);
        ShowDialogue();
        DialogueBodyText.text = node.dialogueText;
        DialogueTitleText.text = title;
        adviceText.text = "";
        adviceCanvas.SetActive(false);
        NPCDialogueCanvas.SetActive(true);

        animator.SetBool("IsTalking", true);
        PlayNodeAudioClip(node.dialogueAudio);
        StartCoroutine(DisplayResponsesWithDelay(title, node));

        currentDialogueNode = node;
    }

    public void EndDialogue()
    {
        HideDialogue();
        isConversationFinished = true;
        currentDialogueNode = null;
        animator.SetBool("IsTalking", false);
        playerStats.DisplayFinalScore(finalScoreText);
        this.enabled = false;
    }

    private IEnumerator DisplayResponsesWithDelay(string title, DialogueNode node)
    {
        foreach (Transform child in playerResponseButtonParent)
        {
            child.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(node.dialogueAudio != null ? node.dialogueAudio.length : 1f);

        animator.SetBool("IsTalking", false);

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
                Debug.LogError(gameObject.name + ": TextMeshProUGUI component not found in the button prefab.");
            }
            else
            {
                responseText.text = response.responseText;
                Debug.Log(gameObject.name + ": Set response text to: " + response.responseText);
            }

            Button button = buttonObj.GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError(gameObject.name + ": Button component not found in the button prefab.");
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

        Debug.Log(gameObject.name + ": Responses updated.");
    }

    public void SelectResponse(DialogueResponse response, string title)
    {
        playerStats.totalScore += response.score;
        playerStats.SubtractScore(response.score);
        adviceText.text = response.adviceText;

        Debug.Log(gameObject.name + ": Response selected: " + response.responseText);
        Debug.Log(gameObject.name + ": Total Score: " + totalScore);

        if (!string.IsNullOrEmpty(response.adviceText))
        {
            adviceCanvas.SetActive(true);
        }
        else
        {
            adviceCanvas.SetActive(false);
        }

        PlayResponseAudioClip(response.responseAudio);

        foreach (Transform child in playerResponseButtonParent)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                //button.interactable = false;
            }
        }

        StartCoroutine(HandleResponseWithDelay(response, title));
    }

    private IEnumerator HandleResponseWithDelay(DialogueResponse response, string title)
    {
        if (response.responseAudio != null)
        {
            Debug.Log(gameObject.name + ": Waiting for audio clip to finish: " + response.responseAudio.name);
            yield return new WaitForSeconds(response.responseAudio.length);
        }
        else
        {
            Debug.Log(gameObject.name + ": No audio clip, waiting default time.");
            yield return new WaitForSeconds(1f);
        }

        animator.SetBool("IsTalking", false);

        if (response.nextNode != null && !response.nextNode.IsLastNode())
        {
            Debug.Log(gameObject.name + ": Transitioning to next node: " + response.nextNode.dialogueText);
            HandleNextNode(response, title);
        }
        else
        {
            Debug.Log(gameObject.name + ": Ending dialogue. Displaying final score.");
            EndDialogue();
        }
    }

    protected virtual void HandleNextNode(DialogueResponse response, string title)
    {
        StartDialogue(title, response.nextNode);
    }

    private void PlayNodeAudioClip(AudioClip clip)
    {
        if (clip != null)
        {
            playerAudioSource.clip = clip;
            playerAudioSource.Play();
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

    private void ShowDialogue()
    {
        DialogueParent.SetActive(true);
        PlayerResponseCanvas.SetActive(true);
    }

    public void HideDialogue()
    {
        NPCDialogueCanvas.SetActive(false);
        DialogueParent.SetActive(false);
        PlayerResponseCanvas.SetActive(false);
        adviceCanvas.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
    }

    public void PauseDialogue(DialogueNode nextNode, string title)
    {
        pausedNode = nextNode;
        pausedTitle = title;
        isDialoguePaused = true;
        HideDialogue();
    }

    public void ResumeDialogue()
    {
        Debug.Log(gameObject.name + ": Resume dialogue called");

        if (pausedNode != null)
        {
            isDialoguePaused = false;
            Debug.Log(gameObject.name + ": Resuming dialogue with node: " + pausedNode.dialogueText);
            StartDialogue(pausedTitle, pausedNode);
            pausedNode = null;
            pausedTitle = null;
        }
    }

    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf || PlayerResponseCanvas.activeSelf;
    }
    public bool IsDialoguePaused()
    {
        return isDialoguePaused;
    }

    public bool IsConversationFinished()
    {
        return isConversationFinished;
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
