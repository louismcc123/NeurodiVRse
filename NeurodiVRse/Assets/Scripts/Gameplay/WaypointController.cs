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
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(waypoints[currentWaypoint].position - transform.position), 
                rotationSpeed * Time.deltaTime);

            Vector3 moveDirection = waypoints[currentWaypoint].position - transform.position;

            if (moveDirection.magnitude < 1)
            {
                currentWaypoint++;
            }
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentWaypoint = 0;
        }
    }
}