using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit;

public class EnterParty : MonoBehaviour
{
    [Header("Welcome Canvas")]
    [SerializeField] private GameObject welcomeCanvas;
    [SerializeField] private Button scriptedButton;
    [SerializeField] private Button aiButton;

    [Header("Fade Canvas")]
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 150f;

    [Header("Player Movement")]
    [SerializeField] private GameObject xrRig;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;
    private LocomotionSystem locomotionSystem;
    private CharacterController characterController;
    private ActionBasedContinuousTurnProvider continuousTurnProvider;
    private InputActionManager inputActionManager;

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

    private void ScriptedRoutine()
    {
        OnButtonPress();
        StartCoroutine(EnterPartyRoutine(true));
    }

    private void AIRoutine()
    {
        OnButtonPress();
        StartCoroutine(EnterPartyRoutine(false));
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

    private IEnumerator EnterPartyRoutine(bool isScripted)
    {
        yield return StartCoroutine(FadeScreen(0, 100));

        if (isScripted)
        {
            //GameManager.Instance.LoadScene(6);
            gameManager.LoadScene(6);
        }
        else
        {
            //GameManager.Instance.LoadScene(7);
            gameManager.LoadScene(5);
        }
    }

    private IEnumerator FadeScreen(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color currentColor = fadeImage.color;

        if (elapsedTime < 3f)
        {
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

                fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                yield return null;
            }
        }
        else
        {
            fadeImage.color = Color.black;
        }
    }
}