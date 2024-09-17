using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupConversationManager : MonoBehaviour
{
    [Header("Group ChatGPT")]
    [SerializeField] private List<GroupChatGPT> npcList;
    [SerializeField] GroupChatGPT activeNPC;

    [Header("Dialogue Settings")]
    [SerializeField] private float pauseDuration = 5f;
    [SerializeField] private float extendedResponseTime = 12f;
    [SerializeField] private List<Button> speechButtons;
    public bool playerResponded = false;
    public bool greetingPlayed = false;

    private float currentResponseTime;
    private float lastExitTime;
    private bool firstEntry = true;
    private bool isConversationActive = false;
    private Coroutine waitingCoroutine;

    public Transform player;

    private void Start()
    {
        foreach (var button in speechButtons)
        {
            button.onClick.AddListener(OnButtonPressed);
        }

        foreach (var npc in npcList)
        {
            RegisterNPC(npc);
        }

        //BeginGroupDialogue();
    }

    public void RegisterNPC(GroupChatGPT npc)
    {
        if (!npcList.Contains(npc))
        {
            npcList.Add(npc);
        }
    }

    public void BeginGroupDialogue()
    {
        activeNPC = npcList[0];
        ChooseRandomNPC();
        playerResponded = false;

        if (!greetingPlayed)
        {
            activeNPC.GreetPlayer();
        }

        currentResponseTime = pauseDuration;

        if (waitingCoroutine != null)
        {
            StopCoroutine(waitingCoroutine);
        }

        waitingCoroutine = StartCoroutine(HandlePlayerResponse());
    }

    private IEnumerator HandlePlayerResponse()
    {
        float startTime = Time.time;

        while (Time.time - startTime < pauseDuration)
        {
            if (playerResponded)
            {
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }

        ChooseNextSpeaker();
        BeginGroupDialogue();
    }

    public void NotifyNPCsToFaceSpeaker()
    {
        foreach (var npc in npcList)
        {
            GroupNPCBehaviours npcBehaviours = npc.GetComponent<GroupNPCBehaviours>();

            if (npcBehaviours != null)
            {
                if (!greetingPlayed)
                {
                    npcBehaviours.FaceSpeaker(player.transform);
                }
                else
                {
                    if (npc != activeNPC)
                    {
                        npcBehaviours.FaceSpeaker(activeNPC.transform);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"GroupNPCBehaviours component not found on {npc.name}");
            }
        }
    }

    /*public void NotifyConversationUpdate(GroupChatGPT newActiveNPC)
    {
        foreach (var npc in npcList)
        {
            if (npc != newActiveNPC)
            {
                npc.DeactivateNPC();
            }
        }

        activeNPC = newActiveNPC;
        activeNPC.ActivateNPC();
        activeNPC.SetNpcTalking(true);
    }*/

    public GroupChatGPT ChooseRandomNPC()
    {
        if (npcList == null || npcList.Count == 0)
        {
            Debug.LogWarning("No NPCs available to select.");
            return null;
        }

        int randomIndex = Random.Range(0, npcList.Count);
        activeNPC = npcList[randomIndex];

        Debug.Log($"{activeNPC.gameObject.name} has been chosen as the active NPC.");
        return activeNPC;
    }

    public void ChooseNextSpeaker()
    {
        ChooseRandomNPC();
        activeNPC.ActivateNPC();
        NotifyNPCsToFaceSpeaker();
    }

    public void HandleInterruption()
    {
        if (activeNPC != null)
        {
            activeNPC.DeactivateNPC();
        }

        PauseAllNPCs();
    }

    public void PauseAllNPCs()
    {
        foreach (var npc in npcList)
        { 
            if (!npc.isDialoguePaused)
            {
                npc.PauseDialogue();
            }
        }
    }

    private void OnButtonPressed()
    {
        //Debug.Log("Player speaking/typing. Resetting timer and extending response time.");
        ResetTimerAndExtendResponseTime();
    }

    private void ResetTimerAndExtendResponseTime()
    {
        if (waitingCoroutine != null)
        {
            StopCoroutine(waitingCoroutine);
        }

        currentResponseTime = extendedResponseTime;
        waitingCoroutine = StartCoroutine(HandlePlayerResponse());
    }

    public void OnPlayerExitTrigger()
    {
        lastExitTime = Time.time;

        foreach (var npc in npcList)
        {
            npc.PauseDialogue();
        }

        isConversationActive = false;
    }

    public void OnPlayerEnterTrigger()
    {
        if (firstEntry)
        {
            BeginGroupDialogue();
            firstEntry = false;
            isConversationActive = true; 
        }
        else if (Time.time - lastExitTime <= pauseDuration && isConversationActive)
        {
            if (activeNPC != null)
            {
                activeNPC.ResumeDialogue();
            }
        }
        else if (Time.time - lastExitTime > pauseDuration)
        {
            ChooseNextSpeaker();
            BeginGroupDialogue();
            isConversationActive = true; 
        }
    }
}


/*using System.Collections;
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

    public void OnPlayerExitTrigger()
    {

    }

    public void OnPlayerEnterTrigger()
    {

    }
}
*/