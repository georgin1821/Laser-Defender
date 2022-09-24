using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Division2WaysPath : MonoBehaviour
{
    [SerializeField] DivisionConfigAndPath waveConfig;

    [SerializeField] GameObject path1;
    [SerializeField] GameObject path2;

    List<Transform> waypointStart;
    List<Transform> waypointEnd;



    float speed;

    private void Start()
    {
        EnemyCount.instance.CountEnemiesAtScene(waveConfig.GetNumberOfEnemies());
        waypointStart = new List<Transform>();
        waypointEnd = new List<Transform>();

        foreach (Transform child in path1.transform)
        {
            waypointStart.Add(child);
        }
        foreach (Transform child in path2.transform)
        {
            waypointEnd.Add(child);
        }

        StartCoroutine(InstatiateWave());

    }
    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        return waveWaypoints; ;
    }

    IEnumerator InstatiateWave()
    {
        GameObject[] enemyPrefabs = waveConfig.GetEnemyPrefabsList();
        GameObject obj = new GameObject("Wave");
        Vector3 startPos = obj.transform.position;

        // information about enemies prefab List
        int prefabEnemiesCount = enemyPrefabs.Length;
        for (int index = 0; index < waveConfig.GetNumberOfEnemies(); index++)
        {
            // select one of two enemies prefab to spawn, works only for two ep list
            int i = 0;
            if (prefabEnemiesCount > 1)
            {
                if (index % 2 == 0) i = 0;
                else i = 1;
            }

            GameObject newEnemy = Instantiate(enemyPrefabs[i],
            waypointStart[index].position,
            enemyPrefabs[i].transform.rotation) as GameObject;
            newEnemy.transform.SetParent(obj.transform);

            speed = waveConfig.GetMoveSpeed();
            newEnemy.GetComponent<EnemyPathfinding>().SetWaypoints(waypointStart, speed, 0);

            newEnemy.GetComponent<EnemyPathfinding>().DeploymentSinglePoint(waypointEnd[index]);
           yield return new WaitForSeconds(.1f);
           // yield return null;
        }

        yield return new WaitForSeconds(3f);

        while (true)
        {
            // transform.position = startPos + transform.right * Mathf.Sin(Time.time * frequency + offset) * magnitude;

            obj.transform.position = startPos + transform.up * Mathf.Sin(Time.time * 3f - 0.65f) * .2f;
            yield return null;

        }

    }

}

