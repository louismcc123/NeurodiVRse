using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupDialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GroupDialogueSequence initialSequence;

    private float timePlayerExited = -Mathf.Infinity;
    public float resumeTimeThreshold = 5f;

    private MeshRenderer meshRenderer;

    public GroupDialogueManager groupDialogueManager;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range");

            meshRenderer.enabled = false;
            instructionCanvas.SetActive(false);

            if (groupDialogueManager != null)
            {
                if (Time.time - timePlayerExited > resumeTimeThreshold)
                {
                    Debug.Log("Starting new group dialogue.");
                    groupDialogueManager.StartGroupDialogue(initialSequence);
                }
                else
                {
                    Debug.Log("Resuming group dialogue.");
                    groupDialogueManager.ResumeGroupDialogue();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player no longer in range");

            timePlayerExited = Time.time;

            if (groupDialogueManager != null)
            {
                groupDialogueManager.PauseGroupDialogue();
            }

            meshRenderer.enabled = true;
            instructionCanvas.SetActive(true);
        }
    }
}