using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndleesMove : MonoBehaviour
{
    public WaveConfigPath waveConfig;

    float speed;
    float rotationSpeed;

    public List<Transform> waypoints;
    public List<GameObject> waveEnemies;
    public float timeBetweenSpawns;

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
            
            GameObject newEnemy = Instantiate(waveConfig.enemyPrefabs[0],
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
