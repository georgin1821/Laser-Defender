using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config Path")]
public class WaveConfigPath : ScriptableObject
{
    public WaveConfigFormation waveConfigFormation;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    public int numberOfEnemies;
    public GameObject enemyPrefab;
    public GameObject pathPrefab;


    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints; ;
    }


    public int GetNumberOfEnemies()
    {
        return numberOfEnemies;
    }

    public float GetTimeBetweenSpawns()
    {
        return timeBetweenSpawns;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }




}
