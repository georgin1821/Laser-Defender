using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    public float speed;
    public GameObject path;

    public Transform[] waypoints;

    void Start()
    {
        //waypoints = waveConfig.GetWaypoints();
        waypoints = path.GetComponentsInChildren<Transform>();
        StartCoroutine(FoolwPath());

    }

    // Update is called once per frame
    IEnumerator FoolwPath()
    {
        int index = 0;
        while (index < waypoints.Length - 1)
        {
            Vector3 nextPos = waypoints[index + 1].position;
           Vector3 dir = (nextPos - transform.position).normalized;


            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, nextPos) < 0.3f)
            {
                index++;
            }
            yield return null;
        }

    }
}
