using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{

    public static LevelSpawner instance;

    [HideInInspector] public int totalEnemiesPerWave;

    List<Transform> waypoints;
    List<WaveConfig> waveConfigs;


    int startingWave = 0;

    public static event Action NoEnemiesOnLevel;
    public static event Action<WaveConfig> OnWaveSpawnComplete;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SpawnTheLevel()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnLevel());
    }
    public IEnumerator SpawnLevel()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnWaves(currentWave));
        }

        NoEnemiesOnLevel?.Invoke();
        //End of level

    }
    IEnumerator SpawnWaves(WaveConfig waveConfig)
    {

        totalEnemiesPerWave = waveConfig.GetEnemisCountAllSubWaves();

        // Info text
        ShowWaveStartingInfo(waveConfig);
        // 0 for speed running
        yield return new WaitForSeconds(0);

        // start coroutine for each subWave in wave
        for (int index = 0; index < waveConfig.subWaveConfigs.Count; index++)
        {
            // yield return StartCoroutine(SubWaveDeployment(waveConfig, index, waveConfig.subWaveConfigs[index].numberOfEnemies));
            StartCoroutine(SubWaveDeployment(waveConfig, index, waveConfig.subWaveConfigs[index].numberOfEnemies));
        }

        OnWaveSpawnComplete?.Invoke(waveConfig);

        yield return StartCoroutine(NoEnemiesOnWave());
        //End of wave

    }
    IEnumerator SubWaveDeployment(WaveConfig waveConfig, int subWaveIndex, int totalEnemies)
    {
        waypoints = waveConfig.GetWaypoints(subWaveIndex);
        for (int enemyCount = 0; enemyCount < totalEnemies; enemyCount++)
        {
            //instance enemy prefab from SubWaveConfig at the first waypoint of SubWaveConfig
            GameObject newEnemy = Instantiate(waveConfig.subWaveConfigs[subWaveIndex].enemyPrefab,
    waypoints[0].position,
    waveConfig.subWaveConfigs[subWaveIndex].enemyPrefab.transform.rotation) as GameObject;

            newEnemy.GetComponent<EnemyPathfinding>().SetWaveConfig(waveConfig, subWaveIndex);
            newEnemy.GetComponent<EnemyPathfinding>().StartDeploymentRoutine();
            newEnemy.GetComponent<Enemy>().placeAtWave = enemyCount;


            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());

        }
    }

    private void ShowWaveStartingInfo(WaveConfig waveConfig)
    {
        int wavePos = waveConfigs.IndexOf(waveConfig) + 1;
        int wavesCount = waveConfigs.Count;
        // commment for speed running
        //GameUIController.instance.ShowWaveInfoText(wavePos, wavesCount);
    }

    IEnumerator NoEnemiesOnWave()
    {
        while (totalEnemiesPerWave > 0)
        {
            yield return null;
        }
        // wait put 0 for speed running
        yield return new WaitForSeconds(2);
    }
    public void SetWavesOfLevel(List<WaveConfig> _waves)
    {
        waveConfigs = _waves;
    }


}
