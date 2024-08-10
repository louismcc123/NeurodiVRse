using UnityEngine;
using System.Collections;

public class LeavingNPCBehaviour : MonoBehaviour
{
    public GameObject door;
    public CharacterController characterController;

    private Door doorScript;
    private bool isMovingToDoor = false;
    private bool isLeaving = false;

    private void Awake()
    {
        if (door != null)
        {
            doorScript = door.GetComponent<Door>();
        }
        else
        {
            Debug.LogError(gameObject.name + ": Door not assigned.");
        }
    }

    private void Update()
    {
        // Check if we are at the first waypoint and not already moving
        if (characterController.GetCurrentWaypoint() == 0 && !isMovingToDoor && !characterController.IsMoving())
        {
            StartCoroutine(MoveToDoor());
        }

        // Check if we have reached the second waypoint and not already leaving
        if (characterController.GetCurrentWaypoint() == 1 && !isLeaving && characterController.HasReachedCurrentWaypoint())
        {
            if (doorScript != null)
            {
                doorScript.HandleNPCLeaving();
            }

            StartCoroutine(LeaveCafe());
        }
    }

    private IEnumerator MoveToDoor()
    {
        isMovingToDoor = true;
        yield return new WaitForSeconds(4f);

        characterController.MoveToWaypoint(1);
    }

    private IEnumerator LeaveCafe()
    {
        isLeaving = true;
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
