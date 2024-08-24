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
        if (playerInRange)
        {
            FaceTowardsPlayer();

            if (!hasMovedToWaypoint)
            {
                characterController.MoveToWaypoint(1);
                hasMovedToWaypoint = true;
            }
        }
    }

    private void FaceTowardsPlayer()
    {
        if (player == null) 
        { 
            return;
        }

        if (characterController.IsMoving() || characterController.GetCurrentWaypoint() == 2)
        {
            //Debug.Log("Not facing player because character is moving or waypoint is 2.");
            return;
        }

        characterController.FacePlayer();
    }
}
