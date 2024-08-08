using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class BaristaBehaviour : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 3.0f;

    private bool isPlayerInRange = false;
    private bool hasMovedToWaypoint = false;

    public CharacterController characterController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            isPlayerInRange = true;
            Debug.Log("player in range");

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
            Debug.Log("player out of range");
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
        if (player == null) return;

        if (characterController.IsMoving() || characterController.GetCurrentWaypoint() == 2)
        {
            return;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
