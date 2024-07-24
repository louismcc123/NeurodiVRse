using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ActivateDoorRay activateDoorRay;
    [SerializeField] private InputActionReference rightHandActivate;

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
        rightHandActivate.action.Enable();
    }

    void OnDisable()
    {
        rightHandActivate.action.Disable();
    }

    void Update()
    {
        if (activateDoorRay != null && activateDoorRay.rayIsActive)
        {
            if (rightHandActivate.action.triggered)
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
