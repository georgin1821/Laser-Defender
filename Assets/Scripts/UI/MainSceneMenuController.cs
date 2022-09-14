using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainSceneMenuController : MonoBehaviour
{
    [SerializeField] GameObject profilePanel, shopPanel, squapPanel;
    [SerializeField] AudioClip click1, click2;
    [SerializeField] Button shipBtn, upgradeBtn;
    [SerializeField] Sprite[] sprites;
    [SerializeField] TMP_Text powerText, upgradeInfo, coinsText;

    int selectedShip;
    public float amountUpgrade = 100;
    string shipName;

    private void Start()
    {
        coinsText.text = GameDataManager.Instance.coins.ToString();
        UpdateUpgradeInfo();
    }

    public void ShowProfilePanel()
    {
       AudioManager.Instance.PlayOneShotClip(click1, 1);
        if (profilePanel.activeInHierarchy)
        {
            profilePanel.SetActive(false);
        }
        else
        {
            profilePanel.SetActive(true);
            shopPanel.SetActive(false);
            squapPanel.SetActive(false);
        }
    }

    public void ShowShopPanel()
    {
       AudioManager.Instance.PlayOneShotClip(click1, 1);

        if (shopPanel.activeInHierarchy)
        {
            shopPanel.SetActive(false);
        }
        else
        {
            AudioSource.PlayClipAtPoint(click1, transform.position);
            shopPanel.SetActive(true);
            profilePanel.SetActive(false);
            squapPanel.SetActive(false);
        }
    }

    public void ShowHomeScreen()
    {
        AudioManager.Instance.PlayOneShotClip(click2, 1);
        profilePanel.SetActive(false);
        shopPanel.SetActive(false);
        squapPanel.SetActive(false);
    }

    public void ShowSquadPanel()
    {
       AudioManager.Instance.PlayOneShotClip(click2, 1);
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();

        squapPanel.SetActive(true);

    }

    public void LoadLevelScene()
    {
       AudioManager.Instance.PlayOneShotClip(click2, 1);
        LoadingWithFadeScenes.Instance.LoadScene("LevelSelect");

    }

    public void SelectShip()
    {
        shipName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (shipName)
        {
            case "Ship1":
                selectedShip = 0;
                UpdateUpgradeInfo();

                break;

            case "Ship2":
                selectedShip = 1;

                UpdateUpgradeInfo();

                break;

        }

    }

    public void UpgradeShip()
    {
        {
            switch (selectedShip)
            {
                case 0:

                    amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
                    if (GameDataManager.Instance.coins >= amountUpgrade)
                    {
                        Upgrade();
                    }

                    break;
                case 1:

                    amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
                    if (GameDataManager.Instance.coins >= amountUpgrade)
                    {
                        Upgrade();
                    }
                    break;
            }
        }
    }

    private void Upgrade()
    {

        GameDataManager.Instance.selectedShip = selectedShip;
        int power = GameDataManager.Instance.shipsPower[selectedShip] + 100;
        GameDataManager.Instance.shipsPower[selectedShip] = power;
        GameDataManager.Instance.coins -= (int)amountUpgrade;
        GameDataManager.Instance.shipsRank[selectedShip]++;
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();
        amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
        upgradeInfo.text = amountUpgrade.ToString();

        coinsText.text = GameDataManager.Instance.coins.ToString();

        GameDataManager.Instance.Save();
    }
    private void UpdateUpgradeInfo()
    {
        amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();
        shipBtn.image.sprite = sprites[selectedShip];

        upgradeInfo.text = amountUpgrade.ToString();
    }
}
