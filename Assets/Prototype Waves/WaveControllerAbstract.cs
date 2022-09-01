using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class WaveControllerAbstract : MonoBehaviour
{
    public static WaveControllerAbstract instance;
     public static int count = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public static void CountEnemiesEvent(int number)
    {
        count += number;

    }
}
