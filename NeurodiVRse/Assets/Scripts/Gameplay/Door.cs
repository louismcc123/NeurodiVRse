using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RayManager rayManager;
    [SerializeField] private InputActionReference aButton;

    public GameObject Player;
    public GameObject endGameCanvas;
    public GameObject incompleteLevelCanvas; 
    public TextMeshProUGUI scoreText;

    public Animator openAndClose; 
    public Animator openAndClose1;

    private PlayerStats playerStats;

    public bool open;

    void Awake()
    {
        open = false;
        playerStats = Player.GetComponent<PlayerStats>();
    }

    void OnEnable()
    {
        aButton.action.Enable();
    }

    void OnDisable()
    {
        aButton.action.Disable();
    }

    void Update()
    {
        if (rayManager != null)
        {
            if (aButton.action.triggered)
            {
                HandleDoorInteraction();
            }
        }
    }

    public void HandleDoorInteraction()
    {
        if (!open)
        {
            if (playerStats.hasCoffee)
            {
                StartCoroutine(Opening());
                EndGame();
            }
            else
            {
                ShowIncompleteLevelUI();
            }
        }
        else
        {
            StartCoroutine(Closing());
        }
    }

    public void HandleNPCLeaving()
    {
        StartCoroutine(NPCLeaving());
    }

    IEnumerator Opening()
    {
        openAndClose.Play("Opening");
        openAndClose1.Play("Opening");
        open = true;
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator Closing()
    {
        openAndClose.Play("Closing");
        openAndClose1.Play("Closing");
        open = false;
        yield return new WaitForSeconds(.5f);
    }
    public IEnumerator NPCLeaving()
    {
        if (!open)
        {
            openAndClose.Play("Opening");
            openAndClose1.Play("Opening");
            open = true;
        }
        yield return new WaitForSeconds(.5f);
        openAndClose.Play("Closing");
        openAndClose1.Play("Closing");
        open = false;
    }


    void EndGame()
    {
        endGameCanvas.SetActive(true);

        if (playerStats != null)
        {
            float playerScore = playerStats.GetCurrentScore();
            scoreText.text = "Score: " + playerScore + "/100";
        }

        gameManager.EndGame();
    }

    void ShowIncompleteLevelUI()
    {
        incompleteLevelCanvas.SetActive(true);
    }
}
