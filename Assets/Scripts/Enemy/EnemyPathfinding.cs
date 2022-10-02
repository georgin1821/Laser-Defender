using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    int index = 0;
    Vector3 dir;
    Quaternion rot;
    bool isMovingHorizontal;

    public bool isMovingAtFormation = true;
    public Vector3 FormationPosition { get; set; }
    public float Speed { get; set; }
    public float RotationSpeed { get; set; }
    public List<Transform> Waypoints { get; set; }

    public void SetWaypoints(List<Transform> waypoints, float speed, float rotSpeed, bool isMovingHorizontal)
    {
        this.Waypoints = waypoints;
        this.Speed = speed;
        this.RotationSpeed = rotSpeed;
        this.isMovingHorizontal = isMovingHorizontal;
    }

    public void StartDeploymentRoutine(bool isRotating)
    {
        StartCoroutine(DeploymentRoutine(isRotating));
    }
    IEnumerator DeploymentRoutine(bool isRotating)
    {
                Vector3 velocity = Vector3.zero;
        while (index < Waypoints.Count - 1)
        {
            Vector3 nextPos = Waypoints[this.index + 1].position;
            if (isRotating)
            {
                dir = (nextPos - transform.position).normalized;
                rot = Quaternion.LookRotation(Vector3.forward, dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, RotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
            }
            else
            {
                //transform.position = Vector3.SmoothDamp(transform.position,
                //                             nextPos, ref velocity, 04f,
                //                             Speed * Time.deltaTime);
                transform.position = Vector3.SmoothDamp(transform.position,
                             nextPos, ref velocity, 0.8f );

            }

            if (Vector3.Distance(transform.position, nextPos) < 0.2f)
            {
                index++;
            }
            yield return null;

            if (index == Waypoints.Count - 1)
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

        while (Vector3.Distance(transform.position, end.position) > .01f)
        {
            Speed -= Time.deltaTime * (Speed + 2);
            if (Speed < 2) Speed = 2;
            transform.position = Vector3.MoveTowards(transform.position,
               end.position,
               Speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(FormationMove());
    }

    public void DeploymentFormNoRotation()
    {
        StartCoroutine(DeploymentFormationNoRotationRoutine());
    }
    IEnumerator DeploymentFormationNoRotationRoutine()
    {
        while (index < Waypoints.Count - 1)
        {
            Vector3 nextPos = Waypoints[this.index + 1].position;

            transform.position = Vector3.MoveTowards(transform.position,
                                                     nextPos,
                                                     Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPos) < 0.3f)
            {
                index++;
            }
            yield return null;
        }

        while (Vector3.Distance(transform.position, FormationPosition) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                     FormationPosition,
                                                     Speed * Time.deltaTime);
            yield return null;

        }

        yield return new WaitForSeconds(1);

        if (GetComponent<EnemyAI>() != null)
        {
            GetComponent<EnemyAI>().AgentReacting(FormationPosition);
        }

        StartCoroutine(FormationMove());
    }

    public void DeploymentWithFormation()
    {
        StartCoroutine(DeploymentFormationRoutine());
    }
    public IEnumerator DeploymentFormationRoutine()
    {
        while (index < Waypoints.Count - 1)
        {
            Vector3 nextPos = Waypoints[this.index + 1].position;

            dir = (nextPos - transform.position).normalized;
            rot = Quaternion.LookRotation(Vector3.forward, dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, RotationSpeed * Time.deltaTime);

            transform.Translate(Vector3.up * Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPos) < 0.3f)
            {
                index++;
            }
            yield return null;
        }

        dir = (FormationPosition - transform.position).normalized;
        rot = Quaternion.LookRotation(Vector3.forward, dir);
        while (Vector3.Distance(transform.position, FormationPosition) > 0.01)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 700 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position,
              FormationPosition,
               Speed * Time.deltaTime);
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
            GetComponent<EnemyAI>().AgentReacting(FormationPosition);
        }

        StartCoroutine(FormationMove());
    }

    public IEnumerator FormationMove()
    {
        Vector3 startPos = gameObject.transform.position;
        float frequency = Random.Range(3.5f, 3.7f);
        float magnitude = Random.Range(0.05f, 0.07f);

        while (isMovingAtFormation)
        {
            // transform.position = startPos + transform.up * Mathf.Sin(Time.time * frequency- 0.5f) * magnitude;

            Vector3 dir = Vector3.zero;
            if (isMovingHorizontal)
            {
            dir.x = Mathf.Sin(Time.time * 2f - 0.5f) * .2f;
            }
            else
            {
                dir.x = 0;
            }

            dir.y = Mathf.Sin(Time.time * frequency - 0.5f) * magnitude;
           // Vector3 pos = new Vector3(Mathf.Sin(Time.time * 2f - 0.5f) * .2f, Mathf.Sin(Time.time * frequency - 0.5f) * magnitude);

            transform.position = startPos + dir;
            yield return null;
        }
    }

}


