using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    public DialogueNode RootNode;
}

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string dialogueText;
    public List<DialogueResponse> responses;

    public bool IsLastNode()
    {
        return responses == null || responses.Count == 0;
    }
}

[CreateAssetMenu(fileName = "New Dialogue Response", menuName = "Dialogue/Response")]
public class DialogueResponse : ScriptableObject
{
    public string responseText;
    public DialogueNode nextNode;
    public int score;
    public string adviceText;
}
