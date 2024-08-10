using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaTrigger : MonoBehaviour
{
    public NPCInteraction npcInteraction;
    public BaristaBehaviour baristaBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (npcInteraction != null)
            {
                npcInteraction.playerInRange = true;
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
            if (npcInteraction != null)
            {
                npcInteraction.playerInRange = false;
            }
            if (baristaBehaviour != null)
            {
                baristaBehaviour.playerInRange = false;
            }
        }
    }
}
