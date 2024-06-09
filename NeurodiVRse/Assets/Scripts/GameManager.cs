using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public void GameOver()
    {
        gameOverUI.SetActive(true);

        Invoke("LoadVRRestartScene", 3f);
    }

    public void Win()
    {
        gameWinUI.SetActive(true);

        Invoke("LoadVRRestartScene", 4f);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void LoadVRRestartScene()
    {
        SceneManager.LoadScene(3);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
