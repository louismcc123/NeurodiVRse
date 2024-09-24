using Meta.WitAi.TTS.Utilities;
using System.Collections;
using UnityEngine;

public class MainMenuNPC : MonoBehaviour
{
    public Animator animator;
    public TTSSpeaker ttsSpeaker;

    private string welcome = "Welcome to Neurodiverse, the interactive virtual reality game for social skills development.";
    private string pickScenario = "Pick a scenario to practice your social skills in.";
    private string about = "This game has been designed to expose neurodivergent people to social scenarios to allow them to adapt social skills and interactions with neurotypical people.\r\n\r\nPlayers have opportunity to explore interactions with artificial intelligent characters in the game and the effectiveness of this teaching method is being researched.";

    private void Start()
    {
        StartCoroutine(SayScript(welcome));
    }

    private void OnEnable()
    {
        if (ttsSpeaker != null)
        {
            ttsSpeaker.OnPlaybackCompleteEvent += HandlePlaybackComplete;
        }
    }

    private void OnDisable()
    {
        if (ttsSpeaker != null)
        {
            ttsSpeaker.OnPlaybackCompleteEvent -= HandlePlaybackComplete;
        }
    }

    public void StartSayPickScenario()
    {
        StartCoroutine(SayScript(pickScenario));
    }

    public void StartSayAbout()
    {
        StartCoroutine(SayScript(about));
    }

    public void StopTalking()
    {
        animator.SetBool("IsTalking", false);
        ttsSpeaker.Stop();
    }

    public IEnumerator SayScript(string script)
    {
        ttsSpeaker.Stop();
        animator.SetBool("IsTalking", true);
        ttsSpeaker.Speak(script);
        yield return new WaitForSeconds(1f);
    }

    private void HandlePlaybackComplete()
    {
        animator.SetBool("IsTalking", false);
    }
}
