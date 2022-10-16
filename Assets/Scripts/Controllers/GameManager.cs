using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isSpeedLevel;
    [HideInInspector] public LoadingFrom loadingFrom;


}

public enum LoadingFrom
{
    MAIN,
    LVLCOMP,
    DEFEAT
}

