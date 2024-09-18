using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyIntroNPC : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 8f;
    [SerializeField] private float minTalkingTime = 3f;
    [SerializeField] private float maxTalkingTime = 6f;
    [SerializeField] private float minLaughingTime = 1f;
    [SerializeField] private float maxLaughingTime = 3f;

    private void Start()
    {
        StartCoroutine(InitialAction());

        StartCoroutine(ToggleActions());
    }

    private IEnumerator InitialAction()
    {
        int randomAction = Random.Range(0, 2);

        if (randomAction == 0) // Talking
        {
            animator.SetBool("IsTalking", true);
            yield return new WaitForSeconds(Random.Range(minTalkingTime, maxTalkingTime));
            animator.SetBool("IsTalking", false);
        }
        else if (randomAction == 1) // Laughing
        {
            animator.SetBool("IsLaughing", true);
            yield return new WaitForSeconds(Random.Range(minLaughingTime, maxLaughingTime));
            animator.SetBool("IsLaughing", false);
        }
    }

    private IEnumerator ToggleActions()
    {
        while (true)
        {
            animator.SetBool("IsTalking", false);
            animator.SetBool("IsLaughing", false);

            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));

            int randomAction = Random.Range(0, 2);

            if (randomAction == 0) // Talking
            {
                animator.SetBool("IsTalking", true);
                yield return new WaitForSeconds(Random.Range(minTalkingTime, maxTalkingTime));
                animator.SetBool("IsTalking", false);
            }
            else if (randomAction == 1) // Laughing
            {
                animator.SetBool("IsLaughing", true);
                yield return new WaitForSeconds(Random.Range(minLaughingTime, maxLaughingTime));
                animator.SetBool("IsLaughing", false);
            }

            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        }
    }
}
