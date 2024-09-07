using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GroupDialogueManager : MonoBehaviour
{
    [Header("Player Dialogue")]
    public GameObject PlayerResponseCanvas;
    public Transform playerResponseButtonParent;
    public GameObject responseButtonPrefab;

    [Header("Dialogue Data")]
    public List<GroupDialogueNode> dialogueSequence;
    private int currentDialogueIndex = 0;

    private bool isDialogueActive = false;

    private void Start()
    {
        HideDialogue();
    }

    public void StartGroupDialogue()
    {
        if (dialogueSequence.Count > 0)
        {
            isDialogueActive = true;
            ShowDialogue();
            DisplayNextDialogue();
        }
    }

    private void DisplayNextDialogue()
    {
        if (currentDialogueIndex >= dialogueSequence.Count)
        {
            EndDialogue();
            return;
        }

        GroupDialogueNode node = dialogueSequence[currentDialogueIndex];
        UpdateCanvasForCurrentDialogueNode(node);
        DisplayResponses(node);
    }

    private void UpdateCanvasForCurrentDialogueNode(GroupDialogueNode node)
    {
        // Deactivate all NPC canvases if needed
        foreach (var canvas in FindObjectsOfType<GameObject>()) // Find all canvases in scene
        {
            if (canvas.CompareTag("NPCDialogueCanvas"))
            {
                canvas.SetActive(false);
            }
        }

        // Activate the canvas for the speaking NPC
        if (node.NPCDialogueCanvas != null)
        {
            node.NPCDialogueCanvas.SetActive(true);

            // Update the UI elements on the canvas
            var titleText = node.NPCDialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
            var bodyText = node.NPCDialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();

            titleText.text = "NPC"; // Set the title or use a specific title if needed
            bodyText.text = node.dialogueText;
        }
    }

    private void DisplayResponses(GroupDialogueNode node)
    {
        foreach (Transform child in playerResponseButtonParent)
        {
            child.gameObject.SetActive(false);
        }

        List<DialogueResponse> shuffledResponses = new List<DialogueResponse>(node.responses);
        ShuffleList(shuffledResponses);
        int responseCount = Mathf.Min(shuffledResponses.Count, 3);

        for (int i = 0; i < responseCount; i++)
        {
            DialogueResponse response = shuffledResponses[i];
            GameObject buttonObj = Instantiate(responseButtonPrefab, playerResponseButtonParent);
            TextMeshProUGUI responseText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            Button button = buttonObj.GetComponent<Button>();

            responseText.text = response.responseText;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnResponseSelected(response));
        }

        for (int i = responseCount; i < playerResponseButtonParent.childCount; i++)
        {
            playerResponseButtonParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnResponseSelected(DialogueResponse response)
    {
        // Process the selected response
        currentDialogueIndex++;
        DisplayNextDialogue();
    }

    private void EndDialogue()
    {
        HideDialogue();
        isDialogueActive = false;
        currentDialogueIndex = 0;
    }

    private void ShowDialogue()
    {
        PlayerResponseCanvas.SetActive(true);
    }

    private void HideDialogue()
    {
        foreach (var canvas in FindObjectsOfType<GameObject>()) // Find all canvases in scene
        {
            if (canvas.CompareTag("NPCDialogueCanvas"))
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
