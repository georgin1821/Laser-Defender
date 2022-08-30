using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainSceneMenuController : MonoBehaviour
{
    [SerializeField]  Button shopBtn, upgradeBtn, profileBtn, mapBtn, homeBtn;
    [SerializeField] GameObject profilePanel, shopPanel;

    public void ShowProfilePanel()
    {
        if (profilePanel.activeInHierarchy)
        {
            profilePanel.SetActive(false);
        }
        else
        {
            profilePanel.SetActive(true);
            shopPanel.SetActive(false);
        }
    }

    public void ShowShopPanel()
    {
        if (shopPanel.activeInHierarchy)
        {
            shopPanel.SetActive(false);
        }
        else
        {
            shopPanel.SetActive(true);
            profilePanel.SetActive(false);
        }
    }

    public void ShowHomeScreen()
    {
        profilePanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void LoadLevelScene()
    {
        FadeScenes.Instance.FadeOut();
    }

}
