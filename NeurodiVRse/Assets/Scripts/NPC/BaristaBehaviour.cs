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

    public CharacterController characterController;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            FacePlayer();

            if (!hasMovedToWaypoint)
            {
                characterController.MoveToWaypoint(1);
                hasMovedToWaypoint = true;
            }
        }
    }

    private void FacePlayer()
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

        if (agent != null)
        {
            agent.updateRotation = false;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
