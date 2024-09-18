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

        if (response.nextSequence != null)
        {
            GroupDialogueSequence nextSequence = response.nextSequence;
            StartGroupDialogue(nextSequence);
        }
        else
        {
            Debug.Log("no next sequence set. ending dialogue");
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
