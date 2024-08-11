/*using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RayManager rayManager;
    [SerializeField] private InputActionReference aButton;

    public GameObject Player;
    public GameObject interactCanvas;
    public GameObject endGameCanvas;
    public GameObject incompleteLevelCanvas;
    public TextMeshProUGUI scoreText;

    public Animator openAndClose;
    public Animator openAndClose1;

    public bool open;
    private bool playerInRange;
    private bool rayPointingAtDoor;

    private PlayerStats playerStats;

    void Awake()
    {
        open = false;
        playerInRange = false;
        rayPointingAtDoor = false;
        playerStats = Player.GetComponent<PlayerStats>();
        interactCanvas.SetActive(false);
    }

    void OnEnable()
    {
        aButton.action.Enable();
        rayManager.OnRayHoverEnter += HandleRayHoverEnter;
        rayManager.OnRayHoverExit += HandleRayHoverExit;
    }

    void OnDisable()
    {
        aButton.action.Disable();
        rayManager.OnRayHoverEnter -= HandleRayHoverEnter;
        rayManager.OnRayHoverExit -= HandleRayHoverExit;
    }

    void Update()
    {
        if ((playerInRange && IsPlayerFacingDoor()) || rayPointingAtDoor)
        {
            interactCanvas.SetActive(true);

            if (aButton.action.triggered)
            {
                HandleDoorInteraction();
            }
        }
        else
        {
            interactCanvas.SetActive(false);
        }
    }

    private void HandleRayHoverEnter(GameObject hoveredObject)
    {
        if (hoveredObject == gameObject)
        {
            rayPointingAtDoor = true;
        }
    }

    private void HandleRayHoverExit(GameObject hoveredObject)
    {
        if (hoveredObject == gameObject)
        {
            rayPointingAtDoor = false;
        }
    }

    public void PlayerInRange(bool inRange)
    {
        playerInRange = inRange;
    }

    private bool IsPlayerFacingDoor()
    {
        Vector3 directionToDoor = (transform.position - Player.transform.position).normalized;
        float dotProduct = Vector3.Dot(Player.transform.forward, directionToDoor);

        return dotProduct > 0.5f;
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RayManager rayManager;
    [SerializeField] private InputActionReference aButton;

    public GameObject Player;
    public GameObject interactCanvas;
    public GameObject endGameCanvas;
    public GameObject incompleteLevelCanvas;
    public TextMeshProUGUI scoreText;

    public Animator openAndClose;
    public Animator openAndClose1;

    public  bool open;
    private bool playerInRange;
    private bool rayPointingAtDoor;

    private PlayerStats playerStats;

    void Awake()
    {
        open = false;
        playerInRange = false;
        rayPointingAtDoor = false;
        playerStats = Player.GetComponent<PlayerStats>();
        interactCanvas.SetActive(false);
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
        if (playerInRange && IsPlayerFacingDoor())
        {
            if (!interactCanvas.activeSelf)
            {
                interactCanvas.SetActive(true);
            }

            if (aButton.action.triggered)
            {
                HandleDoorInteraction();
            }
        }
        else
        {
            interactCanvas.SetActive(false);
        }
    }

    private bool IsPlayerFacingDoor()
    {
        Vector3 directionToDoor = (transform.position - Player.transform.position).normalized;
        float dotProduct = Vector3.Dot(Player.transform.forward, directionToDoor);

        return dotProduct > 0.5f;
    }

    public void PlayerInRange(bool inRange)
    {
        playerInRange = inRange;
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
