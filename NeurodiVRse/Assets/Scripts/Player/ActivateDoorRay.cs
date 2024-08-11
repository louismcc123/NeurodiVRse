using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActivateDoorRay : MonoBehaviour
{
    [SerializeField] private float rayActivationDistance = 2f;

    public GameObject leftRay;
    public GameObject rightRay;

    public Door door;

    [SerializeField] private InputActionReference aButton;

    void OnEnable()
    {
        aButton.action.Enable();
    }

    void OnDisable()
    {
        aButton.action.Disable();
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayActivationDistance, Color.red);


        if (Physics.Raycast(ray, out hit, rayActivationDistance))
        {
            Debug.Log("Physics.Raycast(ray, out hit, rayActivationDistance)");

            if (hit.collider != null && hit.collider.CompareTag("Door"))
            {
                Debug.Log("hit.collider != null && hit.collider.CompareTag(\"Door\")");

                leftRay.SetActive(true);
                rightRay.SetActive(true);

                Door hitDoor = hit.collider.GetComponent<Door>();
                if (hitDoor != null && hitDoor == door)
                {
                    Debug.Log("hitDoor != null && hitDoor == door");

                    if (!door.interactCanvas.activeSelf)
                    {
                        Debug.Log("!door.interactCanvas.activeSelf");
                        door.interactCanvas.SetActive(true);
                    }
                }

                if (aButton.action.triggered)
                {
                    door.HandleDoorInteraction();
                }
            }
            else
            {
                Debug.Log("else (hit.collider != null && hit.collider.CompareTag(\"Door\"))");

                DeactivateRay();
            }
        }
        else
        {
            Debug.Log("else Physics.Raycast(ray, out hit, rayActivationDistance)");

            DeactivateRay();
        }
    }

    private void DeactivateRay()
    {
        Debug.Log("ActivateDoorRay.DeactiveRay called.");
        leftRay.SetActive(false);
        rightRay.SetActive(false);

        if (door.interactCanvas.activeSelf)
        {
            door.interactCanvas.SetActive(false);
            Debug.Log("activate door ray: door.interactCanvas.SetActive(false)");
        }
    }
}
