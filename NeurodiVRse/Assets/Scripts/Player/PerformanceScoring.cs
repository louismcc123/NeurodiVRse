using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceScoring : MonoBehaviour
{
    public Transform playerCamera;
    public Transform npc;
    public float maxAngle = 30.0f;
    public float scorePenalty = 1.0f;
    public float checkInterval = 1.0f;
    private float timer;

    //public RecordAudio recordAudio;
    //public TextToSpeech textToSpeech;
    private PlayerStats playerStats;

    private void Awake()
    {
        //recordAudio = FindObjectOfType<RecordAudio>();
        //textToSpeech = FindObjectOfType<TextToSpeech>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0;
            CheckEyeContact();
            CheckTurnBasedDialogue();
        }
    }

    void CheckEyeContact()
    {
        //if (Microphone.isRecording) // || textToSpeech.npcTalking)
        {
            Vector3 directionToNPC = (npc.position - playerCamera.position).normalized;
            float angle = Vector3.Angle(playerCamera.forward, directionToNPC);

            if (angle > maxAngle)
            {
                playerStats.SubtactScore(scorePenalty);
                Debug.Log("Eye contact not maintained! Score penalty applied.");

                // make npc give verbal queues to say it is rude to not look at them when they are talking
                // give visual prompt to tell player to look at the npc
            }
        }
    }

    void CheckTurnBasedDialogue()
    {
        //if (recordAudio.isTalking && textToSpeech.npcTalking)
        { 
            playerStats.SubtactScore(scorePenalty);
            Debug.Log("Player talking over npc! Score penalty applied.");

            // make npc give verbal queues to say it is rude to talk over someone else
            // give visual prompt to tell player to wait for the npc top finish talking
            
        }
    }

    // here are more examples of how players score can be affected:
    // - how close they are stood to the npc (personal space)
    // - saying key phrases such as please and thank you, asking how the are, etc. (niceties)
    // - other social queues
 }
