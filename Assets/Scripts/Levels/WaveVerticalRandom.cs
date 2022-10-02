using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveVerticalRandom : MonoBehaviour
{
    [SerializeField] GameObject shipPrefab;
    [SerializeField] int shipsCount;
    [SerializeField] float minSpawn, maxSpawnTime;

    private float xMin, xMax, yMax;


    private void Start()
    {
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        EnemyCount.instance.CountEnemiesAtScene(shipsCount);

        StartCoroutine(InstatiateWave());
    }

    IEnumerator InstatiateWave()
    {
        for (int index = 0; index < shipsCount; index++)
        {
            Vector3 startPos = new Vector3(Random.Range(xMin, xMax), yMax + 1f, 0);

            GameObject enemy = Instantiate(shipPrefab, startPos, shipPrefab.transform.rotation);
            enemy.GetComponent<EnemyPathfindingVertical>().StartDeployment();

            float time = CalculateSpawingIntervals(minSpawn, maxSpawnTime);
            yield return new WaitForSeconds(time);
        }
    }

    private float CalculateSpawingIntervals(float minTime, float maxTime)
    {
        if (maxTime > minTime) return Random.Range(minTime, maxTime);
        else return minTime;
    }
}
