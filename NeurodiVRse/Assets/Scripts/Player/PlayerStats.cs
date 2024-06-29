using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxScore;
    public float currentScore;

    //public ScoreBar scoreBar;
    public GameManager gameManager;

    private bool isDead;

    void Start()
    {
        maxScore = 100f;
        currentScore = maxScore;
        //scoreBar.SetSliderMax(maxScore);
        //scoreBar.SetSlider(currentScore);
    }

    public void SubtactScore(float amount)
    {
        Debug.Log($"Subtracting score: {amount}");
        UpdateScore(amount);
    }

    /*public void AddHealth(float amount)
    {
        Debug.Log($"Adding health: {amount}");
        UpdateScore(amount);
    }*/

    private void UpdateScore(float amount)
    {
        currentScore -= amount;

        if (currentScore <= 0 && !isDead)
        {
            Debug.Log("Player died.");
            isDead = true;
            gameObject.SetActive(false);
            gameManager.GameOver();
        }
        else if (currentScore >= 100)
        {
            currentScore = 100;
            Debug.Log("Player score reached maximum.");
        }

        Debug.Log("Player score adjusted. Current score: " + currentScore);
        //scoreBar.SetSlider(currentScore);
    }
}
