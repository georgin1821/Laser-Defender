using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndleesMove : MonoBehaviour
{
    [SerializeField] WaveConfigPath waveConfig;

    float speed;
    float rotationSpeed;

    List<Transform> waypoints;
    List<GameObject> waveEnemies;
    float timeBetweenSpawns;

    private void Start()
    {
        EnemyCount.instance.CountEnemiesAtScene(waveConfig.GetNumberOfEnemies());
        InstatiateWave();
    }
    public void InstatiateWave()
    {
        waveEnemies = new List<GameObject>();

        waypoints = waveConfig.GetWaypoints();
        speed = waveConfig.GetMoveSpeed();
        rotationSpeed = waveConfig.GetRotationSpeed();
        timeBetweenSpawns = waveConfig.GetTimeBetweenSpawns();

        for (int index = 0; index < waveConfig.GetNumberOfEnemies(); index++)
        {
            int i;
            if (index % 2 == 0)
            {
                i = 0;
            }
            else
            {
                i = 1;
            }

            GameObject newEnemy = Instantiate(waveConfig.enemyPrefabs[i],
    waypoints[0].position,
    Quaternion.identity) as GameObject;
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
