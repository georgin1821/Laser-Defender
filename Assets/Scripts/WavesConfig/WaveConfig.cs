using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config Path")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] public List<SubWaveConfig> subWaveConfigs;

    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] bool isDeploingWithFormation;
    [SerializeField] float rotationSpeed;

    public List<Transform> GetWaypoints(int index)
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in subWaveConfigs[index].pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints; ;
    }
    public List<Transform> GetFormationWaypoints(int subWaveIndex)
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in subWaveConfigs[subWaveIndex].waveConfigFormation.formationPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints; ;
    }

    public int GetEnemisCountAllSubWaves()
    {
        int enemiesCount = 0;
        foreach (var subWave in subWaveConfigs)
        {
            enemiesCount += subWave.numberOfEnemies;
        }
        return enemiesCount;
    }

    public float GetTimeBetweenSpawns()
    {
        return timeBetweenSpawns;
    }
    public float GetSpawnRandomFactor()
    {
        return spawnRandomFactor;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public bool GetIsDeploingWithFormation()
    {
        return isDeploingWithFormation;
    }



}
