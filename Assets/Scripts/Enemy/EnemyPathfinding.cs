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
    float speedRF;
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
    bool smoothMovement;

    List<Transform> waypoints;
    int id;
    float yMax;

    Transform endTrans;
    DivisionAbstract divisionAbstract;

    private void Start()
    {
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + 2;
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.PointToPoint:
                break;
            case EnemyState.FormationMove:
                break;
            case EnemyState.MovingToPlayer:
                break;
            case EnemyState.SinglePath:
                break;
            case EnemyState.FormationDeployment:
                break;
            case EnemyState.VerticalMovement:
                if (transform.position.y < -yMax)
                {
                    StopAllCoroutines();
                    Destroy(this.gameObject);
                    EnemyCount.instance.Count--;
                }
                break;
        }
    }

    public void UpdateState(EnemyState newState)
    {
        EndState();
        state = newState;
        switch (newState)
        {
            case EnemyState.PointToPoint:
                this.endTrans = divisionAbstract.formationWaypoints[id];
                StartCoroutine(DeploymentRoutineSinglePoint());
                break;

            case EnemyState.FormationMove:
                isReacting = false;
                StartCoroutine(FormationMove());
                StartCoroutine(AgentChanceToReact());
                break;

            case EnemyState.MovingToPlayer:
                StartCoroutine(AgentCycle());
                break;

            case EnemyState.SinglePath:
                waypoints = divisionAbstract.waypoints;
                StartCoroutine(DeploymentRoutine(isRotating));
                break;

            case EnemyState.FormationDeployment:
                formPosition = divisionAbstract.formationWaypoints[id].position;
                StartCoroutine(DeploymentFormationRoutine());
                break;

            case EnemyState.VerticalMovement:
                StartCoroutine(VerticalMovement());
                break;

        }
    }
    void EndState()
    {
        switch (state)
        {
            case EnemyState.PointToPoint:
                break;
            case EnemyState.FormationMove:
                StopAllCoroutines();
                break;
            case EnemyState.MovingToPlayer:
                break;
            case EnemyState.SinglePath:
                StopAllCoroutines();
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

            if (Vector3.Distance(transform.position, nextPos) < 0.2f)
            {
                index++;
            }
            yield return null;
        }
        if (endlessMove)
        {
            if (index == waypoints.Count - 1)
            {
                index = -1;
            }
        }
        else
        {
            UpdateState(EnemyState.FormationDeployment);
        }
    }
    IEnumerator DeploymentRoutineSinglePoint()
    {

        while (Vector3.Distance(transform.position, endTrans.position) > .01f)
        {
            speed -= Time.deltaTime * (speed + 2);
            if (speed < 2) speed = 2;
            transform.position = Vector3.MoveTowards(transform.position,
            endTrans.position,
            speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1);

        UpdateState(EnemyState.FormationMove);
    }
    IEnumerator DeploymentFormationRoutine()
    {
        dir = (formPosition - transform.position).normalized;
        rot = Quaternion.LookRotation(Vector3.forward, dir);
        while (Vector3.Distance(transform.position, formPosition) > 0.02)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position,
            formPosition,
            speed * Time.deltaTime);
            yield return null;

        }
        float time = Time.time;
        Quaternion rotation = Quaternion.Euler(0, 0, 180);
        while (rotation != transform.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        UpdateState(EnemyState.FormationMove);


    }
    IEnumerator FormationMove()
    {
        startingPosition = gameObject.transform.position;
        float frequency = Random.Range(3.5f, 3.7f);
        float magnitude = Random.Range(0.05f, 0.07f);
        Vector3 dir = Vector3.zero;
        while (true)
        {
            if (isMovingHorizontal)
            {
                dir.x = Mathf.Sin(Time.time * 2f - 0.5f) * .2f;
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
        while (distance >= disToPlayer)
        {
            distance = Vector2.Distance(Player.instance.gameObject.transform.position, transform.position);

            transform.position = Vector3.MoveTowards(transform.position,
                Player.instance.gameObject.transform.position,
                aiSpeed * Time.deltaTime);
            yield return null;

        }

        //isGoingToPlayer = false;

        ////if (!hasFired)
        ////{
        // GetComponent<Enemy>().Fire();
        ////    hasFired = true;
        ////}

        while (Vector3.Distance(transform.position, startingPosition) > .1f)
        {
            //speed = speed * (1 + Random.Range(-speedRandFactor / 2f, speedRandFactor / 2f));
            transform.position = Vector3.MoveTowards(transform.position,
                startingPosition,
                speed * Time.deltaTime);
            yield return null;
        }
        UpdateState(EnemyState.FormationMove);

        //isGoingToPlayer = true;

    }
    IEnumerator AgentChanceToReact()
    {
        while (!isReacting)
        {
            yield return new WaitForSeconds(2);

            if (Random.Range(1, 100) <= AIchanceToReact)
            {
                UpdateState(EnemyState.MovingToPlayer);
                isReacting = true;
            }
            yield return new WaitForSeconds(2);
        }
    }
    IEnumerator VerticalMovement()
    {
        speed = Random.Range(speed - speedRF, speed + speedRF);

        while (true)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }
    }




    public void SetDivisionConfiguration(DivisionConfiguration divConfig, DivisionAbstract divisionAbstract, int id = 0)
    {
        this.divisionAbstract = divisionAbstract;
        speed = divConfig.moveSpeed;
        isRotating = divConfig.isRotating;
        rotationSpeed = divConfig.rotationSpeed;
        this.endlessMove = divConfig.endlessMove;
        this.id = id;
        smoothMovement = divConfig.smoothMovement;
    }
    public void SetFormMoveConfg(FormMoveSettings settings)
    {
        isMovingHorizontal = settings.isMovingHorizontal;
        isMovingVertical = settings.isMovingVertical;
    }
    public void SetAIConfg(EnemyAISettings settings)
    {
        AIchanceToReact = settings.AiChanceToReact;
        aiSpeed = settings.aiSpeed;
    }
    public void SetVerticalMoveConfg(VerticalMoveSettings settings)
    {
        speed = settings.speed;
        speedRF = settings.speedRF;
    }


}

public enum EnemyState
{
    PointToPoint,
    FormationMove,
    MovingToPlayer,
    SinglePath,
    FormationDeployment,
    VerticalMovement
}

