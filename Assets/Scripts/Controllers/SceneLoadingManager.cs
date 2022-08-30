using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : Singleton<SceneLoadingManager>
{
    public static SceneLoadingManager instance;


    public void LoadStartMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
    }

    public void LaodGame()
    {
        GamePlayController.instance.ResetGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
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

