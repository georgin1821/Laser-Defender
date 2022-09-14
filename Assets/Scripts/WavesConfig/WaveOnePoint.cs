using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOnePoint : MonoBehaviour
{
    [SerializeField] WaveConfigPath waveConfig;

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

        List<GameObject> Enemies = new List<GameObject>();
        StartCoroutine(InstatiateWave());

    }
    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        return waveWaypoints; ;
    }

    IEnumerator InstatiateWave()
    {
        GameObject obj = new GameObject("Wave");
        Vector3 startPos = obj.transform.position;

        for (int index = 0; index < waveConfig.GetNumberOfEnemies(); index++)
        {
            int i;
            if (index % 2 == 0)
            {
                i = 0;
            }
            else
            {
                i = 1;
            }

            GameObject newEnemy = Instantiate(waveConfig.enemyPrefabs[i],
            waypointStart[index].position,
            Quaternion.identity) as GameObject;
            newEnemy.transform.SetParent(obj.transform);

            speed = waveConfig.GetMoveSpeed();
            newEnemy.GetComponent<EnemyPathfinding>().SetWaypoints(waypointStart, speed, 0);

            StartCoroutine(newEnemy.GetComponent<EnemyPathfinding>().DeploymentRoutineSinglePoint(waypointEnd[index]));
            yield return new WaitForSeconds(.1f);
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

