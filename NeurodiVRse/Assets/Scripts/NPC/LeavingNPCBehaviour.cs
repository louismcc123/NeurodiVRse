using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class LeavingNPCBehaviour : MonoBehaviour
{
    //private NavMeshAgent navMeshAgent;
    public GameObject door; 

    //public float interactionDistance = 1f; 
    //private bool isInteracting = false;

    //public DialogueManager dialogueManager;
    public CharacterController characterController;
    private Door doorScript; 

    private void Awake()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();

        if (door != null)
        {
            doorScript = door.GetComponent<Door>();
        }
    }

    private void Update()
    {
        if (characterController.GetCurrentWaypoint() == 0)
        {
            StartCoroutine(MoveToDoor());
        }

        if (characterController.GetCurrentWaypoint() == 1)
        {
            if (doorScript != null)
            {
                doorScript.HandleNPCLeaving();
            }

            StartCoroutine(LeaveCafe());
        }

        /*if (!isInteracting)
        {
            GameObject player = FindObjectOfType<PlayerController>().gameObject;
            if (player != null && Vector3.Distance(transform.position, player.transform.position) < interactionDistance)
            {
                if (Input.GetKeyDown(KeyCode.E)) // Change as needed
                {
                    StartCoroutine(InteractWithPlayer(player));
                }
            }
        }*/
    }

    private IEnumerator MoveToDoor()
    {
        yield return new WaitForSeconds(5f);

        characterController.MoveToWaypoint(1);
    }
    private IEnumerator LeaveCafe()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    /*private IEnumerator InteractWithPlayer(GameObject player)
    {
        isInteracting = true;
        navMeshAgent.isStopped = true; // Stop the NPC
        yield return new WaitUntil(() => !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance);

        // Assuming there's a specific dialogue node for interactions with this NPC
        DialogueNode interactionNode = GetInteractionDialogueNode(); // Implement this method
        dialogueManager.StartDialogue(gameObject.name, interactionNode);

        yield return new WaitUntil(() => !dialogueManager.IsDialogueActive());

        // Resume movement after dialogue ends
        navMeshAgent.isStopped = false;
        MoveToWaypoint(2);
        isInteracting = false;
    }

    private DialogueNode GetInteractionDialogueNode()
    {
        // Return a DialogueNode suitable for the NPC's interaction
        // You might need to set this up in the Inspector or through some other method
        return new DialogueNode(); // Placeholder; replace with actual implementation
    }*/
}
