using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsLoadingFromLevelComplete { get; set; }
    public bool IsLoadingFromGameDefeat { get; set; }
    public bool IsLoadingFromMainScene { get; set; }

    public bool isSpeedLevel;

    public Loading loading;

}
    public enum Loading
    {
        MAIN,
        LVLCOMP,
        DEFEAT
    }
