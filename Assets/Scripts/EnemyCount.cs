using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{

    public static EnemyCount instance;
    public  int count = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public  void CountEnemiesAtScene(int number)
    {
        count += number;
    }



}
