using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit;

public class EnterCafe : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator openAndClose;
    [SerializeField] private Animator openAndClose1;

    [Header("Welcome Canvas")]
    [SerializeField] private GameObject welcomeCanvas;
    [SerializeField] private Button scriptedButton;
    [SerializeField] private Button aiButton;

    [Header("Fade Canvas")]
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 150f;

    [Header("Player Movement")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform waypoint;
    [SerializeField] private GameObject xrRig;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;
    private LocomotionSystem locomotionSystem;
    private CharacterController characterController;
    private ActionBasedContinuousTurnProvider continuousTurnProvider;
    private InputActionManager inputActionManager;
    public float moveSpeed = 2f;
    private bool isMoving = false;

    [Header("Scene Management")]
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        scriptedButton.onClick.AddListener(ScriptedRoutine);
        aiButton.onClick.AddListener(AIRoutine);

        locomotionSystem = xrRig.GetComponent<LocomotionSystem>();
        characterController = xrRig.GetComponent<CharacterController>();
        continuousTurnProvider = xrRig.GetComponent<ActionBasedContinuousTurnProvider>();
        inputActionManager = xrRig.GetComponent<InputActionManager>();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveToWaypoint();
        }
    }

    private void ScriptedRoutine()
    {
        OnButtonPress();
        StartCoroutine(EnterCafeRoutine(true));
    }

    private void AIRoutine()
    {
        OnButtonPress();
        StartCoroutine(EnterCafeRoutine(false));
    }

    private void OnButtonPress()
    {
        welcomeCanvas.SetActive(false);
        fadeCanvas.SetActive(true);
        DisableXRComponents();
    }

    private void DisableXRComponents()
    {
        if (locomotionSystem != null)
            locomotionSystem.enabled = false;

        if (inputActionManager != null)
            inputActionManager.enabled = false;

        if (characterController != null)
            characterController.enabled = false;

        if (continuousTurnProvider != null)
            continuousTurnProvider.enabled = false;

        leftController.SetActive(false);
        rightController.SetActive(false);
    }

    private IEnumerator EnterCafeRoutine(bool isScripted)
    {
        OpenDoors();
        MoveToWaypoint();
        StartCoroutine(FadeScreen(0, 100));
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, waypoint.position) < 0.1f);
        fadeImage.color = Color.black;

        if (isScripted)
        {
            //GameManager.Instance.LoadScene(3);
            gameManager.LoadScene(3);
        }
        else
        {
            //GameManager.Instance.LoadScene(2);
            gameManager.LoadScene(2);
        }
    }

    private void MoveToWaypoint()
    {        
        isMoving = true;

        player.transform.position = Vector3.MoveTowards(player.transform.position, waypoint.position, moveSpeed * Time.deltaTime);

        Vector3 direction = (waypoint.position - player.transform.position).normalized;
        player.transform.rotation = Quaternion.LookRotation(direction);
    }

    private IEnumerator FadeScreen(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color currentColour = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            fadeImage.color = new Color(currentColour.r, currentColour.g, currentColour.b, alpha);
            yield return null;
        }

        fadeImage.color = Color.black;
    }

    private void OpenDoors()
    {
        openAndClose.Play("Opening");
        openAndClose1.Play("Opening");
    }
}
