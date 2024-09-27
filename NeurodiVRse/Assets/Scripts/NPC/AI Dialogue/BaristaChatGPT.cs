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
        
        protected override void HandleResponse(string responseContent) // Handles ChatGPT's response content and checks if payment is requested
        {
            if (IsRequestingPayment(responseContent))
            {
                PauseDialogue(); // Pauses the conversation for payment
                paymentCanvas.SetActive(true); // Displays the payment options
            }
        }

        private bool IsRequestingPayment(string response) // Detects if the response mentions payment-related terms like "card" or "cash"
        {
            return response.Contains("card", System.StringComparison.OrdinalIgnoreCase)
                || response.Contains("cash", System.StringComparison.OrdinalIgnoreCase)
                || response.Contains("£", System.StringComparison.OrdinalIgnoreCase);
        }

        private void OnCardSelected()
        {
            paymentCanvas.SetActive(false);
            cardManager.InstantiateCard(); // Triggers card payment process
        }

        private void OnCashSelected()
        {
            paymentCanvas.SetActive(false);
            cashManager.InstantiateCash(); // Triggers cash payment process
        }

        public IEnumerator SayThankYou() // Plays a thank you message and begins the coffee preparation sequence
        {
            SetNpcTalking(true);

            string thankYou = "Thank you. Your coffee will be ready in just a moment.";

            var thankYouMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = thankYou
            };

            AppendMessage(thankYouMessage); // adds the thank you message to the dialogue history
            ttsBridge.Speak(thankYouMessage.Content); // Speak the message

            yield return new WaitForSeconds(3f);

            PauseDialogue();  // Pauses the conversation
            StartCoroutine(StartCoffeePreparationSequence()); // Begins coffee preparation
        }

       
        public IEnumerator SayEnjoy() // Plays an "enjoy" message after coffee preparation is complete
        {
            SetNpcTalking(true);  // Sets NPC to talking state

            string enjoy = "Here's your coffee. Enjoy!";

            var enjoyMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = enjoy
            };

            AppendMessage(enjoyMessage); // adds the enjoy message to the dialogue history
            ttsBridge.Speak(enjoyMessage.Content); // Uses text-to-speech to speak the message

            yield return new WaitForSeconds(2f);

            SetNpcTalking(false); // Ends talking state
            openAICanvas.SetActive(true); // Reactivates dialogue interface
        }

        private IEnumerator StartCoffeePreparationSequence()
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
            StartCoroutine(SayEnjoy()); // NPC tells the player to enjoy the coffee
        }

        
        private IEnumerator StartCoffeePreparation() // play steam particle system for 5 secs
        {
            Debug.Log(gameObject.name + ": StartCoffeePreparation");

            risingSteam.SetActive(true);
            yield return new WaitForSeconds(5f);
            risingSteam.SetActive(false); 
        }
    }
}