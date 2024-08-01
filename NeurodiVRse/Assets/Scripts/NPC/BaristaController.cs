using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaController : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 5.0f;
    private int currentWaypoint = 0;
    private bool isMoving = false;

    private void Start()
    {
        MoveToWaypoint(0);
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveTowardsWaypoint();
        }
    }

    public void MoveToWaypoint(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < waypoints.Length)
        {
            currentWaypoint = waypointIndex;
            isMoving = true;
            Debug.Log("Moving to waypoint: " + waypointIndex);
        }
        else
        {
            Debug.LogError("Waypoint index out of bounds! Index: " + waypointIndex + ", Waypoints length: " + waypoints.Length);
        }
    }

    private void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypoint];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distance < 0.1f)
        {
            Debug.Log("Reached waypoint: " + currentWaypoint);
            isMoving = false;

            // Notify other components if needed
            //OnReachedWaypoint(currentWaypoint);
        }
    }

    /*private void OnReachedWaypoint(int waypointIndex)
    {
        // This function can be used to trigger actions when a waypoint is reached.
        // Delegate specific actions to other components if needed.
        // Example:
        // if (waypointIndex == 1) { Notify DialogueManager or other scripts }

        Debug.Log("OnReachedWaypoint: " + waypointIndex);
    }*/

    public bool IsMoving()
    {
        return isMoving;
    }

    public int GetCurrentWaypoint()
    {
        return currentWaypoint;
    }
}
