using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPathfinding : MonoBehaviour
{
    EnemyState state;
    int index = 0;
    Vector3 dir;
    Quaternion rot;
    Vector3 formPosition;
    float speed;
    float speedRF = 0.5f;
    bool isMovingHorizontal;
    bool isMovingVertical;
    int AIchanceToReact;
    float aiSpeed;
    float disToPlayer = 2f;
    float rotationSpeed;
    Vector3 startingPosition;
    bool isReacting;
    bool isRotating;
    bool endlessMove;
    bool isFormMoving;
    bool smoothMovement;
    float smoothDelta;
    float frequency;
    float magnitude;
    bool isChasingPlayer;
    List<Transform> waypoints;
    int id;

    Transform endTrans;
    DivisionSpawn divSpawn;

    private void Update()
    {
        switch (state)
        {
            case EnemyState.PointToPoint:
                break;
            case EnemyState.FormationMove:
                break;
            case EnemyState.AgentCycle:
                break;
            case EnemyState.DivisionToPath:
                break;
            case EnemyState.FormationDeployment:
                break;
        }
    }

    public void UpdateState(EnemyState newState)
    {
        state = newState;
        switch (newState)
        {
            case EnemyState.PointToPoint:
                this.endTrans = divSpawn.formationWaypoints[id];
                StartCoroutine(DeploymentRoutineSinglePoint());
                break;

            case EnemyState.FormationMove:
                StartCoroutine(FormationMove());
                if (AIchanceToReact > 0) StartCoroutine(AgentChanceToReact());
                break;

            case EnemyState.AgentCycle:
                StopAllCoroutines();
                StartCoroutine(AgentCycle());
                break;

            case EnemyState.DivisionToPath:
                waypoints = divSpawn.waypoints;
                StartCoroutine(DeploymentRoutine(isRotating));
                break;

            case EnemyState.FormationDeployment:
                formPosition = divSpawn.formationWaypoints[id].position;
                StartCoroutine(DeploymentFormationRoutine());
                break;

            case EnemyState.ChasingPlayer:
                this.endTrans = divSpawn.formationWaypoints[id];
                StartCoroutine(DeploymentRoutineSinglePoint());
                break;
        }
    }
    void EndState()
    {
        switch (state)
        {
            case EnemyState.PointToPoint:
                if (isFormMoving)
                {
                    UpdateState(EnemyState.FormationMove);
                }
                break;
            case EnemyState.FormationMove:
                StopAllCoroutines();
                break;
            case EnemyState.AgentCycle:
                break;
            case EnemyState.DivisionToPath:
                break;
            case EnemyState.ChasingPlayer:
                if (isChasingPlayer)
                {
                    StartCoroutine(ChasingPlayer());
                }
                break;
        }
    }

    IEnumerator DeploymentRoutine(bool isRotating)
    {
        Vector3 velocity = Vector3.zero;
        while (index < waypoints.Count - 1)
        {
            Vector3 nextPos = waypoints[this.index + 1].position;

            if (isRotating)
            {
                dir = (nextPos - transform.position).normalized;
                rot = Quaternion.LookRotation(Vector3.forward, dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            else
            {
                if (!smoothMovement)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                    nextPos,
                    speed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position,
                    nextPos, ref velocity, 0.8f);
                }
            }

            if (Vector3.Distance(transform.position, nextPos) < 0.1f)
            {
                index++;
            }
            if (index == waypoints.Count - 1 && endlessMove)
            {
                index = -1;
            }
            yield return null;
        }

        UpdateState(EnemyState.FormationDeployment);

    }
    IEnumerator DeploymentRoutineSinglePoint()
    {
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, endTrans.position) > .01f)
        {
            //speed -= Time.deltaTime * (speed + 2);
            //if (speed < 2) speed = 2;
            //transform.position = Vector3.MoveTowards(transform.position,
            //endTrans.position,
            //speed * Time.deltaTime);

            transform.position = Vector3.SmoothDamp(transform.position,
              endTrans.position, ref velocity, smoothDelta);

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        EndState();
    }
    IEnumerator DeploymentFormationRoutine()
    {
        dir = (formPosition - transform.position).normalized;
        rot = Quaternion.LookRotation(Vector3.forward, dir);

        while (Vector3.Distance(transform.position, formPosition) > 0.02)
        {
            if (isRotating)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            }
            transform.position = Vector3.MoveTowards(transform.position,
            formPosition,
            speed * Time.deltaTime);
            yield return null;
        }

        Quaternion rotation = Quaternion.Euler(0, 0, 180);

        while (rotation != transform.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        if (isFormMoving)
        {
            UpdateState(EnemyState.FormationMove);
        }
    }
    IEnumerator FormationMove()
    {
        startingPosition = gameObject.transform.position;
        Vector3 dir = Vector3.zero;

        while (true)
        {
            if (isMovingHorizontal)
            {
                dir.x = Mathf.Sin(Time.time * frequency - 1f) * magnitude;
            }
            if (isMovingVertical)
            {
                dir.y = Mathf.Sin(Time.time * frequency - 0.5f) * magnitude;
            }

            transform.position = startingPosition + dir;
            yield return null;
        }
    }
    IEnumerator AgentCycle()
    {
        float distance;

        distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);
        speedRF = aiSpeed * speedRF;
        aiSpeed = aiSpeed * (1 + Random.Range(-speedRF / 2f, speedRF / 2f));

        while (distance >= disToPlayer)
        {
            distance = Vector2.Distance(Player.instance.gameObject.transform.position, transform.position);

            transform.position = Vector3.MoveTowards(transform.position,
                Player.instance.gameObject.transform.position,
                aiSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<EnemyWeaponAbstract>().Firing();
        yield return new WaitForSeconds(1f);


        while (Vector3.Distance(transform.position, startingPosition) > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                 startingPosition,
                                aiSpeed * Time.deltaTime);
            yield return null;
        }
        isReacting = false;
        UpdateState(EnemyState.FormationMove);
    }
    IEnumerator AgentChanceToReact()
    {
        while (!isReacting)
        {
            yield return new WaitForSeconds(Random.Range(1, 7));

            if (Random.Range(1, 100) <= AIchanceToReact)
            {
                UpdateState(EnemyState.AgentCycle);
                isReacting = true;
            }
        }
    }
    IEnumerator ChasingPlayer()
    {
        yield return new WaitForSeconds(2f);
        Vector3 velicity = Vector3.zero;
        float distance;
        distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);
        GameObject player = Player.instance.gameObject;
        while (distance > .2f)
        {
            dir = (player.transform.position - transform.position).normalized;
            rot = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 250 * Time.deltaTime);

            distance = Vector2.Distance(player.transform.position, transform.position);

            transform.position = Vector3.SmoothDamp(transform.position,
                player.transform.position, ref velicity,
                0.9f, speed);
            yield return null;
        }
    }

    public void SetDivisionConfiguration(DivisionConfiguration divConfig, DivisionSpawn spawn, int id = 0)
    {
        divSpawn = spawn;
        speed = divConfig.spawns.moveSpeed;

        this.id = id;
        isFormMoving = divConfig.general.isFormMoving;
        isMovingHorizontal = divConfig.formMove.isMovingHorizontal;
        isMovingVertical = divConfig.formMove.isMovingVertical;
        isChasingPlayer = divConfig.general.isChasingPlayer;
        frequency = divConfig.formMove.frequency;
        magnitude = divConfig.formMove.magitude;
        if (divConfig.aISettings != null)
        {
            AIchanceToReact = divConfig.aISettings.AiChanceToReact;
            aiSpeed = divConfig.aISettings.aiSpeed;
        }
        endlessMove = divConfig.general.endlessMove;
        smoothDelta = divConfig.smooth.smoothDelta;
        smoothMovement = divConfig.smooth.smoothMovement;
        rotationSpeed = divConfig.rotation.rotationSpeed;
        isRotating = divConfig.general.isRotating;
    }
}

public enum EnemyState
{
    PointToPoint,
    FormationMove,
    AgentCycle,
    DivisionToPath,
    FormationDeployment,
    ChasingPlayer
}

