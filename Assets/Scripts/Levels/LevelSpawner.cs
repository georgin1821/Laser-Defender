using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner instance;

    public List<WaveScripts> waveScripts;
    public int totalEnemiesPerWave;

    public List<Transform> waypoints;
    public List<GameObject> waveEnemies;
    public WaveConfig waveConfig;
    int startingWave = 0;
    public int noe;

    public List<GameObject> waveObjects;

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
        for (int waveIndex = startingWave; waveIndex < waveScripts.Count; waveIndex++)
        {
            var currentWave = waveScripts[waveIndex];
            waveObjects = currentWave.GetWaveScripts();
            List<GameObject> waves = new List<GameObject>();
            for (int i = 0; i < waveObjects.Count; i++)
            {
               GameObject wave = Instantiate(waveObjects[i]);
                waves.Add(wave);
            }
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(NoEnemiesOnWave());
            Debug.Log("END");
            WaveControllerAbstract.count = 0;
            foreach (var item in waves)
            {
                Destroy(item.gameObject);
            }

        }

        //End of level

    }

    IEnumerator NoEnemiesOnWave()
    {
        noe = WaveControllerAbstract.count;
        while (noe > 0)
        {
            yield return null;
        }
        // wait put 0 for speed running
        yield return new WaitForSeconds(1);
    }
    public void SetWavesOfLevel(List<WaveScripts> _waves)
    {
        waveScripts = _waves;

    }


}
