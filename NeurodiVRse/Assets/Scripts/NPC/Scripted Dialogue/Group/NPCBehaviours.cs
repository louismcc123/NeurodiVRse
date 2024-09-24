using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NPCBehaviours : MonoBehaviour
{
    public Transform player;
    public Transform door;
    public float moveSpeed = 0.8f;
    public float rotationSpeed = 5f;

    private bool isLeaving = false;

    private Transform currentSpeaker;
    private Animator animator;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void FaceSpeaker(Transform target)
    {
        currentSpeaker = target;

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void StartTalking()
    {
        if (animator != null)
        {
            animator.SetBool("IsTalking", true);
        }
    }

    public void StopTalking()
    {
        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
        }
    }

    public void StartLeavingParty()
    {
        if (!isLeaving)
        {
            isLeaving = true;
            StartCoroutine(LeaveParty());
        }
    }

    public IEnumerator LeaveParty()
    {
        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
            animator.SetBool("IsLaughing", false);
            animator.SetBool("IsWalking", true);
        }

        yield return StartCoroutine(MoveToDoor());
    }

    private IEnumerator MoveToDoor()
    {
        while (Vector3.Distance(transform.position, door.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, door.position, moveSpeed * Time.deltaTime);

            Vector3 direction = (door.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(gameObject);
    }
}