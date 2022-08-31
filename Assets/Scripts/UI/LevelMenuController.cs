using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelMenuController : MonoBehaviour
{
    public bool[] levels;
    public List<Image> lockIcons;
    public List<Button> levelButtons;
    public List<TMP_Text> levelText;


    public string level;
    public TMP_Text coinText;


    private void Awake()
    {


    }

    private void Start()
    {
        InitializeLevelMenu();
    }
    void InitializeLevelMenu()
    {

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

    public void LoadLevel()
    {
        level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (level)
        {

            case "Level0":
                    Debug.Log("Level0");
               // FadeScenes.Instance.PlayLoadingScene();
                GameDataManager.Instance.CurrentLevel = 0;
                break;
            case "Level1":
                //    Debug.Log("Level1");
                GameDataManager.Instance.LoadLevel(1);
                break;
            case "Level2":
                //  Debug.Log("Level2");
                GameDataManager.Instance.LoadLevel(2);
                break;
            case "Level3":
                //  Debug.Log("Level3");
                GameDataManager.Instance.LoadLevel(3);
                break;
            case "Level4":
                //  Debug.Log("Level3");
                GameDataManager.Instance.LoadLevel(4);
                break;
        }

        LoadingWithFadeScenes.Instance.setSceneName("Game");
        LoadingWithFadeScenes.Instance.FadeOut();
    }
}
