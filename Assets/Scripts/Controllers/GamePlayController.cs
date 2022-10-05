using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public sealed class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    [SerializeField] GameObject[] shipsPrefabs;
    [SerializeField] GameObject[] scrollingsBgsPrefabs;
    [SerializeField] GameObject[] sbgGameObjects;

    public static event Action<GameState> OnGameStateChange;
    public static event Action OnScrollingBGEnabled;
    public GameState state; //to be serialize only
    public GameDifficulty ganeDifficulty;
    public AudioType soundtrack;

    GameObject playerPrefab;
    int levelScore;
    int batterySpend;
    public float Difficulty { get; set; }
    public int Score { get; set; }
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
        SelectShipPrefabSetShipPower();
        SetLevelDifficulty();
        AudioController.Instance.PlayAudio(AudioType.Soundtrack_1);
        UpdateState(GameState.INIT);
    }
    private void Update()
    {
        //switch (state)
        //{
        //    case GameState.LOADLEVEL:
        //        break;
        //    case GameState.LEVELCOMPLETE:
        //        break;
        //    case GameState.RETRY:
        //        break;
        //    case GameState.INIT:
        //        break;
        //    case GameState.PLAY:
        //        break;
        //    case GameState.PAUSE:
        //        break;
        //    case GameState.DEFEAT:
        //        break;
        //}
    }

    public void UpdateState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.INIT:
                Time.timeScale = 1;
                DestroyLevel();
                WaveSpawner.instance.DestroyWaves();
                InitializeScrollingBackrounds();
                InitializePlayer();
                BeginIntroSequence();
                EnemyCount.instance.Count = 0;

                Score = 0;
                levelScore = 0;
                levelCoins = 0;
                batterySpend = 0;
                GameUIController.instance.UpdateScore(Score);
                ganeDifficulty = (GameDifficulty)GameDataManager.Instance.currentDifficulty;

                break;
            case GameState.LOADLEVEL:
                LevelSpawner.instance.SpawnLevelWithIndex(GameDataManager.Instance.CurrentLevel);
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
                GameDataManager.Instance.LevelIndex = LevelSpawner.instance.LevelIndex;
                GameDataManager.Instance.currentDifficulty = (CurrentGameDifficulty)ganeDifficulty;
                GameDataManager.Instance.Save();
                AudioController.Instance.StopAudio(soundtrack, true);
                StartCoroutine(DelayRoutine());

                break;
            case GameState.DEFEAT:
                Time.timeScale = 0;
                Player.instance.StopShootingClip();
                WaveSpawner.instance.StopAllCoroutines();
                break;

            case GameState.LEVELCOMPLETE_UI:
                break;
            case GameState.RETRY:
                Time.timeScale = 1;
                UpdateState(GameState.INIT);
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
                Player.instance.StopShootingClip();
                break;
            case GameState.EXIT:
                AudioController.Instance.StopAudio(soundtrack, true);
                break;
        }

        OnGameStateChange?.Invoke(state);
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
    public void SetLevelDifficulty()
    {
        switch (ganeDifficulty)
        {
            case GameDifficulty.EASY:
                Difficulty = 0;
                break;
            case GameDifficulty.MEDIUM:
                Difficulty = 1.5f;
                break;
            case GameDifficulty.HARD:
                Difficulty = 3f;
                break;
        }
    }

    IEnumerator DelayRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        UpdateState(GameState.LEVELCOMPLETE_UI);
    }

    private void InitializeScrollingBackrounds()
    {
        sbgGameObjects = new GameObject[scrollingsBgsPrefabs.Length];
        for (int i = 0; i < scrollingsBgsPrefabs.Length; i++)
        {
          sbgGameObjects[i] = Instantiate(scrollingsBgsPrefabs[i]);
        }
        OnScrollingBGEnabled?.Invoke();
    }
    private void InitializePlayer()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Instantiate(playerPrefab, new Vector3(0, -6, 0), Quaternion.identity);
        }

    }
    private void BeginIntroSequence()
    {
        StartCoroutine(PlayerStartingAnim());
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
    private void DestroyLevel()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length != 0)
        {
            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
        }
        foreach (var sbg in sbgGameObjects)
        {
            Destroy(sbg);
        }
    }
    private void SelectShipPrefabSetShipPower()
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
    LEVELCOMPLETE_UI,
    EXIT
}

public enum GameDifficulty
{
    EASY,
    MEDIUM,
    HARD
}
