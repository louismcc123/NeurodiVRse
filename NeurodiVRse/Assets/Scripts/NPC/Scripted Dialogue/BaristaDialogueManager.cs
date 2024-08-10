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

    protected override void HandleNextNode(DialogueResponse response, string title)
    {
        switch (response.responseText)
        {
            case "Card, please.":
                PauseDialogue(response.nextNode, title);
                cardManager.InstantiateCard();
                break;
            case "Cash, please.":
                PauseDialogue(response.nextNode, title);
                cashManager.InstantiateCash();
                break;
            case "Thank you.":
                PauseDialogue(response.nextNode, title);
                StartCoroutine(StartCoffeePreparationSequence());
                break;
            default:
                base.HandleNextNode(response, title);
                break;
        }
    }

    private IEnumerator StartCoffeePreparationSequence()
    {
        characterController.MoveToWaypoint(2);
        yield return new WaitUntil(() => !characterController.IsMoving());
        StartCoroutine(StartCoffeePreparation());
        yield return new WaitForSeconds(8f);
        characterController.MoveToWaypoint(3);
        yield return new WaitUntil(() => !characterController.IsMoving());
        Instantiate(coffeeCupPrefab, coffeeCupSpawnPosition.position, coffeeCupSpawnPosition.rotation);
        Debug.Log(gameObject.name + ": Coffee cup instantiated. Now dialogue should resume.");
        ResumeDialogue();
    }

    private IEnumerator StartCoffeePreparation()
    {
        Debug.Log(gameObject.name + ": StartCoffeePreparation");

        risingSteam.SetActive(true);
        yield return new WaitForSeconds(5f);
        risingSteam.SetActive(false);
    }
}
