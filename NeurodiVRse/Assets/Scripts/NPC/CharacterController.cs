using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class CharacterController : MonoBehaviour
{
    public Transform player;
    public Transform[] waypoints;
    public float rotationSpeed = 5.0f;
    private int currentWaypoint = 0;
    private bool isMoving = false;
    private bool hasReachedCurrentWaypoint = false;

    public NavMeshAgent agent;

    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError(gameObject.name + ": Animator component not found on any child GameObject.");
        }
        else
        {
            animator.applyRootMotion = false;// Ensures the agent controls the movement, not the animation
        }

        if (waypoints.Length > 0)
        {
            MoveToWaypoint(0); // Move to first waypoint
        }
    }

    private void Update()
    {
        CheckIfReachedWaypoint(); // Check if character has reached waypoint
        UpdateAnimator(); // Update the walking animation state

        if (isMoving)
        {
            RotateTowardsDestination(); // Face destination
        }
    }

    public void MoveToWaypoint(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < waypoints.Length)
        {
            currentWaypoint = waypointIndex;
            agent.SetDestination(waypoints[waypointIndex].position); // Set destination of new waypoint on the navmeshagent
            isMoving = true;
            hasReachedCurrentWaypoint = false;
            Debug.Log(gameObject.name + ": Moving to waypoint: " + waypointIndex);
        }
        else
        {
            Debug.LogError(gameObject.name + ": Waypoint index out of bounds! Index: " + waypointIndex + ", Waypoints length: " + waypoints.Length);
        }
    }

    private void CheckIfReachedWaypoint() // Check if character has reached current waypoint
    {
        if (!hasReachedCurrentWaypoint && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.1f) // // Check if the agent has no path and has stopped
            {
                isMoving = false;
                animator.SetBool("IsWalking", false); // Stop walking animation
                hasReachedCurrentWaypoint = true;
                Debug.Log(gameObject.name + ": Reached waypoint: " + currentWaypoint);
            }
        }
    }

    private void RotateTowardsDestination() // Face destination while moving
    {
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (agent != null)
            {
                agent.updateRotation = false; // Disabling rotation by navmeshagent due to bugs/conflicts
            }

            // Calculate the direction to the destination
            Vector3 direction = (agent.steeringTarget - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void FacePlayer()
    {
        if (agent != null)
        {
            agent.updateRotation = false;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public void PauseMovement()
    {
        if (isMoving)
        {
            agent.isStopped = true; // Stop the movement
            isMoving = false;
            Debug.Log(gameObject.name + ": Movement paused at waypoint: " + currentWaypoint);
        }
    }

    public void ResumeMovement()
    {
        if (!isMoving)
        {
            agent.isStopped = false; // Resume movement
            isMoving = true;
            Debug.Log(gameObject.name + ": Resuming movement towards waypoint: " + currentWaypoint);
            agent.SetDestination(waypoints[currentWaypoint].position); // Continue towards the current waypoint
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public int GetCurrentWaypoint()
    {
        return currentWaypoint;
    }

    public bool HasReachedCurrentWaypoint()
    {
        return hasReachedCurrentWaypoint;
    }

    private void UpdateAnimator() // Updates the walking animation based on whether the character is moving
    {
        bool shouldBeWalking = isMoving;

        if (animator.GetBool("IsWalking") != shouldBeWalking)
        {
            animator.SetBool("IsWalking", shouldBeWalking);
        }
    }   

}