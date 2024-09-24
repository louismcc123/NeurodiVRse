/*using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image loadingBar;

    private float target;

    public GameObject gameOverUI;
    public GameObject endGameUI;
    public GameObject incompleteLevelUI;

    public Door door;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameOverUI = GameObject.Find("GameOverCanvas");
        endGameUI = GameObject.Find("EndGameCanvas");
        incompleteLevelUI = GameObject.Find("IncompleteLevelCanvas");
        door = FindObjectOfType<Door>();
    }

    private void Update()
    {
        loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, target, 3 * Time.deltaTime);
    }

    public async void LoadScene(int sceneIndex)
    {
        target = 0;
        loadingBar.fillAmount = 0;
        loadingCanvas.SetActive(true);

        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;


        do
        {
            //await Task.Delay(100);
            target = scene.progress;
        }
        while (scene.progress >= 0.9f);
            
                scene.allowSceneActivation = true; 
            
        loadingCanvas.SetActive(false);
    }

    /*public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }*/

/*public void Back()
{
    LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
}*/

/*public void Quit()
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
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private GameObject incompleteLevelUI;
    //[SerializeField] private GameObject controllers;

    public Door door;

    public void LoadScene(int sceneIndex)
    {
        loadingUI.SetActive(true);
        SceneManager.LoadScene(sceneIndex);
    }

    /*public void Back()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }*/

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        loadingUI.SetActive(true);
        //controllers.SetActive(false);
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