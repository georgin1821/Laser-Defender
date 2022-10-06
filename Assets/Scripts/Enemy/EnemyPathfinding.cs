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
    float chasingSpeed;
    bool isFormMoving;
    bool isChasingPlayer;
    bool smoothMovement;
    float smoothDelta;
    float frequency;
    float magnitude;
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
            case EnemyState.DivisionToPath:
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

            case EnemyState.DivisionToPath:
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

            case EnemyState.ChasingPlayer:
                StartCoroutine(ChasingPlayer());
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
                else if (isChasingPlayer)
                {
                    UpdateState(EnemyState.ChasingPlayer);
                }

                break;
            case EnemyState.FormationMove:
                StopAllCoroutines();
                break;
            case EnemyState.MovingToPlayer:
                break;
            case EnemyState.DivisionToPath:
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
        UpdateState(EnemyState.FormationMove);


    }
    IEnumerator FormationMove()
    {
        startingPosition = gameObject.transform.position;
        //  frequency = Random.Range(frequency - 0.1f, frequency + 0.1f);
        //  magnitude = Random.Range(magnitude - 0.1f, magnitude + .1f);
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
    IEnumerator VerticalMovement()
    {
        speed = Random.Range(speed - speedRF, speed + speedRF);

        while (true)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
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

        UpdateState(EnemyState.FormationMove);
    }
    IEnumerator AgentChanceToReact()
    {
        while (!isReacting)
        {
            yield return new WaitForSeconds(Random.Range(1, 7));

            if (Random.Range(1, 100) <= AIchanceToReact)
            {
                UpdateState(EnemyState.MovingToPlayer);
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
        while ( distance >  .2f)
        {
            dir = (player.transform.position - transform.position).normalized;
            rot = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 250 * Time.deltaTime);

            distance = Vector2.Distance(player.transform.position, transform.position);

            transform.position = Vector3.SmoothDamp(transform.position,
                player.transform.position, ref velicity,
                0.9f ,chasingSpeed);
            yield return null;
        }
        

    }

    public void SetDivisionConfiguration(DivisionConfiguration divConfig, DivisionAbstract divisionAbstract, int id = 0)
    {
        this.divisionAbstract = divisionAbstract;
        speed = divConfig.moveSpeed;
        this.id = id;
        isChasingPlayer = divConfig.isChasingPlayer;
        isFormMoving = divConfig.isFormMoving;
    }
    public void SetFormMoveConfg(FormMoveSettings settings)
    {
        isMovingHorizontal = settings.isMovingHorizontal;
        isMovingVertical = settings.isMovingVertical;
        frequency = settings.frequency;
        magnitude = settings.magitude;
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
    public void SetSpawnConfig(SpawnsSettings settings)
    {
        endlessMove = settings.endlessMove;
    }
    public void SetSmoothDeltaConfig(SmoothDeltaSettings settings)
    {
        smoothDelta = settings.smoothDelta;
        smoothMovement = settings.smoothMovement;
    }
    public void SetRotationConfig(RotationSettings settings)
    {
        rotationSpeed = settings.rotationSpeed;
        isRotating = settings.isRotating;
    }
    public void SetAIChasingPlayerConfig(AIChasingPlayerSettings settings)
    {
        chasingSpeed = settings.chasingSpeed;
    }

}

public enum EnemyState
{
    PointToPoint,
    FormationMove,
    MovingToPlayer,
    DivisionToPath,
    FormationDeployment,
    VerticalMovement,
    ChasingPlayer
}

