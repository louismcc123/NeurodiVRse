using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FootstepController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform xrRigTransform;

    public AudioClip[] footstepSounds;

    public float minTimeBetweenFootsteps = 0.3f;
    public float maxTimeBetweenFootsteps = 0.6f;
    private float timeSinceLastFootstep;
    private bool isWalking = false;

    private Vector3 lastPosition;

    public static FootstepController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                audioSource = mainCamera.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    Debug.LogError("AudioSource component is missing on the main camera. Please attach an AudioSource component.");
                }
            }
            else
            {
                Debug.LogError("Main camera not found. Please ensure there is a camera tagged as MainCamera in the scene.");
            }
        }

        if (xrRigTransform == null)
        {
            Debug.LogError("XR Rig Transform is not assigned. Please assign the XR Rig Transform in the inspector.");
        }

        lastPosition = xrRigTransform.position;
    }

    private void Update()
    {
        if (audioSource == null || xrRigTransform == null)
        {
            return;
        }

        isWalking = (xrRigTransform.position - lastPosition).magnitude > 0.01f;
        lastPosition = xrRigTransform.position;

        if (isWalking)
        {
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                if (footstepSounds.Length > 0)
                {
                    AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];

                    if (footstepSound != null)
                    {
                        audioSource.PlayOneShot(footstepSound);
                        timeSinceLastFootstep = Time.time;
                    }
                }
            }
        }
    }
}
