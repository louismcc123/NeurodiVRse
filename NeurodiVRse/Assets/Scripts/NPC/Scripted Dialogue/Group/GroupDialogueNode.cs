using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group Dialogue Node", menuName = "Dialogue/Group Node")]
public class GroupDialogueNode : ScriptableObject
{
    public string dialogueText;
    public AudioClip dialogueAudio;
    public List<DialogueResponse> responses;
    public GameObject NPCDialogueCanvas;

    public bool IsLastNode()
    {
        return responses == null || responses.Count == 0;
    }
}
