using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaController : MonoBehaviour
{
    public Animator animator;
    public Transform[] waypoint;
    public Transform handTransform;
    public GameObject coffeeCupPrefab;
    public ParticleSystem coffeeMachineParticleSystem;

    public float moveSpeed = 1.0f;
    public float rotationSpeed = 5.0f;

    private int currentWaypoint = 0;
    private bool isMoving = false;


    public DialogueManager dialogueManager;

    private void Update()
    {

        if (isMoving)
        {
            MoveTowardsWaypoint();
        }
    }

    public void MoveToWaypoint(int waypointIndex)
    {
        if (waypointIndex < waypoint.Length)
        {
            currentWaypoint = waypointIndex;
            isMoving = true;
        }
        else
        {
            Debug.LogError("Waypoint index out of bounds!");
        }
    }

    private void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoint[currentWaypoint];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            isMoving = false;

            if (currentWaypoint == 1) // till
            {
                dialogueManager.ResumeDialogue();
            }
        }
    }

    public void StartTakingPayment()
    {
        StartCoroutine(TakePayment());
    }

    private IEnumerator TakePayment()
    {
        MoveToWaypoint(1);
        while (isMoving) yield return null;
        animator.SetTrigger("TakePayment");
        yield return new WaitForSeconds(2.0f);

        // if player pays then:
        // dialogueManager.ResumeDialogue();
    }

    public void StartCoffeePreparation()
    {
        StartCoroutine(PrepareCoffee());
    }

    private IEnumerator PrepareCoffee()
    {
        MoveToWaypoint(3);
        while (isMoving) yield return null;
        animator.SetTrigger("PrepareCoffee");
        if (coffeeMachineParticleSystem != null)
        {
            coffeeMachineParticleSystem.Play();
        }
        yield return new WaitForSeconds(5.0f); // Adjust based on animation duration

        if (coffeeCupPrefab != null && handTransform != null)
        {
            Instantiate(coffeeCupPrefab, handTransform.position, handTransform.rotation, handTransform);
        }

        MoveToWaypoint(4);
        while (isMoving) yield return null;
        animator.SetTrigger("ServeCoffee");
        yield return new WaitForSeconds(2.0f); // Adjust based on animation duration

        dialogueManager.ResumeDialogue();
    }
}
