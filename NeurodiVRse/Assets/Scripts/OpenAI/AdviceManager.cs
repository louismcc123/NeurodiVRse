using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdviceManager : MonoBehaviour
{
    [SerializeField] private GameObject adviceCanvas;
    [SerializeField] private TextMeshProUGUI adviceText;

    private void Start()
    {
        adviceCanvas.SetActive(false);
    }

    public void DisplayAdvice(string advice)
    {
        if (string.IsNullOrEmpty(advice))
        {
            adviceCanvas.SetActive(false);
            return;
        }

        adviceText.text = advice;
        adviceCanvas.SetActive(true);
    }
}
