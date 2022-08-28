using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehavior : MonoBehaviour
{
    public static WaveBehavior instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        LevelSpawner.OnWaveSpawnComplete += OnWaveSpawnCompleteAction;
    }

    private void OnDestroy()
    {
        LevelSpawner.OnWaveSpawnComplete -= OnWaveSpawnCompleteAction;
    }

    private void OnWaveSpawnCompleteAction(WaveConfig waveConfig)
    {
    }
}
