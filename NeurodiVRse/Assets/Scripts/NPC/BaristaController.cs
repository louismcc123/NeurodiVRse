using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaController : MonoBehaviour
{
    public Animator animator;
    public Transform[] waypoints;
    public Transform handTransform;
    public ParticleSystem coffeeMachineParticleSystem;
    public GameObject coffeeCupPrefab;
    public GameObject cashPrefab;
    private GameObject paymentTriggerCollider;

    public float moveSpeed = 1.0f;
    public float rotationSpeed = 5.0f;
    private int currentWaypoint = 0;
    private bool isMoving = false;

    public DialogueManager dialogueManager;

    private void Start()
    {
        Debug.Log("BaristaController script started.");
        MoveToWaypoint(0);
        Debug.Log("MoveToWaypoint(0)");
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

            if (currentWaypoint == 1)
            {
                dialogueManager.OnBaristaAtTill();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other == paymentTriggerCollider && other.CompareTag("Cash"))
        {
            Debug.Log("Cash handed to the barista");
            dialogueManager.OnCashHandedOver();
            StartTakingPayment();
        }
    }

    public void StartTakingPayment()
    {
        StartCoroutine(TakePayment());
    }

    private IEnumerator TakePayment()
    {
        yield return new WaitForSeconds(2.0f);
        dialogueManager.ResumeDialogue();
    }

    public void StartCoffeePreparation()
    {
        StartCoroutine(PrepareCoffee());
    }

    private IEnumerator PrepareCoffee()
    {
        MoveToWaypoint(2);
        while (isMoving) yield return null;
        if (coffeeMachineParticleSystem != null)
        {
            coffeeMachineParticleSystem.Play();
        }
        yield return new WaitForSeconds(5.0f);

        if (coffeeCupPrefab != null && handTransform != null)
        {
            Instantiate(coffeeCupPrefab, handTransform.position, handTransform.rotation, handTransform);
        }

        MoveToWaypoint(3);
        while (isMoving) yield return null;
        yield return new WaitForSeconds(2.0f);

        dialogueManager.ResumeDialogue();
    }
}