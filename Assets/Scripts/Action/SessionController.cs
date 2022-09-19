
using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class SessionController : MonoBehaviour
{
    public static SessionController instance;

    [SerializeField] TMP_Text timeText;

    public bool[] rewardsChecked;
    public bool dailyRewardsReady = false;
    public event Action<int> OnDailyReawardReady;

    private DateTime localTime;
    private void Awake()
    {
        Configure();
        rewardsChecked = new bool[30];
        StartCoroutine(UpdateTimePanel());
    }
    IEnumerator UpdateTimePanel()
    {
        while (true)
        {
        localTime = DateTime.Now;
        string time = localTime.ToString("d/M H:mm");
        timeText.text = time;
        yield return new WaitForSeconds(60);
        }
    }
    private void OnApplicationFocus(bool _focus)
    {
        if (_focus)
        {
            // Open a window to unpause the game
            // PageController.instance.TurnPageOn(PageType.PausePopup);
        }
        else
        {
            // Flag the game paused
            // m_IsPaused = true;
        }
    }

#pragma warning disable UNT0001 // Empty Unity message
    private void Update()
#pragma warning restore UNT0001 // Empty Unity message
    {
        // if (m_IsPaused) return;
    }
    public void RewardCheckOnStart(DateTime sessionTimne, DateTime nextSessionTime)
    {
        //t1.text = sessionTimne.ToString("HH:mm");
       // t2.text = nextSessionTime.ToString("d:H:mm");
        int i = 0;
        if (nextSessionTime.Minute != sessionTimne.Minute)
        {
            dailyRewardsReady = true;
            rewardsChecked[i] = true;
            OnDailyReawardReady?.Invoke(i);
        }

    }
    #region Public Functions
    // public void InitializeGame(GameController _game) {
    //     m_Game = _game;
    //     m_Game.OnInit();
    // }

    public void UnPause()
    {
        // m_IsPaused = false;
    }
    #endregion

    #region Private Functions
    private void Configure()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}


    #endregion



