using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpinningMoney : MonoBehaviour
{
    public float rotationSpeed = 30f;
    private Rigidbody rb;
    private bool isBeingGrabbed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        var interactable = GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.onSelectEntered.AddListener(HandleGrab);
            interactable.onSelectExited.AddListener(HandleRelease);
        }
    }

    private void OnDisable()
    {
        var interactable = GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.onSelectEntered.RemoveListener(HandleGrab);
            interactable.onSelectExited.RemoveListener(HandleRelease);
        }
    }

    void Update()
    {
        if (!isBeingGrabbed)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void HandleGrab(XRBaseInteractor interactor)
    {
        isBeingGrabbed = true;
        if (rb != null)
        {
            rb.useGravity = true; 
        }
    }

    private void HandleRelease(XRBaseInteractor interactor)
    {
        isBeingGrabbed = false;
        if (rb != null)
        {
            rb.useGravity = false; 
        }
    }
}
