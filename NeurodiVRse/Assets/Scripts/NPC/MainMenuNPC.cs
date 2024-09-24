using Meta.WitAi.TTS.Utilities;
using System.Collections;
using UnityEngine;

public class MainMenuNPC : MonoBehaviour
{
    public Animator animator;
    public TTSSpeaker ttsSpeaker;

    private void Start()
    {
        StartCoroutine(SayHello());
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
        ttsSpeaker.Stop();
        StartCoroutine(SayPickScenario());
    }

    public void StartSayAbout()
    {
        ttsSpeaker.Stop();
        StartCoroutine(SayAbout());
    }

    public void StopTalking()
    {
        animator.SetBool("IsTalking", false);
        ttsSpeaker.Stop();
    }

    public IEnumerator SayHello()
    {
        animator.SetBool("IsTalking", true);

        string welcome = "Welcome to Neurodiverse, the interactive virtual reality game for social skills development.";
        ttsSpeaker.Speak(welcome);

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator SayPickScenario()
    {
        animator.SetBool("IsTalking", true);

        string pickScenario = "Pick a scenario to practice your social skills in.";
        ttsSpeaker.Speak(pickScenario);

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator SayAbout()
    {
        animator.SetBool("IsTalking", true);

        string about = "This game has been designed to expose neurodivergent people to social scenarios to allow them to adapt social skills and interactions with neurotypical people.\r\n\r\nPlayers have opportunity to explore interactions with artificial intelligent characters in the game and the effectiveness of this teaching method is being researched.";
        ttsSpeaker.Speak(about);

        yield return new WaitForSeconds(1f);
    }

    private void HandlePlaybackComplete()
    {
        animator.SetBool("IsTalking", false);
    }
}
