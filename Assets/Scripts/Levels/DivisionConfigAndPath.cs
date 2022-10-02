using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Division Config and Path")]
public class DivisionConfigAndPath : ScriptableObject
{
    [SerializeField] public int numberOfEnemies;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] GameObject formationPrefab;
    [SerializeField] bool isRotatingToForm;
    [SerializeField] bool isRotating;
    [SerializeField] bool isMovingHorizontal;

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
    public GameObject[] GetEnemyPrefabsList()
    {
        return enemyPrefabs;
    }
    public bool GetIsRotating()
    {
        return isRotating;
    }
    public bool GetIsMovingHorizontal()
    {
        return isMovingHorizontal;
    }


}
