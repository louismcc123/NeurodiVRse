/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RayManager : MonoBehaviour
{
    [Header("Teleportation Rays")]
    public GameObject leftTeleportationRay;
    public GameObject rightTeleportationRay;

    [Header("Ray Interactors")]
    public GameObject leftRayInteractor;
    public GameObject rightRayInteractor;

    [Header("UI")]
    [SerializeField] private GameObject openUI;
    [SerializeField] private GameObject closeUI;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float rayActivationDistance = 2f;
    public Door door;

    [Header("Controls")]
    public InputActionProperty leftHandActivate;
    public InputActionProperty rightHandActivate;

    private void OnEnable()
    {
        leftHandActivate.action.Enable();
        rightHandActivate.action.Enable();
    }

    private void OnDisable()
    {
        leftHandActivate.action.Disable();
        rightHandActivate.action.Disable();
    }

    private void Update()
    {
        bool isUIOrDoor = CheckForUIOrDoor();

        if (!isUIOrDoor)
        {
            bool isTeleporting = leftHandActivate.action.ReadValue<float>() > 0.1f || rightHandActivate.action.ReadValue<float>() > 0.1f;
            SetTeleportationActive(isTeleporting);
        }
        else
        {
            SetTeleportationActive(false);
        }
    }

    private bool CheckForUIOrDoor()
    {
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayActivationDistance))
        {
            if (hit.collider != null && (hit.collider.CompareTag("Door") || hit.collider.CompareTag("UI")))
            {
                SetRayInteractorsActive(true);

                Door hitDoor = hit.collider.GetComponent<Door>();
                if (hitDoor != null && hitDoor == door)
                {
                    UpdateUI();
                }

                if (leftHandActivate.action.triggered || rightHandActivate.action.triggered)
                {
                    door.HandleDoorInteraction();
                }

                return true;
            }
        }

        SetRayInteractorsActive(false);
        DeactivateUI();
        return false;
    }

    private void SetTeleportationActive(bool isActive)
    {
        leftTeleportationRay.SetActive(isActive);
        rightTeleportationRay.SetActive(isActive);
    }

    private void SetRayInteractorsActive(bool isActive)
    {
        leftRayInteractor.SetActive(isActive);
        rightRayInteractor.SetActive(isActive);
    }

    private void UpdateUI()
    {
        if (door != null)
        {
            openUI.SetActive(!door.open);
            closeUI.SetActive(door.open);
        }
    }

    private void DeactivateUI()
    {
        openUI.SetActive(false);
        closeUI.SetActive(false);
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RayManager : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    public InputActionProperty leftDeactivate;
    public InputActionProperty rightDeactivate;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    private void Update()
    {
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftnumber, out bool leftValid);

        leftTeleportation.SetActive(!isLeftRayHovering && leftDeactivate.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);

        bool isRightRayHovering = rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightnumber, out bool rightValid);

        rightTeleportation.SetActive(!isRightRayHovering && rightDeactivate.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
    }
}