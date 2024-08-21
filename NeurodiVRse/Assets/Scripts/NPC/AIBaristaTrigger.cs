using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBaristaTrigger : MonoBehaviour
{
    public NpcAiDialogue NpcAiDialogue;
    public BaristaBehaviour baristaBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (NpcAiDialogue != null)
            {
                NpcAiDialogue.playerInRange = true;
            }
            if (baristaBehaviour != null)
            {
                baristaBehaviour.playerInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (NpcAiDialogue != null)
            {
                NpcAiDialogue.playerInRange = false;
            }
            if (baristaBehaviour != null)
            {
                baristaBehaviour.playerInRange = false;
            }
        }
    }
}
