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
        audioSource = GetComponent<AudioSource>();
        lastPosition = xrRigTransform.position;
    }


    private void Update()
    {
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