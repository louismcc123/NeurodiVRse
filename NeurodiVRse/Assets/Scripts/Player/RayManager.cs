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