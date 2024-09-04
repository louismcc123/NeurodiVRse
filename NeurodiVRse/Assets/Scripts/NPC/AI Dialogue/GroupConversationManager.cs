using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class GroupConversationManager : MonoBehaviour
    {
        [SerializeField] private float responseTime = 2f;

        private List<GroupChatGPT> npcList = new List<GroupChatGPT>();
        private GroupChatGPT currentSpeaker;
        private bool playerTurn = true;

        private void Start()
        {
            GroupChatGPT[] npcs = FindObjectsOfType<GroupChatGPT>();
            foreach (var npc in npcs)
            {
                RegisterNPC(npc);
            }
        }

        public void RegisterNPC(GroupChatGPT npc)
        {
            npcList.Add(npc);
        }

        public void StartConversation()
        {
            if (playerTurn)
            {
                return;
            }

            if (npcList.Count > 0)
            {
                int randomIndex = Random.Range(0, npcList.Count);
                currentSpeaker = npcList[randomIndex];
                currentSpeaker.ActivateNPC();
            }
        }

        public void OnNPCFinishedSpeaking()
        {
            if (currentSpeaker != null)
            {
                currentSpeaker.DeactivateNPC();
                currentSpeaker = null;
            }

            StartCoroutine(WaitAndCheckForPlayerInput());
        }

        private IEnumerator WaitAndCheckForPlayerInput()
        {
            playerTurn = true;
            Debug.Log("Waiting for player input...");

            yield return new WaitForSeconds(responseTime);

            if (playerTurn)
            {
                Debug.Log("Player did not speak. Moving to the next NPC...");
                playerTurn = false;
                StartConversation();
            }
            else
            {
                Debug.Log("Player has taken their turn.");
            }
        }

        public void OnPlayerFinishedSpeaking(string playerMessage)
        {
            playerTurn = false;
            ProcessPlayerMessage(playerMessage);
            StartConversation();
        }

        public void OnPlayerInterrupts()
        {
            if (currentSpeaker != null)
            {
                currentSpeaker.DeactivateNPC();
                currentSpeaker = null;
            }

            playerTurn = true;
            StopAllCoroutines();
        }

        private void ProcessPlayerMessage(string message)
        {
            Debug.Log($"Player message received: {message}");
        }
    }
}