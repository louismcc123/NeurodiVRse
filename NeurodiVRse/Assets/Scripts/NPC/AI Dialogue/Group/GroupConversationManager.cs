using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupConversationManager : MonoBehaviour
{
    [SerializeField] private GameObject openAICanvas;
    [SerializeField] private float responseTime = 3f;

    private List<GroupChatGPT> npcList = new List<GroupChatGPT>();
    private GroupChatGPT currentNPC;

    private bool playerTurn = true;
    private bool playerRespondedToGreeting = false;

    private Coroutine waitingCoroutine;
    private string lastMessageContent;

    public bool playerInRange = false;

    private void Start()
    {
        GroupChatGPT[] npcs = GetComponentsInChildren<GroupChatGPT>();
        foreach (var npc in npcs)
        {
            RegisterNPC(npc);
        }
        Debug.Log($"GroupConversationManager initialized with {npcList.Count} NPCs.");

        playerTurn = false;
        playerRespondedToGreeting = false;
    }

    public void RegisterNPC(GroupChatGPT npc)
    {
        if (!npcList.Contains(npc))
        {
            npcList.Add(npc);
            Debug.Log($"{npc.gameObject.name} registered to conversation.");
        }
    }

    public void StartConversation()
    {
        if (npcList.Count > 0)
        {
            int randomIndex = Random.Range(0, npcList.Count);
            currentNPC = npcList[randomIndex];
            Debug.Log($"{currentNPC.gameObject.name} selected to greet the player.");

            if (!playerRespondedToGreeting)
            {
                currentNPC.GreetPlayer();
            }
        }
    }

    public GroupChatGPT ChooseRandomNPC()
    {
        if (npcList.Count > 0)
        {
            int randomIndex = Random.Range(0, npcList.Count);
            currentNPC = npcList[randomIndex];
            Debug.Log($"{currentNPC.gameObject.name} has been chosen as the active NPC.");
            return currentNPC;
        }
        else
        {
            Debug.LogWarning("No NPCs available to select.");
            return null;
        }
    }

    public void OnNPCFinishedSpeaking()
    {
        Debug.Log("NPC finished speaking.");

        if (currentNPC != null)
        {
            currentNPC.DeactivateNPC();
            currentNPC = null;
        }

        if (waitingCoroutine != null)
        {
            StopCoroutine(waitingCoroutine);
        }

        waitingCoroutine = StartCoroutine(WaitAndCheckForPlayerInput());
    }

    private IEnumerator WaitAndCheckForPlayerInput()
    {
        playerTurn = true;
        openAICanvas.SetActive(true);


        Debug.Log("Waiting for player input...");

        yield return new WaitForSeconds(responseTime);

        if (playerTurn)
        {
            if (!playerRespondedToGreeting)
            {
                Debug.Log("Player has not yet responded to greeting");

                yield break;
            }
            else
            {
                Debug.Log("Player did not respond. Moving to the next NPC.");
                playerTurn = false;
                StartConversation();
            }
        }
        else
        {
            Debug.Log("Player responded.");
        }
    }

    public void OnPlayerFinishedSpeaking(string playerMessage)
    {
        playerTurn = false;
        playerRespondedToGreeting = true;
        Debug.Log($"Player finished speaking: {playerMessage}");
        ProcessPlayerMessage(playerMessage);

        if (waitingCoroutine != null)
        {
            StopCoroutine(waitingCoroutine);
            waitingCoroutine = null;
        }

        StartConversation();
    }

    public void OnPlayerInterrupts()
    {
        Debug.Log("Player interrupted the conversation.");

        if (currentNPC != null)
        {
            currentNPC.DeactivateNPC();
            currentNPC = null;
        }

        playerTurn = true;

        if (waitingCoroutine != null)
        {
            StopCoroutine(waitingCoroutine);
            waitingCoroutine = null;
        }
    }

    private void ProcessPlayerMessage(string message)
    {
        Debug.Log($"Processing player's message: {message}");
    }

    public void PauseConversation()
    {
        Debug.Log("Pausing dialogue for all NPCs.");
        foreach (var npc in npcList)
        {
            npc.PauseDialogue();
            openAICanvas.SetActive(false);
        }
    }

    public void ResumeConversation()
    {
        Debug.Log("Resuming dialogue for the active NPC.");
        foreach (var npc in npcList)
        {
            npc.ResumeDialogue();
        }
    }
}