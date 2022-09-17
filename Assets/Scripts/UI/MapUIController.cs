using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapUIController : MonoBehaviour
{
    public static MapUIController instance;

    bool[] levels;
    [SerializeField] List<Image> lockIcons;
    [SerializeField] List<Button> levelButtons;
    [SerializeField] List<TMP_Text> levelText;

    [SerializeField] TMP_Text lvlCompleteInfo, scoreTxt, coinsRewardTxt, levelStartTxt;
    [SerializeField] TMP_Text coinText;

    [SerializeField] GameObject defeatPanel, lvlStartPanel, lvlCompletePanel;
    [SerializeField] GameObject easy, medium, hard;
    [SerializeField] AudioClip startClip;

    string level;
    string difficulty;
    Loading load;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        InitializeLevelMenu();

        if (GameManager.Instance.IsLoadingFromGameDefeat)
        {
        }

        switch (load)
        {
            case Loading.MAIN:
                lvlCompletePanel.SetActive(false);
        defeatPanel.SetActive(false);
            lvlStartPanel.SetActive(false);
                break;
            case Loading.LVLCOMP:
                lvlCompletePanel.SetActive(true);
                defeatPanel.SetActive(false);
                lvlStartPanel.SetActive(false);
                break;
            case Loading.DEFEAT:
                lvlCompletePanel.SetActive(false);
                defeatPanel.SetActive(true);
                lvlStartPanel.SetActive(false);
                break;

        }
    }

    void InitializeLevelMenu()
    {
        easy.GetComponent<Animator>().enabled = (GameDataManager.Instance.currentDifficulty == CurrentGameDifficulty.EASY);
        medium.GetComponent<Animator>().enabled = (GameDataManager.Instance.currentDifficulty == CurrentGameDifficulty.MEDIUM);
        hard.GetComponent<Animator>().enabled = (GameDataManager.Instance.currentDifficulty == CurrentGameDifficulty.HARD);

        coinText.text = "" + GameDataManager.Instance.coins;

        levels = GameDataManager.Instance.levels;

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i])
            {
                lockIcons[i].gameObject.SetActive(false);
            }
            else
            {
                lockIcons[i].gameObject.SetActive(true);

                levelButtons[i].interactable = false;
                levelText[i].gameObject.SetActive(false);
            }
        }
    }
    public void StageCompleteScreen()
    {
        lvlCompleteInfo.text = "Stage " + GameDataManager.Instance.LevelIndex + 1 + " " + GameDataManager.Instance.currentDifficulty;
        scoreTxt.text = GameDataManager.Instance.LevelScore.ToString();
        coinsRewardTxt.text = GameDataManager.Instance.LevelCoins.ToString();
    }
    public void SelectLevel()
    {
        level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (level)
        {
            case "Level0":
                GameDataManager.Instance.CurrentLevel = 0;
                break;
            case "Level1":
                GameDataManager.Instance.CurrentLevel = 1;
                break;
            case "Level2":
                GameDataManager.Instance.CurrentLevel = 2;
                break;
            case "Level3":
                GameDataManager.Instance.CurrentLevel = 3;
                break;
            case "Level4":
                GameDataManager.Instance.CurrentLevel = 4;
                break;
        }

        StartLevelPanelInfo();
    }
    public void LoadLevel()
    {
        AudioManager.Instance.PlayOneShotClip(startClip, 1);
        LoadingWithFadeScenes.Instance.LoadScene("Game");
    }
    public void HidePanels()
    {
        defeatPanel.SetActive(false);
        lvlCompletePanel.SetActive(false);
    }
    public void NextLevel()
    {
        StartLevelPanelInfo();
        lvlCompletePanel.SetActive(false);
        defeatPanel.SetActive(false);
        GameDataManager.Instance.CurrentLevel++;
    }
    public void BackToMainScene()
    {
        LoadingWithFadeScenes.Instance.LoadScene("MainScene");

    }
    public void SelectDifficulty()
    {
        difficulty = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        switch (difficulty)
        {

            case "Easy":
                GameDataManager.Instance.currentDifficulty = CurrentGameDifficulty.EASY;
                easy.GetComponentInParent<Animator>().enabled = true;
                medium.GetComponentInParent<Animator>().enabled = false;
                hard.GetComponentInParent<Animator>().enabled = false;

                break;
            case "Medium":
                GameDataManager.Instance.currentDifficulty = CurrentGameDifficulty.MEDIUM;
                easy.GetComponentInParent<Animator>().enabled = false;
                medium.GetComponentInParent<Animator>().enabled = true;
                hard.GetComponentInParent<Animator>().enabled = false;

                break;
            case "Hard":
                GameDataManager.Instance.currentDifficulty = CurrentGameDifficulty.HARD;
                easy.GetComponentInParent<Animator>().enabled = false;
                medium.GetComponentInParent<Animator>().enabled = false;
                hard.GetComponentInParent<Animator>().enabled = true;

                break;

        }

    }

    public void StartLevelPanelInfo()
    {
        lvlStartPanel.SetActive(true);
        levelStartTxt.text = GameDataManager.Instance.CurrentLevel.ToString();

    }
}
