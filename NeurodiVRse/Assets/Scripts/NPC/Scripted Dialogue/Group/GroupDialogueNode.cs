using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group Dialogue Node", menuName = "Dialogue/Group Node")]
public class GroupDialogueNode : ScriptableObject
{
    public GroupActor actor;
    public string dialogueText;
    public AudioClip dialogueAudio; 

    public void StartTalking()
    {
        if (actor.animator != null)
        {
            actor.animator.SetBool("IsTalking", true);
        }
    }

    public void StopTalking()
    {
        if (actor.animator != null)
        {
            actor.animator.SetBool("IsTalking", false);
        }
    }
}