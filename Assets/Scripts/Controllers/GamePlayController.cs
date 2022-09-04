using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    public static event Action<GameState> OnGameStateChange;

    public Event gevent;

    public const int valuse = 10;
    public GameState state;
    public int Score { get; set; }

    [SerializeField] GameObject[] shipsPrefabs;
    GameObject playerPrefab;
    //public int shipPower;
    public int ShipPower { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        SelectShip();
        UpdateState(GameState.INIT);
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
            case GameState.WAVESTART:
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
            case GameState.WAVESTART:
                break;
            case GameState.MENU:
                break;
            case GameState.INIT:
                Time.timeScale = 1;
                LevelSpawner.instance.DestroyWaves();
                DestroyLevel();
                Score = 0;
                InitializePlayer();
                BeginIntroSequence();
                GameUIController.instance.UpdateScore(Score);
                break;
            case GameState.LOADLEVEL:
                // Debug.Log("LOADLEVEL");

                StageSpawner.instance.SpawnLevelWithIndex(GameDataManager.Instance.CurrentLevel);
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
                    GameDataManager.Instance.Save();
                }
                GameDataManager.Instance.Save();
                StartCoroutine(DelayRoutine());

                break;
            case GameState.DEFEAT:
                Time.timeScale = 0;
                break;

            case GameState.LEVELCOMPLETEUI:
                Time.timeScale = 1;
                break;
            case GameState.RETRY:
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
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
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {

            GameObject p = Instantiate(playerPrefab, new Vector3(0, -6, 0), Quaternion.identity);
            p.SetActive(false);
        }
        else
        {
            player.transform.position = new Vector3(0, -6, 0);
            player.gameObject.SetActive(false);
        }
    }

    IEnumerator PlayerStartingAnim()
    {
        yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(.5f));
        Player.instance.gameObject.SetActive(true);
        Player.instance.PlayerAnimation();
        yield return new WaitForSecondsRealtime(4f);

        UpdateState(GameState.LOADLEVEL);
    }
    void BeginIntroSequence()
    {
        StartCoroutine(PlayerStartingAnim());
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

    void SelectShip()
    {
        int index = GameDataManager.Instance.selectedShip;
        playerPrefab = shipsPrefabs[index];
        ShipPower = GameDataManager.Instance.shipsPower[index];
    }
}

public enum GameState
{
    WAVESTART,
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
