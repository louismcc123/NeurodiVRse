using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChatGPTTrigger : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;

    private float timePlayerExited = -1f;
    public float resumeTimeThreshold = 5f;

    private MeshRenderer meshRenderer;

    public GroupConversationManager groupConversationManager;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range");

            //groupConversationManager.playerInRange = true;
            meshRenderer.enabled = false;
            instructionCanvas.SetActive(false);

            if (groupConversationManager != null)
            {
                if (Time.time - timePlayerExited > resumeTimeThreshold)
                {
                    Debug.Log("Starting new conversation.");
                    groupConversationManager.StartConversation();
                }
                else
                {
                    Debug.Log("Resuming conversation.");
                    groupConversationManager.ResumeConversation();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out of range");

            //groupConversationManager.playerInRange = false;

            timePlayerExited = Time.time;

            if (groupConversationManager != null)
            {
                Debug.Log("Pausing conversation.");
                groupConversationManager.PauseConversation();
            }

            meshRenderer.enabled = true;
            instructionCanvas.SetActive(true);
        }
    }
}
