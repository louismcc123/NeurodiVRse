using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Response", menuName = "Dialogue/Response")]
public class DialogueResponse : ScriptableObject
{
    public string responseText;
    public DialogueNode nextNode;
    public int score;
    public string adviceText;
}
