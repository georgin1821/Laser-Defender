using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner instance;

    List<GameObject> squadsInScene;

    List<WaveConfig> waves;
    List<GameObject> squads;
    //public int Noe;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void SpawnTheLevel()
    {
        StartCoroutine(SpawnLevel());
    }
    public IEnumerator SpawnLevel()
    {

        for (int waveIndex = 0; waveIndex < waves.Count; waveIndex++)
        {
            squads = waves[waveIndex].GetSquads();
            // GameUIController.instance.ShowWaveInfoText(waveIndex, squads.Count);
            // yield return new WaitForSeconds(2);

            squadsInScene = new List<GameObject>();

            for (int i = 0; i < squads.Count; i++)
            {
                GameObject squad = Instantiate(squads[i]);
                squadsInScene.Add(squad);
            }

            yield return new WaitForSeconds(1);

            yield return StartCoroutine(NoEnemiesOnWave());
            DestroyWaves();
        }

        GamePlayController.instance.UpdateState(GameState.LEVELCOMPLETE);
    }
    public void DestroyWaves()
    {
        if (squadsInScene != null)
        {
            foreach (var item in squadsInScene)
            {
                Destroy(item.gameObject);
            }
        }

    }
    IEnumerator NoEnemiesOnWave()
    {

        while (EnemyCount.instance.count > 0)
        {
            yield return null;
        }
        // wait put 0 for speed running
        yield return new WaitForSeconds(1);
    }
    public void SetWaves(List<WaveConfig> _waves)
    {
        waves = _waves;
    }

}
