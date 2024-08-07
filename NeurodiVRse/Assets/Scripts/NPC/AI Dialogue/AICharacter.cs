using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AICharacter", menuName = "NPC/Character")]
public class AICharacter : ScriptableObject
{
    public string roleName;
    public string introductionText;
    public string[] behaviors;
    public string background; // Detailed background or description of the NPC
    public string personalityTraits; // Traits that define the NPC's behavior
    public string[] dialogueExamples; // Examples of how the NPC might respond in different situations
}
