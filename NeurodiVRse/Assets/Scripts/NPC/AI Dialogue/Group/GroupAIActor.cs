using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAIActor : MonoBehaviour
{
    public string Name;
    public string npcPrompt;
    public Animator animator;
    public AudioSource audioSource;
    public GameObject NPCDialogueCanvas;
    public GroupAIDialogueController dialogueController;
}