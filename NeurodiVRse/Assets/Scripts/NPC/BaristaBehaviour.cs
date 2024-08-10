using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaristaBehaviour : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 3.0f;

    private bool isPlayerInRange = false;
    private bool hasMovedToWaypoint = false;

    public CharacterController characterController;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            isPlayerInRange = true;
            Debug.Log("Player in range of barista");

            if (!hasMovedToWaypoint)
            {
                characterController.MoveToWaypoint(1);
                hasMovedToWaypoint = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            isPlayerInRange = false;
            Debug.Log("Player out of range of barista");
        }
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            FacePlayer();
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
            Debug.Log("Not facing player because character is moving or waypoint is 2.");
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
