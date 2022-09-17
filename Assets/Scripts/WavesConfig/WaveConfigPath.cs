﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config Path")]
public class WaveConfigPath : ScriptableObject
{
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    public int numberOfEnemies;
    public GameObject[] enemyPrefabs;
    public GameObject pathPrefab;
    [SerializeField] GameObject formationPrefab;
    [SerializeField] bool isRotatingToForm;

    public List<Transform> GetFormWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in formationPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints; ;

    }

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
    public bool GetisRotatingToForm()
    {
        return isRotatingToForm;
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