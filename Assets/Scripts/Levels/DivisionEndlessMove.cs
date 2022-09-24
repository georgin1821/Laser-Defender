using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionEndlessMove : MonoBehaviour
{
    [SerializeField] DivisionConfigAndPath waveConfig;

    float speed;
    float rotationSpeed;

    List<Transform> waypoints;
    List<GameObject> waveEnemies;
    float timeBetweenSpawns;

    private void Awake()
    {
        EnemyCount.instance.CountEnemiesAtScene(waveConfig.GetNumberOfEnemies());
    }
    private void Start()
    {
        InstatiateWave();
    }
    public void InstatiateWave()
    {
        waveEnemies = new List<GameObject>();

        waypoints = waveConfig.GetWaypoints();
        speed = waveConfig.GetMoveSpeed();
        rotationSpeed = waveConfig.GetRotationSpeed();
        timeBetweenSpawns = waveConfig.GetTimeBetweenSpawns();
        GameObject[] enemyPrefabs = waveConfig.GetEnemyPrefabsList();

        // first instatiate all enemies and then start deplyment
        for (int index = 0; index < waveConfig.GetNumberOfEnemies(); index++)
        {
            int i = 0;
            if (enemyPrefabs.Length > 1)
            {
                if (index % 2 == 0)
                {
                    i = 0;
                }
                else
                {
                    i = 1;
                }
            }

            GameObject newEnemy = Instantiate(enemyPrefabs[i],
                                              waypoints[0].position,
                                              enemyPrefabs[i].transform.rotation) as GameObject;

            newEnemy.GetComponent<EnemyPathfinding>().SetWaypoints(waypoints, speed, rotationSpeed);
            waveEnemies.Add(newEnemy);
        }

        StartCoroutine(WaveSpawner());
    }

    IEnumerator WaveSpawner()
    {
        foreach (var enemy in waveEnemies)
        {
            enemy.GetComponent<EnemyPathfinding>().StartDeploymentRoutine();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
