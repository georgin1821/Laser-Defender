using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

   // [SerializeField] float transistionSmoothTime = 0.1f;
   // public CanvasGroup canvasGroup;
   // string targetScene;

    private void Awake()
    {
        MakeSingleton();
        //canvasGroup = GetComponent<CanvasGroup>();
    }
    private void MakeSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
           // DontDestroyOnLoad(instance);
        }
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void LaodGame()
    {
        GamePlayController.instance.ResetGame();
        SceneManager.LoadScene("Game");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
    
}

