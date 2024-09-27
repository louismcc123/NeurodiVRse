using GLTFast.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaDialogueManager : DialogueManager
{
    public CardManager cardManager;
    public CashManager cashManager;
    public Transform coffeeCupSpawnPosition;
    public GameObject coffeeCupPrefab;
    public GameObject risingSteam;

    protected override void HandleNextNode(DialogueResponse response, string title) // Handles different responses based on player choices and prepares the next node
    {
        switch (response.responseText)
        {
            case "Card, please.": // Pauses dialogue for card payment and handles card instantiation
                PauseDialogue(response.nextNode, title);
                cardManager.InstantiateCard();
                break;
            case "Cash, please.": // Pauses dialogue for cash payment and handles cash instantiation
                PauseDialogue(response.nextNode, title);
                cashManager.InstantiateCash();
                break;
            case "Thank you.": // Pauses dialogue and starts the coffee preparation sequence
                PauseDialogue(response.nextNode, title);
                StartCoroutine(StartCoffeePreparationSequence());
                break;
            default:
                base.HandleNextNode(response, title);
                break;
        }
    }

    private IEnumerator StartCoffeePreparationSequence() // Manages the coffee preparation sequence after payment
    {
        characterController.MoveToWaypoint(2);  // Moves NPC to coffee machine
        yield return new WaitUntil(() => !characterController.IsMoving()); // Wait until NPC reaches the waypoint

        StartCoroutine(StartCoffeePreparation()); // Starts the coffee preparation process
        yield return new WaitForSeconds(8f);

        characterController.MoveToWaypoint(3);  // Moves NPC to coffee drop off location
        yield return new WaitUntil(() => !characterController.IsMoving());
        
        Instantiate(coffeeCupPrefab, coffeeCupSpawnPosition.position, coffeeCupSpawnPosition.rotation); // Instantiates a coffee cup at the coffee spawn position
        Debug.Log(gameObject.name + ": Coffee cup instantiated."); 

        ResumeDialogue(); // Resumes conversation after coffee preparation
    }


    private IEnumerator StartCoffeePreparation() // play steam particle system for 5 secs
    {
        Debug.Log(gameObject.name + ": StartCoffeePreparation");

        risingSteam.SetActive(true);
        yield return new WaitForSeconds(5f);
        risingSteam.SetActive(false);
    }
}