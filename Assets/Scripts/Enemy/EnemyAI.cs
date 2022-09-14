using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] [Range(0, 100)] int chanceToReact;
    [SerializeField] [Range(0, .5f)] float speedRF;
    [SerializeField] float speed;

    Vector3 startingPosition;
    public float distance;
    bool hasFired;
    public bool isGoingToPlayer = true;
    public Vector3 dir;
    public bool isReacting;
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

        isReacting = true;
        distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);
        while (distance >= 2 && isGoingToPlayer)
        {
            distance = Vector2.Distance(Player.instance.gameObject.transform.position, transform.position);

            //Randomization of speed
            speed = speed * (1 + Random.Range(-speedRF / 2f, speedRF / 2f));

            transform.position = Vector3.MoveTowards(transform.position,
                Player.instance.gameObject.transform.position,
                speed * Time.deltaTime);
            // dir = (Player.instance.gameObject.transform.position.normalized - transform.position).normalized;
            //  transform.Translate(dir * speed * Time.deltaTime, Space.World);
            yield return null;

        }

        isGoingToPlayer = false;

        if (!hasFired)
        {
            GetComponent<Enemy>().Fire();
            hasFired = true;
        }

        while (Vector3.Distance(transform.position, startingPosition) > .1f)
        {
            speed = speed * (1 + Random.Range(-speedRF / 2f, speedRF / 2f));
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

