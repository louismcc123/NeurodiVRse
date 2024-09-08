using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GroupDialogueManager : MonoBehaviour
{
    [Header("NPC Dialogue")]
    public List<GroupDialogueSequence> dialogueSequences;
    private int currentSequenceIndex = 0;
    private int currentDialogueIndex = 0;
    private int pausedSequenceIndex = -1;
    private int pausedDialogueIndex = -1;
    private bool isDialogueActive = false;
    private bool isDialoguePaused = false;

    [Header("Player Dialogue")]
    public GameObject PlayerResponseCanvas;
    public Transform playerResponseButtonParent;
    public GameObject responseButtonPrefab;

    [Header("Advice")]
    public GameObject AdviceCanvas;
    public TextMeshProUGUI adviceText;

    [Header("Scoring")]
    private int totalScore = 0;
    public TextMeshProUGUI finalScoreText;
    public PlayerStats playerStats;

    [SerializeField] private Transform player;
    public List<NPCBehaviours> npcBehaviours;

    private void Awake()
    {
        HideDialogue();
    }

    public void StartGroupDialogue()
    {
        if (dialogueSequences.Count > 0 && !isDialogueActive)
        {
            if (dialogueSequences[currentSequenceIndex].completed)
            {
                Debug.Log("Dialogue has already been completed.");
                return;
            }

            isDialogueActive = true;
            ShowDialogue();
            DisplayNextDialogue();
        }
    }

    public void PauseGroupDialogue()
    {
        if (isDialogueActive)
        {
            isDialoguePaused = true;
            isDialogueActive = false;
            HideDialogue();
            pausedSequenceIndex = currentSequenceIndex;
            pausedDialogueIndex = currentDialogueIndex;
        }
    }


    public void ResumeGroupDialogue()
    {
        if (isDialoguePaused)
        {
            isDialoguePaused = false;
            isDialogueActive = true;
            ShowDialogue();
            currentSequenceIndex = pausedSequenceIndex;
            currentDialogueIndex = pausedDialogueIndex;
            DisplayNextDialogue();
        }
    }


    public void EndGroupDialogue()
    {
        HideDialogue();
        isDialogueActive = false;
        dialogueSequences[currentSequenceIndex].completed = true;
        playerStats.DisplayFinalScore();
    }

    private void DisplayNextDialogue()
    {
        if (!isDialogueActive)
        {
            return;
        }

        GroupDialogueSequence currentSequence = dialogueSequences[currentSequenceIndex];

        if (currentDialogueIndex >= currentSequence.nodes.Count)
        {
            ShowPlayerResponses(currentSequence);
            return;
        }

        GroupDialogueNode currentNode = currentSequence.nodes[currentDialogueIndex];

        TriggerNodeDialogue(currentNode);

        foreach (var npc in npcBehaviours)
        {
            if (npc.transform != currentNode.actor.character)
            {
                npc.FaceSpeaker(currentNode.actor.character);
            }
        }

        PlayDialogueAudio(currentNode);
    }

    private void TriggerNodeDialogue(GroupDialogueNode node)
    {
        node.actor.NPCDialogueCanvas.SetActive(true);

        var titleText = node.actor.NPCDialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
        var bodyText = node.actor.NPCDialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();

        titleText.text = node.actor.Name;
        bodyText.text = node.dialogueText;

        node.StartTalking();
    }

    private void ShowPlayerResponses(GroupDialogueSequence sequence)
    {
        foreach (var npc in npcBehaviours)
        {
            npc.FaceSpeaker(player);
        }

        foreach (Transform child in playerResponseButtonParent)
        {
            child.gameObject.SetActive(false);
        }

        List<GroupDialogueResponse> shuffledResponses = new List<GroupDialogueResponse>(sequence.responses);
        ShuffleList(shuffledResponses);
        int responseCount = Mathf.Min(shuffledResponses.Count, 3);

        for (int i = 0; i < responseCount; i++)
        {
            GroupDialogueResponse response = shuffledResponses[i];
            GameObject buttonObj = Instantiate(responseButtonPrefab, playerResponseButtonParent);
            TextMeshProUGUI responseText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            Button button = buttonObj.GetComponent<Button>();

            responseText.text = response.responseText;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnResponseSelected(response));
        }
    }

    private void OnResponseSelected(GroupDialogueResponse response)
    {
        currentSequenceIndex = response.nextSequence;
        currentDialogueIndex = 0;
        DisplayNextDialogue();
    }

    private void HandlePlaybackComplete(GroupDialogueNode node)
    {
        node.StopTalking();
        currentDialogueIndex++;
        DisplayNextDialogue();
    }

    private void PlayDialogueAudio(GroupDialogueNode node)
    {
        if (node.dialogueAudio != null)
        {
            node.actor.audioSource.clip = node.dialogueAudio;
            node.actor.audioSource.Play();
            HandlePlaybackComplete(node);
        }
        else
        {
            HandlePlaybackComplete(node);
        }
    }

    private void ShowDialogue()
    {
        PlayerResponseCanvas.SetActive(true);
    }

    private void HideDialogue()
    {
        foreach (var canvas in FindObjectsOfType<GameObject>())
        {
            if (canvas.CompareTag("NPC Dialogue Canvas"))
            {
                canvas.SetActive(false);
            }
        }
        PlayerResponseCanvas.SetActive(false);
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
