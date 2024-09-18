using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group Dialogue Response", menuName = "Dialogue/Group Response")]
public class GroupDialogueResponse : ScriptableObject
{
    public string responseText;
    public AudioClip responseAudio;
    public GroupDialogueSequence nextSequence;
    public int score;
    public string adviceText;
}
