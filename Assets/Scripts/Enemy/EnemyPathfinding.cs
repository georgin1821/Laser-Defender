﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{


    List<Transform> waypoints;

    int index = 0;
    Vector3 dir;
    Quaternion rot;
    public float speed;
    float rotationSpeed;

    public bool isMovingAtFormation;

    IEnumerator myRoutine;


    public void SetWaypoints(List<Transform> waypoints, float speed, float rotSpeed)
    {
        this.waypoints = waypoints;
        this.speed = speed;
        this.rotationSpeed = rotSpeed;

    }
    IEnumerator DeploymentRoutine()
    {
        while (index < waypoints.Count - 1)
        {
            Vector3 nextPos = waypoints[this.index + 1].position;

            dir = (nextPos - transform.position).normalized;
            rot = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            transform.Translate(Vector3.up * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPos) < 01f)
            {
                index++;
            }
            yield return null;

            if(index == waypoints.Count - 1)
            {
                index = -1;
            }
        }

       // formationPosition = formationWaypoints[gameObject.GetComponent<Enemy>().placeAtWave].position;


        //    while (transform.position != formationPosition)
        //    {
        //        dir = (formationPosition - transform.position).normalized;
        //        rot = Quaternion.LookRotation(Vector3.forward, dir);
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 200 * Time.deltaTime);

        //        transform.position = Vector3.MoveTowards(transform.position,
        //            formationPosition,
        //             waveConfig.GetMoveSpeed() * Time.deltaTime);
        //        yield return null;
        //    }

        //    dir = (formationPosition - transform.position).normalized;
        //    float time = Time.time;
        //    while (Time.time < time + 1)
        //    {
        //        Quaternion rotation = Quaternion.Euler(0, 0, 180);
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 350 * Time.deltaTime);
        //        yield return null;
        //    }

        //    if (GetComponent<EnemyAI>() != null)
        //    {
        //        GetComponent<EnemyAI>().AgentReacting();
        //    }

        //    StartCoroutine(FormationMovement());
        //}

        //IEnumerator FormationMovement()
        //{
        //    yield return new WaitForSeconds(4);
        //    float startPos = transform.position.x - (distance / 2);
        //    while (isMovingAtFormation)
        //    {
        //        linearSpeed = linearSpeed * (1 + Random.Range(-randomValue / 2f, randomValue / 2f));

        //        transform.position = new Vector2((Mathf.Sin((2 * Mathf.PI * (Time.time * linearSpeed / distance)) - (Mathf.PI / 2)) * (distance / 2) + (distance / 2)) + startPos, transform.position.y);
        //        yield return null;
        //    }
        //}
    }
    public void StartDeploymentRoutine()
    {
        StopRoutine();
        myRoutine = DeploymentRoutine();
        StartCoroutine(myRoutine);
    }

    void StopRoutine()
    {
        if (myRoutine != null)
        {
            StopCoroutine("DeploymentRoutine");
        }
    }

}


