using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public WaveConfig waveConfig;

    float speed;
    float rotationSpeed;

    public List<Transform> waypoints;
    public List<GameObject> waveEnemies;
    public float timeBetweenSpawns;

    private void Start()
    {
        InstatiateWave();
    }
    public void InstatiateWave()
    {
        waveEnemies = new List<GameObject>();

        waypoints = waveConfig.GetWaypoints();

        for (int index = 0; index < waveConfig.GetNumberOfEnemies(); index++)
        {
            GameObject newEnemy = Instantiate(waveConfig.enemyPrefab,
    waypoints[0].position,
    waveConfig.enemyPrefab.transform.rotation) as GameObject;
            speed = waveConfig.GetMoveSpeed();
            rotationSpeed = waveConfig.GetRotationSpeed();
            timeBetweenSpawns = waveConfig.GetTimeBetweenSpawns();
            newEnemy.GetComponent<EnemyPathfinding>().SetWaypoints(waypoints, speed, rotationSpeed);
            newEnemy.GetComponent<EnemyPathfinding>().StartDeploymentRoutine();
            newEnemy.SetActive(false);
            waveEnemies.Add(newEnemy);
        }
        StartCoroutine(WaveSpawner());
        WaveControllerAbstract.CountEnemiesEvent(waveEnemies.Count);

    }
    private void OnWaveSpawnCompleteHandler(WaveConfig waveConfig)
    {
        // waveEnemies = LevelSpawner.instance.waveEnemies;

        StartCoroutine(WaveSpawner());
    }

    IEnumerator WaveSpawner()
    {
        foreach (var enemy in waveEnemies)
        {
            // enemy.GetComponent<EnemyPathfinding>().SetWaypoints(waypoints, speed, rotationSpeed);
            // enemy.GetComponent<EnemyPathfinding>().StartDeploymentRoutine();
            enemy.SetActive(true);
            enemy.GetComponent<EnemyPathfinding>().StartDeploymentRoutine();

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

    }

}
