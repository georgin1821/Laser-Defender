using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{

    public static GameUIController instance;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] GameObject defeatPanel;
    [SerializeField] Button retryBtn;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text startWaveText;


    [SerializeField] TMP_Text stageCompleteText;
    public GameObject stageCompletedPanel;


    Player player;
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
        player = FindObjectOfType<Player>();
        coinsText.text = GameDataManager.Instance.coins.ToString();
        UpdateHealthText();
    }

    void Update()
    {
        levelText.text = GameDataManager.Instance.CurrentLevel.ToString();
    }

    public void UpdateHealthText()
    {
        healthText.text = player.GetHealth().ToString();
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

    public void UpdateScore(int score)
    {
        scoreText.text = "" + score;
    }

    public void ShowWaveInfoText(int waveIndex, int wavesTotal)
    {
        waveText.gameObject.SetActive(true);
        startWaveText.gameObject.SetActive(true);

        if (waveIndex == 1)
        {
            anim1.Play("wavetextanim");
            StartCoroutine(DelayAnimation());
        }
        else
        {
            anim.Play("wavetextanim");
        }
            waveText.text = "WAVE " + waveIndex + "/" + wavesTotal;
        Invoke("WaveTextDisable", 6);
    }

    IEnumerator DelayAnimation()
    {
        yield return StartCoroutine(MyCoroutine.WaitForRealSeconds(2));
        anim.Play("wavetextanim");

    }
    void WaveTextDisable()
    {
        waveText.gameObject.SetActive(false);
        startWaveText.gameObject.SetActive(false);

    }
}

