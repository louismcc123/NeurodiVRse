using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenAI
{
    public class BaristaChatGPT : ChatGPT
    {
        [Header("Barista Specific")]
        [SerializeField] private AICardManager cardManager;
        [SerializeField] private AICashManager cashManager;
        [SerializeField] private Transform coffeeCupSpawnPosition;
        [SerializeField] private GameObject coffeeCupPrefab;
        [SerializeField] private GameObject risingSteam;
        [SerializeField] private GameObject paymentCanvas;
        [SerializeField] private Button cardButton;
        [SerializeField] private Button cashButton;

        public CharacterController characterController;

        protected override void Start()
        {
            base.Start();

            cardButton.onClick.AddListener(OnCardSelected);
            cashButton.onClick.AddListener(OnCashSelected);
        }
        
        protected override void HandleResponse(string responseContent)
        {
            if (IsRequestingPayment(responseContent))
            {
                PauseDialogue();
                paymentCanvas.SetActive(true);
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
            //StartCoroutine(StartCoffeePreparationSequence());
        }

        private void OnCashSelected()
        {
            paymentCanvas.SetActive(false);
            cashManager.InstantiateCash();
            //StartCoroutine(StartCoffeePreparationSequence());
        }

        public IEnumerator SayThankYou()
        {
            SetNpcTalking(true);

            string thankYou = "Thank you. Your coffee will be ready in just a moment.";

            var thankYouMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = thankYou
            };

            AppendMessage(thankYouMessage);
            ttsBridge.Speak(thankYouMessage.Content);

            yield return new WaitForSeconds(3f);

            PauseDialogue();
            StartCoroutine(StartCoffeePreparationSequence());
        }

        public IEnumerator SayEnjoy()
        {
            SetNpcTalking(true);

            string enjoy = "Here's your coffee. Enjoy!";

            var enjoyMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = enjoy
            };

            AppendMessage(enjoyMessage);
            ttsBridge.Speak(enjoyMessage.Content);

            yield return new WaitForSeconds(2f);

            SetNpcTalking(false);
            openAICanvas.SetActive(true);
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
            Debug.Log(gameObject.name + ": Coffee cup instantiated.");

            ResumeDialogue();
            StartCoroutine(SayEnjoy());
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
