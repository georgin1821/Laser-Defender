using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{

    public static GameUIController instance;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] GameObject defeatPanel;
    [SerializeField] Button retryBtn, pauseBtn, skill1Btn;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text introText;
    [SerializeField] TMP_Text gunRankText;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Slider healthSlider;

    [SerializeField] TMP_Text stageCompleteText;
    public GameObject stageCompletedPanel;

    [HideInInspector] public int currentScore;
    [HideInInspector] public int currentCoins;

    public int score;
    public Animator anim;
    public Animator anim1;

    public AnimationClip clip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GamePlayController.OnGameStateChange += OnGameStateChangeMenuActivation;
    }
    private void OnDestroy()
    {
        GamePlayController.OnGameStateChange -= OnGameStateChangeMenuActivation;
    }
    private void Start()
    {
        coinsText.text = GameDataManager.Instance.coins.ToString();
        UpdateHealthText();
    }

    void Update()
    {
        levelText.text = GameDataManager.Instance.CurrentLevel.ToString();
    }
    public void UpdateHealthText()
    {
        int health = Player.instance.GetHealth();
        healthText.text = "" + health;
        healthSlider.value = health;

    }
    public IEnumerator UpdateScore(int CurrentScore, int newScore)
    {
        while (CurrentScore < newScore)
        {
            CurrentScore = (int)Mathf.MoveTowards(CurrentScore, newScore, 1f);
            scoreText.text = CurrentScore.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public IEnumerator UpdateCoinsRoutine(int coins, int coinsToAdd)
    {
        while (coins < coinsToAdd)
        {
            coins = (int)Mathf.MoveTowards(coins, coinsToAdd, 4f);

            coinsText.text = coins.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public void UpdateCoins(int coins, int coinsToAdd)
    {
        StartCoroutine(UpdateCoinsRoutine(coins, coinsToAdd));
    }
    public void LoadNextStage()
    {
        //Debug.Log("Laod Next Level");
        GameDataManager.Instance.CurrentLevel++;
        GamePlayController.instance.UpdateState(GameState.LOADLEVEL);
    }
    void OnGameStateChangeMenuActivation(GameState state)
    {

        stageCompletedPanel.SetActive(state == GameState.LEVELCOMPLETEUI);
        defeatPanel.SetActive(state == GameState.DEFEAT);

        switch (state)
        {
            case GameState.LEVELCOMPLETEUI:
                stageCompleteText.text = "Stage " + GameDataManager.Instance.CurrentLevel + " Completed!";
                break;
        }
    }
    public void RetryButton()
    {
        GamePlayController.instance.UpdateState(GameState.INIT);
    }
    public void ResumeGame()
    {
        pausePanel.GetComponent<CoolDownCounter>().StartCountDown();
       // pausePanel.SetActive(false);
      //  GamePlayController.instance.UpdateState(GameState.PLAY);
    }
    public void BackToMapAfterDefeat()
    {
        // SceneManager.LoadScene("MainScene");
        LoadingWithFadeScenes.Instance.setSceneName("LevelSelect");
        LoadingWithFadeScenes.Instance.FadeOut();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = "" + score;
    }
    public void ShowWaveInfoText(int waveIndex, int wavesTotal)
    {

        if (waveIndex == 0)
        {
            introText.gameObject.SetActive(true);
            waveText.gameObject.SetActive(true);
        }
        else
        {
            waveText.gameObject.SetActive(true);
        }
        waveText.text = "WAVE " + (waveIndex + 1) + "/" + wavesTotal;
        Invoke("WaveTextDisable", 4);
    }
    void WaveTextDisable()
    {
        waveText.gameObject.SetActive(false);
        introText.gameObject.SetActive(false);

    }
    public void UpdateRankStatus()
    {
        if (Player.instance.UpgradeRank < 6)
        {
            gunRankText.text = Player.instance.UpgradeRank.ToString();
        }
        else
        {
            gunRankText.text = "MAX";
        }
    }
    public void OpenPausePanel()
    {
        if (GamePlayController.instance.state == GameState.PLAY)
        {
            GamePlayController.instance.UpdateState(GameState.PAUSE);
            pausePanel.SetActive(true);
        }

    }
    public void SetPlayerStatus()
    {
        healthSlider.maxValue = Player.instance.GetHealth();
        healthSlider.value = Player.instance.GetHealth();
    }

    public void Skill1()
    {
        Player.instance.Skill1();
    }
}

