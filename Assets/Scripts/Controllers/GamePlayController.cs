using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Events;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    public static event Action<GameState> OnGameStateChange;

    public Event gevent;

    public const int valuse = 10;
    public GameState state;
    public int Score { get; set; }

    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //Using?
        LevelSpawner.NoEnemiesOnLevel += OnLevelSpawnCompleted;
    }
    private void OnDestroy()
    {
        LevelSpawner.NoEnemiesOnLevel -= OnLevelSpawnCompleted;
    }
    private void Start()
    {
        UpdateState(GameState.INIT);
    }

    void OnLevelSpawnCompleted()
    {
        if (GameDataManager.Instance.CurrentLevel < GameDataManager.Instance.levels.Length)
        {
           // UpdateState(GameState.DELAY);
            UpdateState(GameState.LEVELCOMPLETE);

        }
    }

    private void Update()
    {
        // testing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");
        }
        //
        switch (state)
        {
            case GameState.PREGAME:
                break;
            case GameState.MENU:
                break;
            case GameState.LOADLEVEL:
                break;
            case GameState.LEVELCOMPLETE:
                break;
            case GameState.RETRY:
                break;
            case GameState.INIT:
                break;
            case GameState.PLAY:
                break;
            case GameState.PAUSE:
                break;
            case GameState.DEFEAT:
                break;
        }
    }

    public void UpdateState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.PREGAME:
                break;
            case GameState.MENU:
                break;
            case GameState.INIT:
                InitializePlayer();
                Score = 0;
                DestroyLevel();
                GameUIController.instance.UpdateScore(Score);
                UpdateState(GameState.LOADLEVEL);
                break;
            case GameState.LOADLEVEL:
                // Debug.Log("LOADLEVEL");

                StagesToSpawn.instance.SpawnLevelWithIndex(GameDataManager.Instance.CurrentLevel);
                UpdateState(GameState.PLAY);
                break;
            case GameState.PLAY:
                Time.timeScale = 1;
                break;
            case GameState.LEVELCOMPLETE:
               // Time.timeScale = 0;

                int unlockedLevel = GameDataManager.Instance.CurrentLevel;
                if (!(unlockedLevel >= GameDataManager.Instance.levels.Length))
                {
                    GameDataManager.Instance.levels[unlockedLevel] = true;
                }
                GameDataManager.Instance.Save();
                StartCoroutine(DelayRoutine());

                break;
            case GameState.DEFEAT:
                Time.timeScale = 0;
                break;

            case GameState.LEVELCOMPLETEUI:
                break;
            case GameState.RETRY:
                break;
            case GameState.PAUSE:
                break;
        }

        OnGameStateChange?.Invoke(state);
        //StartCoroutine(DelayRoutine(newState));
    }

    IEnumerator DelayRoutine()
    {
        yield return new WaitForSecondsRealtime(2);
        UpdateState(GameState.LEVELCOMPLETEUI);
    }
    private void InitializePlayer()
    {
        //Debug.Log("Instatiate Player " + Player.instance);
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Instantiate(playerPrefab, new Vector3(0, -4, 0), Quaternion.identity);
        }
        else
        {
            Player.instance.transform.position = new Vector3(0, -4, 0);
        }
    }

    public void AddToScore(int scoreValue)
    {
        int currentScore = Score;
        Score += scoreValue;
        StartCoroutine(GameUIController.instance.UpdateScore(currentScore, Score));
    }

    public void ResetGame()
    {
        Score = 0;
    }

    void DestroyLevel()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length != 0)
        {
            foreach (var enemy in enemies)
            {
                // Debug.Log(enemies.Length + " Destroy Enemy");
                Destroy(enemy.gameObject);
            }
        }
    }

}

public enum GameState
{
    PREGAME,
    MENU,
    INIT,
    LOADLEVEL,
    PLAY,
    LEVELCOMPLETE,
    RETRY,
    PAUSE,
    DEFEAT,
    LEVELCOMPLETEUI
}
