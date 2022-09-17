using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{

    List<Transform> waypoints;
    int index = 0;
    Vector3 dir;
    [HideInInspector]
    public Vector3 formationPosition;
    Quaternion rot;
    public float speed;
    float rotationSpeed;

    public bool isMovingAtFormation = true;
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

            if (index == waypoints.Count - 1)
            {
                index = -1;
            }
        }
    }

    public void DeploymentSinglePoint(Transform end)
    {
        StartCoroutine(DeploymentRoutineSinglePoint(end));
    }

    IEnumerator DeploymentRoutineSinglePoint(Transform end)
    {

        while (Vector3.Distance(transform.position, end.position) > .2f)
        {
            speed -= Time.deltaTime * (speed + 2);
            if (speed < 2) speed = 2;
            transform.position = Vector3.MoveTowards(transform.position,
               end.position,
               speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(FormationMove());
    }
    public void DeplymentFormNoRotation()
    {
        StartCoroutine(DeploymentRoutineFormnNoRotation());
    }
    IEnumerator DeploymentRoutineFormnNoRotation()
    {
        while (index < waypoints.Count - 1)
        {
            Vector3 nextPos = waypoints[this.index + 1].position;

            transform.position = Vector3.MoveTowards(transform.position,
    nextPos,
    speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPos) < 0.3f)
            {
                index++;
            }
            yield return null;
        }

        dir = (formationPosition - transform.position).normalized;
        rot = Quaternion.LookRotation(Vector3.forward, dir);
        while (Vector3.Distance(transform.position, formationPosition) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,
              formationPosition,
               speed * Time.deltaTime);
            yield return null;

        }
        float time = Time.time;
        while (Time.time < time + 1)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 180);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        if (GetComponent<EnemyAI>() != null)
        {
            GetComponent<EnemyAI>().AgentReacting(formationPosition);
        }

        gameObject.GetComponent<Enemy>().InvokeRepeating("FireChance", 1, 2);
        StartCoroutine(FormationMove());
    }

    public IEnumerator DeploymentRoutineForm()
    {
        while (index < waypoints.Count - 1)
        {
            Vector3 nextPos = waypoints[this.index + 1].position;

            dir = (nextPos - transform.position).normalized;
            rot = Quaternion.LookRotation(Vector3.forward, dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            transform.Translate(Vector3.up * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPos) < 0.3f)
            {
                index++;
            }
            yield return null;
        }

        dir = (formationPosition - transform.position).normalized;
        rot = Quaternion.LookRotation(Vector3.forward, dir);
        while (Vector3.Distance(transform.position, formationPosition) > 0.01)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 700 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position,
              formationPosition,
               speed * Time.deltaTime);
            yield return null;

        }
        float time = Time.time;
        while (Time.time < time + 1)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 180);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 450 * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        if (GetComponent<EnemyAI>() != null)
        {
            GetComponent<EnemyAI>().AgentReacting(formationPosition);
        }

        gameObject.GetComponent<Enemy>().InvokeRepeating("FireChance", 1, 2);
        StartCoroutine(FormationMove());
    }

    public IEnumerator FormationMove()
    {
        // Vector3 startPos = gameObject.transform.position;

        while (isMovingAtFormation)
        {
            // transform.position = startPos + transform.right * Mathf.Sin(Time.time * frequency + offset) * magnitude;

            yield return null;
        }
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


