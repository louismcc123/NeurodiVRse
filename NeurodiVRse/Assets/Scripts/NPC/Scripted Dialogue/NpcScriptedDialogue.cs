using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScriptedDialogue : MonoBehaviour
{
    private Actor actor;
    [SerializeField] private Canvas NPCDialogueCanvas;

    private void Awake()
    {
        actor = GetComponent<Actor>();
        NPCDialogueCanvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NPCDialogueCanvas.gameObject.SetActive(true);
            actor.dialogueManager.PlayerResponseCanvas.SetActive(true); 
            actor.SpeakTo();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NPCDialogueCanvas.gameObject.SetActive(false);
            actor.dialogueManager.PlayerResponseCanvas.SetActive(false); 
        }
    }
}
