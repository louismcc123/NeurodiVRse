using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenAI
{
    public class BaristaChatGPT : ChatGPT
    {
        [SerializeField] private AICardManager cardManager;
        [SerializeField] private AICashManager cashManager;
        [SerializeField] private Transform coffeeCupSpawnPosition;
        [SerializeField] private GameObject coffeeCupPrefab;
        [SerializeField] private GameObject risingSteam;
        [SerializeField] private GameObject paymentCanvas;
        [SerializeField] private Button cardButton;
        [SerializeField] private Button cashButton;

        public CharacterController characterController;

        private enum State 
        { 
            Idle, AwaitingPayment, PreparingCoffee
        }

        private State currentState = State.Idle;

        protected override void Start()
        {
            base.Start();

            cardButton.onClick.AddListener(OnCardSelected);
            cashButton.onClick.AddListener(OnCashSelected);
        }
        
        protected override void HandleResponse(string responseContent)
        {
            /*if (responseContent.Contains("Card, please"))
            {
                currentState = State.AwaitingPayment;
                cardManager.InstantiateCard();
            }
            else if (responseContent.Contains("Cash, please"))
            {
                currentState = State.AwaitingPayment;
                cashManager.InstantiateCash();
            }
            else if (responseContent.Contains("Thank you"))
            {
                StartCoroutine(StartCoffeePreparationSequence());
            }
            else
            {
                currentState = State.Idle;
            }*/

            if (IsRequestingPayment(responseContent))
            {
                currentState = State.AwaitingPayment;
                PauseDialogue();
                paymentCanvas.SetActive(true);
            }
            else
            {
                currentState = State.Idle;
            }
        }

        private bool IsRequestingPayment(string response)
        {
            return response.Contains("card", System.StringComparison.OrdinalIgnoreCase)
                || response.Contains("cash", System.StringComparison.OrdinalIgnoreCase)
                || response.Contains("£", System.StringComparison.OrdinalIgnoreCase);
        }

        private void OnCardSelected()
        {
            paymentCanvas.SetActive(false);
            cardManager.InstantiateCard();
            currentState = State.AwaitingPayment;
            //StartCoroutine(StartCoffeePreparationSequence());
        }

        private void OnCashSelected()
        {
            paymentCanvas.SetActive(false);
            cashManager.InstantiateCash();
            currentState = State.AwaitingPayment;
            //StartCoroutine(StartCoffeePreparationSequence());
        }

        public IEnumerator SayThankYou()
        {
            //npcAiDialogue.SetNpcTalking(true);
            npcAiDialogue.isNpcTalking = true;

            AppendMessage(new ChatMessage { Role = "npc", Content = "Thank you. Your coffee will be ready in just a moment." });

            yield return new WaitForSeconds(2f);

            PauseDialogue();

            StartCoroutine(StartCoffeePreparationSequence());
        }

        private IEnumerator StartCoffeePreparationSequence()
        {
            currentState = State.PreparingCoffee;

            characterController.MoveToWaypoint(2);
            yield return new WaitUntil(() => !characterController.IsMoving());

            StartCoroutine(StartCoffeePreparation());
            yield return new WaitForSeconds(8f);

            characterController.MoveToWaypoint(3);
            yield return new WaitUntil(() => !characterController.IsMoving());

            Instantiate(coffeeCupPrefab, coffeeCupSpawnPosition.position, coffeeCupSpawnPosition.rotation);
            Debug.Log(gameObject.name + ": Coffee cup instantiated.");

            ResumeDialogue();
            currentState = State.Idle;
        }

        private IEnumerator StartCoffeePreparation()
        {
            Debug.Log(gameObject.name + ": StartCoffeePreparation");

            risingSteam.SetActive(true);
            yield return new WaitForSeconds(5f);
            risingSteam.SetActive(false);
        }
    }
}
