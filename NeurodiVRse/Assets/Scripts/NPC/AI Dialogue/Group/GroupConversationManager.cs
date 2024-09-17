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