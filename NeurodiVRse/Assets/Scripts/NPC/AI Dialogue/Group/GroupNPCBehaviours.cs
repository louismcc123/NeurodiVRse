using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupNPCBehaviours : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;

    public Transform currentTarget;
    private NavMeshAgent agent;
    private Animator animator;

    public GroupConversationManager conversationManager;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        if (conversationManager == null)
        {
            conversationManager = GetComponentInParent<GroupConversationManager>();
        }
    }

    public void FaceSpeaker(Transform target)
    {
        currentTarget = target;
        
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        //Debug.Log($"{this.gameObject.name} is looking at the speaker");
    }
}
