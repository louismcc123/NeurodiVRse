using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxScore;
    public float currentScore;
    public bool hasCoffee;

    private bool isDead;

    public TextMeshProUGUI finalScoreText;
    public int totalScore = 0;

    //public ScoreBar scoreBar;
    public GameManager gameManager;

    void Start()
    {
        maxScore = 100f;
        currentScore = maxScore;
        //scoreBar.SetSliderMax(maxScore);
        //scoreBar.SetSlider(currentScore);

        hasCoffee = false;
    }

    public void SubtractScore(float amount)
    {
        Debug.Log($"Subtracting score: {amount}");
        UpdateScore(-amount);
    }

    /*public void AddHealth(float amount)
    {
        Debug.Log($"Adding health: {amount}");
        UpdateScore(amount);
    }*/

    private void UpdateScore(float amount)
    {
        currentScore += amount;

        if (currentScore <= 0 && !isDead)
        {
            Debug.Log("Player died.");
            isDead = true;
            gameObject.SetActive(false);
            gameManager.GameOver();
        }
        else if (currentScore >= maxScore)
        {
            currentScore = maxScore;
            Debug.Log("Player score reached maximum.");
        }

        Debug.Log("Player score adjusted. Current score: " + currentScore);
        //scoreBar.SetSlider(currentScore);
    }

    public float GetCurrentScore()
    {
        return currentScore;
    }

    public void DisplayFinalScore()
    {
        finalScoreText.text = "Final Score: " + totalScore;
        finalScoreText.gameObject.SetActive(true);
    }
}
