using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamePlayController : MonoBehaviour
{
    [SerializeField] GameObject scrollingBg1, scrollingBg2;
    [SerializeField] GameObject[] shipsPrefabs;

    public static GamePlayController instance;
    public static event Action<GameState> OnGameStateChange;
    public const int valuse = 10;
    public GameState state;
    public GameDifficulty ganeDifficulty;
    public float difficulty;
    GameObject playerPrefab;
    GameObject sc1, sc2;
    public AudioType soundtrack;
    public int Score { get; set; }
    int levelScore;
    //int batterySpend;
    public int levelCoins { get; set; }
    public int ShipPower { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameDataManager.Instance.Save();
    }

    private void Start()
    {
        SelectShip();
        UpdateState(GameState.INIT);
        SetLevelDifficulty();
        AudioController.Instance.PlayAudio(AudioType.Soundtrack_1);
    }
    private void Update()
    {
        switch (state)
        {
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
            case GameState.INIT:
                Time.timeScale = 1;
                WaveSpawner.instance.DestroyWaves();
                DestroyLevel();
                InitializeScrollingBackrounds();
                InitializePlayer();
                BeginIntroSequence();
                Score = 0;
                levelScore = 0;
                levelCoins = 0;
               // batterySpend = 0;
                GameUIController.instance.UpdateScore(Score);
                ganeDifficulty = (GameDifficulty)GameDataManager.Instance.currentDifficulty;

                break;
            case GameState.LOADLEVEL:
                StageSpawner.instance.SpawnLevelWithIndex(GameDataManager.Instance.CurrentLevel);
                UpdateState(GameState.PLAY);
                break;
            case GameState.PLAY:
                Time.timeScale = 1;
                break;
            case GameState.LEVELCOMPLETE:
                Player.instance.StopShootingClip();

                int unlockedLevel = GameDataManager.Instance.CurrentLevel;
                if (!(unlockedLevel >= GameDataManager.Instance.levels.Length))
                {
                    GameDataManager.Instance.levels[unlockedLevel] = true;
                }

                GameDataManager.Instance.LevelCoins = levelCoins;
                GameDataManager.Instance.LevelScore = levelScore;
                GameDataManager.Instance.batteryLife -= 10;
                GameDataManager.Instance.LevelIndex = StageSpawner.instance.LevelIndex;
                GameDataManager.Instance.currentDifficulty = (CurrentGameDifficulty)ganeDifficulty;
                GameDataManager.Instance.Save();

                StartCoroutine(DelayRoutine());

                break;
            case GameState.DEFEAT:
                Time.timeScale = 0;
                Player.instance.StopShootingClip();
                break;

            case GameState.LEVELCOMPLETE_UI:
                break;
            case GameState.RETRY:
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
                Player.instance.StopShootingClip();
                break;
        }

        OnGameStateChange?.Invoke(state);
    }

    public void SetLevelDifficulty()
    {
        switch (ganeDifficulty)
        {
            case GameDifficulty.EASY:
                difficulty = 0;
                break;
            case GameDifficulty.MEDIUM:
                difficulty = 1.5f;
                break;
            case GameDifficulty.HARD:
                difficulty = 3f;
                break;
        }
    }
    private void InitializeScrollingBackrounds()
    {
        sc1 = Instantiate(scrollingBg1);
        sc2 = Instantiate(scrollingBg2);
    }
    IEnumerator DelayRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        UpdateState(GameState.LEVELCOMPLETE_UI);
    }
    private void InitializePlayer()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Instantiate(playerPrefab, new Vector3(0, -6, 0), Quaternion.identity);
        }
    }
    IEnumerator PlayerStartingAnim()
    {
        float t1, t2;
        if (GameManager.Instance.isSpeedLevel)
        {
            t1 = 0;
            t2 = 1;
        }
        else
        {
            t1 = 0.5f; t2 = 4f;
        }

        yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(t1));
        Player.instance.gameObject.SetActive(true);
        Player.instance.PlayerAnimation();
        yield return new WaitForSecondsRealtime(t2);

        UpdateState(GameState.LOADLEVEL);
    }
    void BeginIntroSequence()
    {
        StartCoroutine(PlayerStartingAnim());
    }
    public void AddToScore(int scoreValue)
    {
        int startScore = Score;
        Score += scoreValue;
        levelScore += scoreValue;
        StartCoroutine(GameUIController.instance.UpdateScore(startScore, Score));
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
        Destroy(sc1);
        Destroy(sc2);
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
    INIT,
    LOADLEVEL,
    PLAY,
    LEVELCOMPLETE,
    RETRY,
    PAUSE,
    DEFEAT,
    LEVELCOMPLETE_UI
}

public enum GameDifficulty
{
    EASY,
    MEDIUM,
    HARD
}
