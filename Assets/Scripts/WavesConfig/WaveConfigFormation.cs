using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveConfigFormation : MonoBehaviour
{
    public WaveConfigPath waveConfig;

    float speed;
    float rotationSpeed;

    public List<Transform> waypoints;
    public List<Transform> formationWaypoints;
    public List<GameObject> waveEnemies;
    public float timeBetweenSpawns;
    public int totalEnemies;
    bool isRotating;

    private void Start()
    {
        EnemyCount.instance.CountEnemiesAtScene(waveConfig.GetNumberOfEnemies());
        isRotating = waveConfig.GetisRotatingToForm();
        InstatiateWave();
    }
    public void InstatiateWave()
    {
        waveEnemies = new List<GameObject>();

        waypoints = waveConfig.GetWaypoints();
        formationWaypoints = waveConfig.GetFormWaypoints();

        for (int index = 0; index < waveConfig.GetNumberOfEnemies(); index++)
        {
            GameObject newEnemy = Instantiate(waveConfig.enemyPrefabs[0],
    waypoints[0].position,
    Quaternion.identity) as GameObject;

            speed = waveConfig.GetMoveSpeed();
            rotationSpeed = waveConfig.GetRotationSpeed();
            timeBetweenSpawns = waveConfig.GetTimeBetweenSpawns();
            newEnemy.GetComponent<EnemyPathfinding>().SetWaypoints(waypoints, speed, rotationSpeed);
            waveEnemies.Add(newEnemy);
            newEnemy.GetComponent<EnemyPathfinding>().formationPosition = formationWaypoints[index].position;

        }
        StartCoroutine(WaveSpawner());

    }

    IEnumerator WaveSpawner()
    {
        foreach (var enemy in waveEnemies)
        {
            if (isRotating)
            {
                StartCoroutine(enemy.GetComponent<EnemyPathfinding>().DeploymentRoutineForm());
            }
            else
            {
                StartCoroutine(enemy.GetComponent<EnemyPathfinding>().DeploymentRoutineFormnNoRotation());
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
