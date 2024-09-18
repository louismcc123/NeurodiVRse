using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance;

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image loadingBar;

    public GameObject gameOverUI;
    public GameObject endGameUI;
    public GameObject incompleteLevelUI;

    public Door door;

   /* private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }*/

    /*public async void LoadScene(int sceneIndex)
    {
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;

        loadingCanvas.SetActive(true);
        loadingBar.fillAmount = 0; // Ensure loading bar starts from 0

        while (!scene.isDone)
        {
            loadingBar.fillAmount = Mathf.Clamp01(scene.progress / 0.9f);

            // Check if the scene is almost loaded
            if (scene.progress >= 0.9f)
            {
                // Optionally, you could add a small delay to make sure everything is ready
                //await Task.Delay(500);
                scene.allowSceneActivation = true; // Allow the scene to activate
            }
            else
            {
                await Task.Delay(100);  // Wait before updating progress
            }
        }

        // Ensure loading canvas is deactivated only after scene is fully loaded
        loadingCanvas.SetActive(false);
    }*/

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Back()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    // SCENE INDEXES
    // MainMenu (0)
    // CafeIntro (1)
    // CafeLLM (2)
    // CafeScripted (3)
    // PartyIntro (4)
    // PartyLLM (5)
    // PartyScripted (6)