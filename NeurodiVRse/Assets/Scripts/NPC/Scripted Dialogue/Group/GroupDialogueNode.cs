using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group Dialogue Node", menuName = "Dialogue/Group Node")]
public class GroupDialogueNode : ScriptableObject
{
    public string character;
    public string dialogueText;
    public AudioClip dialogueAudio;
    public bool leaveAfterDialogue;
}