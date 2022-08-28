using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{

    public static Levels instance;

    [SerializeField] List<LevelConfig> levels;

    public int LevelIndex { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SpawnLevelWithIndex(int index)
    {
        
        if (index < levels.Count)
        {

            LevelSpawner.instance.SetWavesOfLevel(levels[index].GetWaves());
            LevelSpawner.instance.SpawnTheLevel();

        }
    }
}
