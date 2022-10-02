using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] [Range(0, 100)] int chanceToReact;
    [SerializeField] [Range(0, 1f)] float speedRandFactor;
    [SerializeField] float speed;
    [SerializeField] float disToPlayer = 2f;
    [SerializeField] bool freeToReact = false;

    bool hasFired;
    bool isGoingToPlayer = true;
    bool isReacting;
    Vector3 startingPosition;
    private void Start()
    {
        if (freeToReact)
        {
            AgentReacting(transform.position);
        }
    }
    public void AgentReacting(Vector3 startingPosition)
    {
        this.startingPosition = startingPosition;
        InvokeRepeating("AgentChanceToReact", Random.Range(2, 8), Random.Range(4, 8));
    }

    void AgentChanceToReact()
    {
        if (Random.Range(1, 100) <= chanceToReact && !isReacting)
        {
            StartCoroutine(AgentCycle());
            gameObject.GetComponent<EnemyPathfinding>().isMovingAtFormation = false;
        }

    }
    IEnumerator AgentCycle()
    {
        float distance;
        hasFired = false;
        isReacting = true;
        speed = Random.Range(speed - speedRandFactor, speed + speedRandFactor);

        distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);
        while (distance >= disToPlayer && isGoingToPlayer)
        {
            distance = Vector2.Distance(Player.instance.gameObject.transform.position, transform.position);

            transform.position = Vector3.MoveTowards(transform.position,
                Player.instance.gameObject.transform.position,
                speed * Time.deltaTime);
            yield return null;

        }

        isGoingToPlayer = false;

        if (!hasFired)
        {
           // GetComponent<Enemy>().Fire();
            hasFired = true;
        }

        while (Vector3.Distance(transform.position, startingPosition) > .1f)
        {
            //speed = speed * (1 + Random.Range(-speedRandFactor / 2f, speedRandFactor / 2f));
            transform.position = Vector3.MoveTowards(transform.position,
                startingPosition,
                speed * Time.deltaTime);
            yield return null;
        }


        isGoingToPlayer = true;
        isReacting = false;
        gameObject.GetComponent<EnemyPathfinding>().isMovingAtFormation = true;
        StartCoroutine(GetComponent<EnemyPathfinding>().FormationMove());

    }
}

