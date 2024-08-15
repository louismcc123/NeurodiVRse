/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAiDialogue : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject NPCSpeechCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openAICanvas.SetActive(true);
            NPCSpeechCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openAICanvas.SetActive(false);
            NPCSpeechCanvas.SetActive(false);
        }
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAiDialogue : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private GameObject NPCSpeechCanvas;
    [SerializeField] private BaristaBehaviour baristaBehaviour;

    private Animator animator;

    private bool isNpcTalking = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError(gameObject.name + ": Animator component not found on any child GameObject.");
        }
    }
    
    private void Update()
    {
        if (baristaBehaviour != null && baristaBehaviour.playerInRange)
        {
            openAICanvas.SetActive(true);

            if (isNpcTalking)
            {
                NPCSpeechCanvas.SetActive(true);
                animator.SetBool("IsTalking", true);

            }
            else
            {
                NPCSpeechCanvas.SetActive(false);
                animator.SetBool("IsTalking", false);
            }
        }
        else
        {
            openAICanvas.SetActive(false);
            NPCSpeechCanvas.SetActive(false);
        }
    }

    public void SetNpcTalking(bool talking)
    {
        isNpcTalking = talking;
    }
}
