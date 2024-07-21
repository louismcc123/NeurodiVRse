using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public string Name;
    public DialogueNode initialDialogueNode;
    public DialogueManager dialogueManager;

    private void Awake()
    {
        if (dialogueManager == null)
        {
            dialogueManager = GetComponentInChildren<DialogueManager>();
        }
    }

    public void SpeakTo()
    {
        dialogueManager.StartDialogue(Name, initialDialogueNode);
    }
}
