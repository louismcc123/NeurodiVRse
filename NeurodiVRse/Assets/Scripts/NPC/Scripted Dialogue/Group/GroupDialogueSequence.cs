using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group Dialogue Sequence", menuName = "Dialogue/Group Sequence")]
public class GroupDialogueSequence : ScriptableObject
{
    public List<GroupDialogueNode> nodes;
    public List<GroupDialogueResponse> responses;
    public bool completed = false;

    public bool IsLastSequence()
    {
        return responses == null || responses.Count == 0;
    }
}
