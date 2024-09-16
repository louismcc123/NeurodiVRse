using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChatGPTTrigger : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject openAICanvas;

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
            openAICanvas.SetActive(true);

            if (groupConversationManager != null)
            {
                groupConversationManager.OnPlayerEnterTrigger();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out of range");

            openAICanvas.SetActive(false);

            timePlayerExited = Time.time;

            if (groupConversationManager != null)
            {
                groupConversationManager.OnPlayerExitTrigger();
            }

            meshRenderer.enabled = true;
            instructionCanvas.SetActive(true);
        }
    }
}
