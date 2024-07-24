using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string dialogueText;
    public AudioClip dialogueAudio; 
    public List<DialogueResponse> responses;

    public bool IsLastNode()
    {
        return responses == null || responses.Count == 0;
    }
}
