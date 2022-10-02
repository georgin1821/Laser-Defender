using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;

    public static event System.Action<int> OnEnemiesDieCount;

    List<WaveConfig> waves;
    List<GameObject> divisionInScene;
    List<GameObject> divisions;

    int enemiesCount;
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
    IEnumerator SpawnLevel()
    {
        for (int waveIndex = 0; waveIndex < waves.Count; waveIndex++)
        {
            //instantiate all the waves in the current level (waves)
            divisions = waves[waveIndex].GetDivisions();
            float delay = waves[waveIndex].GetDealy();
            GameUIController.instance.ShowWaveInfoText(waveIndex, waves.Count);

            if (GameManager.Instance.isSpeedLevel)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(2);
            }

            divisionInScene = new List<GameObject>();
            // spawn squad by squad
            for (int i = 0; i < divisions.Count; i++)
            {
                GameObject division = Instantiate(divisions[i]);
                divisionInScene.Add(division);
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(1);

            yield return StartCoroutine(NoEnemiesOnWave());
            DestroyWaves();
        }

        GamePlayController.instance.UpdateState(GameState.LEVELCOMPLETE);
    }
    public void DestroyWaves()
    {
        if (divisionInScene != null)
        {
            foreach (var item in divisionInScene)
            {
                Destroy(item.gameObject);
            }
        }

    }
    IEnumerator NoEnemiesOnWave()
    {
        enemiesCount = EnemyCount.instance.Count;
        while (EnemyCount.instance.Count > 0)
        {
            yield return null;
        }
      //  OnEnemiesDieCount(enemiesCount);
        yield return new WaitForSeconds(1);
    }
    public void SetWaves(List<WaveConfig> _waves)
    {
        waves = _waves;
    }

}
