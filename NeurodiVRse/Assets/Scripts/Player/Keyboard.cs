using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VRKeyboard : MonoBehaviour
{
    [SerializeField] private XRRayInteractor rightRayInteractor;
    [SerializeField] private XRRayInteractor leftRayInteractor;

    public GameObject keyboard;
    public InputField inputField;

    void Start()
    {
        keyboard.SetActive(false);

        rightRayInteractor.selectEntered.AddListener(OnSelectEnter);
        leftRayInteractor.selectEntered.AddListener(OnSelectEnter);
    }

    void OnDestroy()
    {
        rightRayInteractor.selectEntered.RemoveListener(OnSelectEnter);
        leftRayInteractor.selectEntered.RemoveListener(OnSelectEnter);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        Debug.Log("Select Enter Event Triggered");

        if (args.interactableObject.transform == inputField.transform)
        {
            Debug.Log("Input Field Selected");
            keyboard.SetActive(true);
        }
        else
        {
            Debug.Log("Selected Object: " + args.interactableObject.transform.name);
        }
    }
}
