using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateDoorRay : MonoBehaviour
{
    [SerializeField] private GameObject openUI;
    [SerializeField] private GameObject closeUI;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float rayActivationDistance = 2f;

    public GameObject leftRay;
    public GameObject rightRay;

    public bool rayIsActive = false;

    public Door door;

    [SerializeField] private InputActionReference leftHandActivate;
    [SerializeField] private InputActionReference rightHandActivate;

    void OnEnable()
    {
        leftHandActivate.action.Enable();
        rightHandActivate.action.Enable();
    }

    void OnDisable()
    {
        leftHandActivate.action.Disable();
        rightHandActivate.action.Disable();
    }

    void Update()
    {
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayActivationDistance))
        {
            if (hit.collider != null && hit.collider.CompareTag("Door"))
            {
                leftRay.SetActive(true);
                rightRay.SetActive(true);
                rayIsActive = true;

                Door hitDoor = hit.collider.GetComponent<Door>();
                if (hitDoor != null && hitDoor == door)
                {
                    UpdateUI();
                }

                if (leftHandActivate.action.triggered || rightHandActivate.action.triggered)
                {
                    door.HandleDoorInteraction();
                }
            }
            else
            {
                DeactivateRay();
            }
        }
        else
        {
            DeactivateRay();
        }
    }

    void UpdateUI()
    {
        if (door != null)
        {
            openUI.SetActive(!door.open);
            closeUI.SetActive(door.open);
        }
    }

    void DeactivateRay()
    {
        leftRay.SetActive(false);
        rightRay.SetActive(false);
        rayIsActive = false;
        openUI.SetActive(false);
        closeUI.SetActive(false);
    }
}
