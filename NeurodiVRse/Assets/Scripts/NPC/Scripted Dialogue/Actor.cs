using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public string Name;
    public Dialogue Dialogue;
    public DialogueManager dialogueManager;

    private void Awake()
    {
        if (dialogueManager == null)
        {
            dialogueManager = GetComponent<DialogueManager>();
        }
    }

    public void SpeakTo()
    {
        dialogueManager.StartDialogue(Name, Dialogue.RootNode);
    }
}
