using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy SubWave Config")]

[Serializable]

public class SubWaveConfig : ScriptableObject
{
    public WaveConfigFormation waveConfigFormation;
    public GameObject pathPrefab;
    public int numberOfEnemies = 5;
    public GameObject enemyPrefab;


}
