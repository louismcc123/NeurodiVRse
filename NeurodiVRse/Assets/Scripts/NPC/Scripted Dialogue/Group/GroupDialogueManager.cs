/*using System.Collections;
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

    /*[Header("Scoring")]
    private int totalScore = 0;
    public TextMeshProUGUI finalScoreText;
    public PlayerStats playerStats;*/
/*
    [SerializeField] private Transform player;
    public List<GroupActor> groupActors;

    private void Awake()
    {
        foreach (var actor in groupActors)
        {
            if (actor == null)
            {
                Debug.LogError("One or more actors are not assigned in the groupActors list.");
            }
            else
            {
                Debug.Log($"Actor initialized: {actor.Name}");
            }
        }

        if (dialogueSequences == null || dialogueSequences.Count == 0)
        {
            Debug.LogError("Dialogue sequences not initialized or empty.");
        }

        HideDialogue();
    }

    public void StartGroupDialogue()
    {
        Debug.Log("StartGroupDialogue called. Dialogue sequences count: " + dialogueSequences.Count);

        if (dialogueSequences.Count > 0 && !isDialogueActive)
        {
            if (dialogueSequences[currentSequenceIndex].completed)
            {
                Debug.Log("Dialogue has already been completed.");
                return;
            }

            isDialogueActive = true;
            ShowDialogue();
            Debug.Log("Displaying next dialogue.");
            DisplayNextDialogue();
        }
        else
        {
            Debug.LogWarning("No dialogue sequences available or dialogue is already active.");
        }
    }

    public void PauseGroupDialogue()
    {
        Debug.Log("PauseGroupDialogue called.");

        if (isDialogueActive)
        {
            isDialoguePaused = true;
            isDialogueActive = false;
            HideDialogue();
            pausedSequenceIndex = currentSequenceIndex;
            pausedDialogueIndex = currentDialogueIndex;
            Debug.Log($"Dialogue paused. Sequence Index: {pausedSequenceIndex}, Dialogue Index: {pausedDialogueIndex}");
        }
        else
        {
            Debug.LogWarning("Dialogue is not active, cannot pause.");
        }
    }

    public void ResumeGroupDialogue()
    {
        Debug.Log("ResumeGroupDialogue called.");

        if (isDialoguePaused)
        {
            Debug.Log("Dialogue is paused. Resuming dialogue.");

            isDialoguePaused = false;
            isDialogueActive = true;
            ShowDialogue();
            currentSequenceIndex = pausedSequenceIndex;
            currentDialogueIndex = pausedDialogueIndex;
            Debug.Log($"Resuming dialogue. Sequence Index: {currentSequenceIndex}, Dialogue Index: {currentDialogueIndex}");
            DisplayNextDialogue();
        }
        else
        {
            Debug.LogWarning("Dialogue is not paused, cannot resume.");
        }
    }

    public void EndGroupDialogue()
    {
        HideDialogue();
        isDialogueActive = false;
        dialogueSequences[currentSequenceIndex].completed = true;
        //playerStats.DisplayFinalScore();
    }

    private void DisplayNextDialogue()
    {
        Debug.Log("Entering DisplayNextDialogue.");

        if (!isDialogueActive)
        {
            Debug.LogWarning("Dialogue is not active.");
            return;
        }

        if (dialogueSequences == null || dialogueSequences.Count == 0)
        {
            Debug.LogError("Dialogue sequences are null or empty.");
            return;
        }

        if (currentSequenceIndex >= dialogueSequences.Count || currentSequenceIndex < 0)
        {
            Debug.LogError($"Invalid sequence index: {currentSequenceIndex}");
            return;
        }

        GroupDialogueSequence currentSequence = dialogueSequences[currentSequenceIndex];
        Debug.Log($"Displaying next dialogue. Current sequence index: {currentSequenceIndex}, Dialogue index: {currentDialogueIndex}");

        if (currentSequence == null)
        {
            Debug.LogError("Current sequence is null.");
            return;
        }

        if (currentDialogueIndex < 0 || currentDialogueIndex >= currentSequence.nodes.Count)
        {
            Debug.LogError($"Invalid dialogue index: {currentDialogueIndex}");
            return;
        }

        if (currentDialogueIndex >= currentSequence.nodes.Count || currentDialogueIndex < 0)
        {
            Debug.LogWarning("Current dialogue index out of range.");
            ShowPlayerResponses(currentSequence);
            return;
        }

        GroupDialogueNode currentNode = currentSequence.nodes[currentDialogueIndex];
        if (currentNode == null)
        {
            Debug.LogError("Current node is null.");
            return;
        }

        Debug.Log($"Processing node: {currentNode.name}");
        //AssignActorToNode(currentNode);
        TriggerNodeDialogue(currentNode);
        MakeNPCsFaceSpeaker(currentNode.actor.character);
        PlayDialogueAudio(currentNode);
    }

    /*private void AssignActorToNode(GroupDialogueNode node)
    {
        Debug.Log($"Assigning actor for node: {node.dialogueText}");

        if (node == null)
        {
            Debug.LogError("Node is null!");
            return;
        }

        if (node.actor == null)
        {
            Debug.LogError("Node's actor is null!");
            return;
        }

        foreach (var actor in groupActors)
        {
            if (actor.Name == node.actor.Name)
            {
                node.actor = actor.GetComponent<GroupActor>();
                Debug.Log($"Actor assigned: {node.actor.Name}");
                break;
            }
        }
    }*/
/*
    private void TriggerNodeDialogue(GroupDialogueNode node)
    {
        HideDialogue();

        if (node.actor != null)
        {
            node.actor.NPCDialogueCanvas.SetActive(true);

            var titleText = node.actor.NPCDialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
            var bodyText = node.actor.NPCDialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();

            titleText.text = node.actor.Name;
            bodyText.text = node.dialogueText;

            NPCBehaviours npcBehaviours = node.actor.GetComponent<NPCBehaviours>();
            if (npcBehaviours != null)
            {
                npcBehaviours.StartTalking();
            }
            else
            {
                Debug.LogError("NPCBehaviours component is missing on the actor's GameObject.");
            }
        }
        else
        {
            Debug.LogError("Node's actor is null!");
        }
    }

    private void MakeNPCsFaceSpeaker(Transform speaker)
    {
        foreach (var actor in groupActors)
        {
            if (actor.transform != speaker)
            {
                NPCBehaviours npcBehaviours = actor.GetComponent<NPCBehaviours>();
                if (npcBehaviours != null)
                {
                    npcBehaviours.FaceSpeaker(speaker);
                }
            }
        }
    }

    private void ShowPlayerResponses(GroupDialogueSequence sequence)
    {
        MakeNPCsFaceSpeaker(player);

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
        Debug.Log($"Playback complete for node: {node.dialogueText}");
        node.actor.GetComponent<NPCBehaviours>().StopTalking();
        currentDialogueIndex++;
        DisplayNextDialogue();
    }

    private void PlayDialogueAudio(GroupDialogueNode node)
    {
        if (node.dialogueAudio != null)
        {
            Debug.Log($"Playing audio for node: {node.dialogueText}");
            node.actor.audioSource.clip = node.dialogueAudio;
            node.actor.audioSource.Play();
            HandlePlaybackComplete(node);
        }
        else
        {
            Debug.Log("No audio for this dialogue node.");
            HandlePlaybackComplete(node);
        }
    }

    private void ShowDialogue()
    {
        Debug.Log("Showing dialogue.");
        PlayerResponseCanvas.SetActive(true);
    }

    private void HideDialogue()
    {
        Debug.Log("Hiding dialogue.");
        foreach (var actor in groupActors)
        {
            actor.NPCDialogueCanvas.SetActive(false);
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GroupDialogueManager : MonoBehaviour
{
    [Header("NPC Dialogue")]
    public List<NPCNodeManager> nodeManagers;
    public List<GroupDialogueSequence> allSequences;

    [Header("Player Dialogue")]
    public GameObject PlayerDialogueCanvas;
    public Transform playerResponseButtonParent;
    public GameObject responseButtonPrefab;

    private bool isDialogueActive = false;

    private GroupDialogueNode currentNode;
    private GroupDialogueNode lastNode;
    private GroupDialogueSequence currentSequence;

    private void Awake()
    {
        HideDialogue();
    }

    public void StartGroupDialogue(GroupDialogueSequence sequence)
    {
        if (isDialogueActive)
        {
            Debug.LogWarning("Dialogue is already active. Ending current dialogue.");
            EndGroupDialogue();
        }

        currentSequence = sequence;
        isDialogueActive = true;
        StartCoroutine(PlaySequence(sequence));
    }

    private IEnumerator PlaySequence(GroupDialogueSequence sequence)
    {
        bool startFromCurrentNode = currentNode != null;

        if (sequence == null)
        {
            Debug.LogError("GroupDialogueSequence is null");
            yield break;
        }

        foreach (GroupDialogueNode node in sequence.nodes)
        {
            if (startFromCurrentNode && node != currentNode)
            {
                continue;
            }

            startFromCurrentNode = false;
            currentNode = node;
            if (currentNode != null)
            {
                Debug.Log("currentNode is null");
            }

            yield return StartCoroutine(PlayNode(node));
        }

        Debug.Log("All nodes processed. Showing player responses.");
        ShowPlayerResponses(sequence.responses);
    }

    private IEnumerator PlayNode(GroupDialogueNode node)
    {
        NPCNodeManager nodeManager = FindNodeManagerForNode(node);

        if (nodeManager == null)
        {
            Debug.LogError("No NPCNodeManager found for node: " + node.name);
            yield break;
        }

        GroupActor actor = nodeManager.GetComponent<GroupActor>();
        if (actor == null)
        {
            Debug.LogError("GroupActor script not found on NPCNodeManager: " + nodeManager.name);
            yield break;
        }

        currentNode = node;

        NPCBehaviours npcBehaviours = nodeManager.GetComponent<NPCBehaviours>();
        if (npcBehaviours == null)
        {
            Debug.LogError("NPCBehaviours script not found on actor: " + nodeManager.name);
            yield break;
        }

        npcBehaviours.StartTalking();
        actor.audioSource.clip = node.dialogueAudio;
        actor.audioSource.Play();

        actor.NPCDialogueCanvas.SetActive(true);
        actor.DialogueTitleText.text = node.character;
        actor.DialogueBodyText.text = node.dialogueText;

        yield return new WaitForSeconds(node.dialogueAudio.length + 1f);

        npcBehaviours.StopTalking();
        actor.NPCDialogueCanvas.SetActive(false);
    }

    private NPCNodeManager FindNodeManagerForNode(GroupDialogueNode node)
    {
        foreach (NPCNodeManager manager in nodeManagers)
        {
            if (manager.nodes.Contains(node))
            {
                return manager;
            }
        }

        return null;
    }

    private void ShowPlayerResponses(List<GroupDialogueResponse> responses)
    {
        if (responses == null || responses.Count == 0)
        {
            Debug.LogWarning("No player responses to show.");
            return;
        }

        Debug.Log("Showing player responses."); 
        PlayerDialogueCanvas.SetActive(true);

        foreach (Transform child in playerResponseButtonParent)
        {
            child.gameObject.SetActive(false);
        }

        List<GroupDialogueResponse> shuffledResponses = new List<GroupDialogueResponse>(responses);
        ShuffleList(shuffledResponses);
        int responseCount = Mathf.Min(shuffledResponses.Count, 3);

        for (int i = 0; i < responseCount; i++)
        {
            GroupDialogueResponse response = shuffledResponses[i];
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
            responseText.text = response.responseText;

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SelectResponse(response));
        }

        for (int i = responseCount; i < playerResponseButtonParent.childCount; i++)
        {
            playerResponseButtonParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SelectResponse(GroupDialogueResponse response)
    {
        PlayerDialogueCanvas.SetActive(false);

        if (response.nextSequence >= 0 && response.nextSequence < allSequences.Count)
        {
            GroupDialogueSequence nextSequence = allSequences[response.nextSequence];
            StartGroupDialogue(nextSequence);
        }
        else
        {
            EndGroupDialogue();
        }
    }

    private void HideDialogue()
    {
        foreach (NPCNodeManager manager in nodeManagers)
        {
            GroupActor actor = manager.GetComponent<GroupActor>();
            if (actor != null)
            {
                actor.NPCDialogueCanvas.SetActive(false);


            }
        }

        PlayerDialogueCanvas.SetActive(false);
    }


    public void ResumeGroupDialogue()
    {
        if (lastNode != null)
        {
            StartCoroutine(PlayNode(lastNode));
            lastNode = null;
        }
        /*else
        {
            StartGroupDialogue(initialSequence);
        }*/
    }

    public void PauseGroupDialogue()
    {
        HideDialogue();

        if (currentNode != null)
        {
            lastNode = currentNode;
            HideDialogue();

            NPCNodeManager nodeManager = FindNodeManagerForNode(currentNode);

            if (nodeManager == null)
            {
                Debug.LogError("No NPCNodeManager found for node: " + currentNode.name);
            }

            NPCBehaviours npcBehaviours = nodeManager.GetComponent<NPCBehaviours>();
            if (npcBehaviours == null)
            {
                Debug.LogError("NPCBehaviours script not found on actor: " + nodeManager.name);
            }

            npcBehaviours.StopTalking();
        }
    }

    public void EndGroupDialogue()
    {
        HideDialogue();
        isDialogueActive = false;
        currentNode = null;
        currentSequence = null;
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
