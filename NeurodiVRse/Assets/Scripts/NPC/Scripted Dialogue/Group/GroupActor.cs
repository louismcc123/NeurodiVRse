using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupActor : MonoBehaviour
{
    public string Name;
    public Transform character;
    public Animator animator;
    public AudioSource audioSource;
    public GroupDialogueSequence initialSequence;
    public GroupDialogueManager dialogueManager;
    public GameObject NPCDialogueCanvas;
}