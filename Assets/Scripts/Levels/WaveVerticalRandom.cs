using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveVerticalRandom : MonoBehaviour
{
    [SerializeField] DivisionConfigAndPath waveConfig;
    private float xMin, xMax, yMax;


    private void Start()
    {
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        EnemyCount.instance.CountEnemiesAtScene(waveConfig.GetNumberOfEnemies());

        StartCoroutine(InstatiateWave());
    }

    IEnumerator InstatiateWave()
    {
        GameObject[] enemyPrefabs = waveConfig.GetEnemyPrefabsList();
        for (int index = 0; index < waveConfig.GetNumberOfEnemies(); index++)
        {
            Vector3 startPos = new Vector3(Random.Range(xMin, xMax), yMax + 1.5f, 0);

            GameObject enemy = Instantiate(enemyPrefabs[0], startPos, Quaternion.identity);
            float rand = Random.Range(4, 7);
            enemy.GetComponent<EnemyPathfindingVertical>().StartDeployment();
            yield return new WaitForSeconds(rand);
        }


    }


}
