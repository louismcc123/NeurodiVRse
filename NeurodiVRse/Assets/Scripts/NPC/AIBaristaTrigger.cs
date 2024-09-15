using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBaristaTrigger : MonoBehaviour
{
    public AIDialogueController aiDialogueController;
    public BaristaBehaviour baristaBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (aiDialogueController != null)
            {
                aiDialogueController.playerInRange = true;
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
            if (aiDialogueController != null)
            {
                aiDialogueController.playerInRange = false;
            }
            if (baristaBehaviour != null)
            {
                baristaBehaviour.playerInRange = false;
            }
        }
    }
}
