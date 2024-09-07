using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupActor : MonoBehaviour
{
    public string Name;
    public Transform character;
    public Animator animator;
    public AudioSource audioSource;
    public GroupDialogueNode initialDialogueNode;
    public GroupDialogueManager dialogueManager;
    public GameObject NPCDialogueCanvas;

    private void Awake()
    {
        if (character == null)
        {
            character = GetComponent<Transform>();
        } 

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }
}