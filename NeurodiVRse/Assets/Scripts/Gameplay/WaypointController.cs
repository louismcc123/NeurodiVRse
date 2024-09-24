using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 3.0f;
    private int currentWaypoint = 0;

    void Update()
    {
        if (currentWaypoint < waypoints.Length)
        {
            Vector3 direction = waypoints[currentWaypoint].position - transform.position;

            if (Vector3.Angle(transform.forward, direction) > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (direction.magnitude > 0.1f)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentWaypoint++;
            }
        }
        else
        {
            currentWaypoint = 0;
        }
    }
}
