using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionPathAndFormation : MonoBehaviour
{
    [SerializeField] DivisionConfigAndPath waveConfig;

    List<Transform> waypoints;
    List<Transform> formationWaypoints;
    List<GameObject> waveEnemies;
    float timeBetweenSpawns;
    bool isRotating;

    private void Start()
    {
        EnemyCount.instance.CountEnemiesAtScene(waveConfig.GetNumberOfEnemies());
        isRotating = waveConfig.GetisRotatingToForm();
        InstatiateWave();
    }

    public void InstatiateWave()
    {
        EnemyPathfinding pathfinding = GetComponent<EnemyPathfinding>();

        waveEnemies = new List<GameObject>();
        GameObject[] enemyPrefabs = waveConfig.GetEnemyPrefabsList();
        waypoints = waveConfig.GetWaypoints();
        formationWaypoints = waveConfig.GetFormWaypoints();

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

            pathfinding = newEnemy.GetComponent<EnemyPathfinding>();
            pathfinding.Speed = waveConfig.GetMoveSpeed();
            pathfinding.RotationSpeed = waveConfig.GetRotationSpeed();
            pathfinding.FormationPosition = formationWaypoints[index].position;
            pathfinding.Waypoints = waypoints;

            timeBetweenSpawns = waveConfig.GetTimeBetweenSpawns();
            waveEnemies.Add(newEnemy);

        }

        StartCoroutine(WaveSpawner());
    }
    IEnumerator WaveSpawner()
    {
        foreach (var enemy in waveEnemies)
        {
            EnemyPathfinding pathfinding = enemy.GetComponent<EnemyPathfinding>();

            if (isRotating)
            {
               pathfinding.DeploymentWithFormation();
            }
            else
            {
                pathfinding.DeploymentFormNoRotation();
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
