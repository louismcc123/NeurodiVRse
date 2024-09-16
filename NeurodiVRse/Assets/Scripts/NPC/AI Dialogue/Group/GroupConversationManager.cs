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
        foreach (var npc in npcList)
        {
            npc.DeactivateNPC();
        }

        activeNPC = npcList[0];

        if (activeNPC == null)
        {
            Debug.LogError("Active NPC is null. Cannot begin group dialogue.");
            return;
        }

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
            else
            {
            ChooseNextSpeaker();
            BeginGroupDialogue();
            }

            yield return new WaitForSeconds(1f);
            currentResponseTime -= 1f;
        }
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

    public void NotifyConversationUpdate(GroupChatGPT newActiveNPC)
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

    public GroupChatGPT ChooseRandomNPC()
    {
        foreach (var npc in npcList)
        {
            npc.DeactivateNPC();
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

    public GroupChatGPT GetActiveNPC()
    {
        return activeNPC;
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

        if (activeNPC != null)
        {
            activeNPC.PauseDialogue();
        }
    }

    public void OnPlayerEnterTrigger()
    {
        if (firstEntry)
        {
            BeginGroupDialogue();
            firstEntry = false;
        }
        else if (Time.time - lastExitTime <= pauseDuration)
        {
            if (activeNPC != null)
            {
                activeNPC.ResumeDialogue();
            }
        }
        else
        {
            ChooseNextSpeaker();
            BeginGroupDialogue();
        }
    }
}
