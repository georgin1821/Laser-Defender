using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameUIController : MonoBehaviour
{

    public static GameUIController instance;

    [SerializeField] TMP_Text scoreText, lostTxt;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text waveText, victoryTxt;
    [SerializeField] TMP_Text introText;
    [SerializeField] TMP_Text gunRankText;
    [SerializeField] Button retryBtn, skill1Btn;
    [SerializeField] Button defeatScreenBtn;
    [SerializeField] Image semiTransperantImage;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject defeatPanel;
    [SerializeField] Slider healthSlider;

    [Header("Sound")]
    [SerializeField] AudioClip click1;
    [SerializeField] AudioClip click2;

    public Animator anim;
    public AnimationClip clip;

    #region Unity Functions
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
        defeatScreenBtn.onClick.RemoveListener(DefeatScreen);
    }

    private void Start()
    {
        coinsText.text = GameDataManager.Instance.coins.ToString();
        defeatScreenBtn.onClick.AddListener(DefeatScreen);
        UpdateHealthText();
    }
    void Update()
    {
        levelText.text = GameDataManager.Instance.CurrentLevel.ToString();
    }
    #endregion
    private void OnApplicationPause(bool pause)
    {
        // Debug.Log("pause");        
    }
    void OnGameStateChangeMenuActivation(GameState state)
    {

        defeatPanel.SetActive(state == GameState.DEFEAT);
        semiTransperantImage.gameObject.SetActive(state == GameState.DEFEAT);
        victoryTxt.gameObject.SetActive(state == GameState.LEVELCOMPLETE);
        switch (state)
        {
            case GameState.LEVELCOMPLETE_UI:
                GameManager.Instance.loadingFrom = LoadingFrom.LVLCOMP;
                LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");
                break;
            case GameState.DEFEAT:


                break;

        }
    }

    #region Unity Public
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
    public void UpdateHealthText()
    {
        int health = Player.instance.GameHealth;
        healthText.text = "" + health;
        healthSlider.value = health;

    }
    public void UpdateCoins(int coins, int coinsToAdd)
    {
        StartCoroutine(UpdateCoinsRoutine(coins, coinsToAdd));
    }
    public void RetryButton()
    {
        AudioController.Instance.PlayAudio(AudioType.UI_click_switch);
        GamePlayController.instance.UpdateState(GameState.INIT);
    }
    public void ResumeGame()
    {
        pausePanel.GetComponent<CoolDownCounter>().StartCountDown();
    }
    public void BackToMap()
    {
        Time.timeScale = 1;
        GamePlayController.instance.UpdateState(GameState.EXIT);
        LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");
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
        if (Player.instance.UpgradeRank < 9)
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
        GamePlayController.instance.UpdateState(GameState.PAUSE);
        pausePanel.SetActive(true);

    }
    public void SetPlayerStatus()
    {
        healthSlider.maxValue = Player.instance.GameHealth;
        healthSlider.value = Player.instance.GameHealth;
    }
    void DefeatScreen()
    {
        StartCoroutine(DefeatScreenRoutine());
    }
    IEnumerator DefeatScreenRoutine()
    {
        Time.timeScale = 1;
        defeatPanel.SetActive(false);
        semiTransperantImage.gameObject.SetActive(false);
        Player.instance.gameObject.SetActive(false);
        AudioController.Instance.PlayAudio(AudioType.DefeatClip);
        lostTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        Time.timeScale = 1;
        GameManager.Instance.loadingFrom = LoadingFrom.DEFEAT;
        LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");
    }
    #endregion


}

