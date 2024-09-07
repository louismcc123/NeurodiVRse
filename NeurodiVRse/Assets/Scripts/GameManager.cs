using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject endGameUI;
    public GameObject incompleteLevelUI;

    public Door door;

    public void PlayCafeLLMGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayCafeScriptedGame()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayPartyLLMGame()
    {
        SceneManager.LoadScene(3);
    }

    public void PlayPartyScriptedGame()
    {
        SceneManager.LoadScene(4);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void EndGame()
    {
        endGameUI.SetActive(true);
    }

    public void Continue()
    {
        door.HandleDoorInteraction();
        incompleteLevelUI.SetActive(false);
    }
}
