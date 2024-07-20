using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScriptedDialogue : MonoBehaviour
{
    [SerializeField] private GameObject DialogueCanvas;
    private Actor actor;

    private void Awake()
    {
        actor = GetComponent<Actor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueCanvas.SetActive(true);
            actor.SpeakTo(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueCanvas.SetActive(false);
        }
    }
}
