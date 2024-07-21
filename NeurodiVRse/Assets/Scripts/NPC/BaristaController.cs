using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaController : MonoBehaviour
{
    public Animator animator;
    public Transform handTransform;
    public GameObject coffeeCupPrefab;
    public ParticleSystem coffeeMachineParticleSystem;
    public DialogueManager dialogueManager; 

    public void StartCoffeePreparation()
    {
        StartCoroutine(PrepareCoffee());
    }

    private IEnumerator PrepareCoffee()
    {
        // Play the animation for the barista moving to the coffee machine
        animator.SetTrigger("MoveToMachine");
        yield return new WaitForSeconds(2.0f); // Adjust this duration based on your animation

        // Play the animation for coffee preparation
        animator.SetTrigger("PrepareCoffee");
        if (coffeeMachineParticleSystem != null)
        {
            coffeeMachineParticleSystem.Play();
        }
        yield return new WaitForSeconds(5.0f); // Adjust this duration based on your animation

        // Instantiate the coffee cup in the barista's hand
        if (coffeeCupPrefab != null && handTransform != null)
        {
            GameObject coffeeCup = Instantiate(coffeeCupPrefab, handTransform.position, handTransform.rotation, handTransform);
        }

        // Play the animation for the barista returning to the player
        animator.SetTrigger("ReturnToPlayer");
        yield return new WaitForSeconds(5.0f); // Adjust this duration based on your animation

        // Resume the dialogue
        dialogueManager.ResumeDialogue();
    }
}
