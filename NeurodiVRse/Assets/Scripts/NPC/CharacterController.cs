/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
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
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public Transform[] waypoints;
    public float rotationSpeed = 5.0f;
    private int currentWaypoint = 0;
    private bool isMoving = false;
    private bool hasReachedCurrentWaypoint = false;

    private NavMeshAgent agent;
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
            animator.applyRootMotion = false;
        }

        if (waypoints.Length > 0)
        {
            MoveToWaypoint(0);
        }
    }

    private void Update()
    {
        CheckIfReachedWaypoint();
        UpdateAnimator();
    }

    public void MoveToWaypoint(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < waypoints.Length)
        {
            currentWaypoint = waypointIndex;
            agent.SetDestination(waypoints[waypointIndex].position);
            isMoving = true;
            hasReachedCurrentWaypoint = false;
            Debug.Log(gameObject.name + ": Moving to waypoint: " + waypointIndex);
        }
        else
        {
            Debug.LogError(gameObject.name + ": Waypoint index out of bounds! Index: " + waypointIndex + ", Waypoints length: " + waypoints.Length);
        }
    }

    private void CheckIfReachedWaypoint()
    {
        if (!hasReachedCurrentWaypoint && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.1f)
            {
                isMoving = false;
                animator.SetBool("IsWalking", false);
                hasReachedCurrentWaypoint = true;
                Debug.Log(gameObject.name + ": Reached waypoint: " + currentWaypoint);
            }
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

    private void UpdateAnimator()
    {
        bool shouldBeWalking = isMoving;

        if (animator.GetBool("IsWalking") != shouldBeWalking)
        {
            animator.SetBool("IsWalking", shouldBeWalking);
        }
    }
}
