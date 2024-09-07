using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehaviours : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5f;

    private Transform currentTarget;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void FaceSpeaker(Transform target)
    {
        currentTarget = target;

        if (agent != null)
        {
            agent.updateRotation = false;
        }

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void StopFacing()
    {
        if (agent != null)
        {
            agent.updateRotation = true;
        }
    }
}