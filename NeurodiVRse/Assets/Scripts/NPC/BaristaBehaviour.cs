using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaristaBehaviour : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 3.0f;
    public bool playerInRange = false;
    private bool hasMovedToWaypoint = false;

    private NavMeshAgent agent;
    public CharacterController characterController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerInRange) // Check if the player is in range
        {
            FaceTowardsPlayer(); // face player
           
            if (!hasMovedToWaypoint) // Move to a specific waypoint only once, if not already moved
            {
                characterController.MoveToWaypoint(1); // Move to till
                hasMovedToWaypoint = true;
            }
        }
    }

    private void FaceTowardsPlayer()
    {
        if (player == null) 
        { 
            return; // Exit if there's no player reference
        }
       
        if (characterController.IsMoving() || characterController.GetCurrentWaypoint() == 2) // Prevent facing the player if the character is moving or at coffee machine
        {
            //Debug.Log("Not facing player because character is moving or waypoint is 2.");
            return;
        }

        characterController.FacePlayer(); // Make the NPC face the player
    }
}
