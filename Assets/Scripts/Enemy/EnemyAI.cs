using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    GameObject player;

    [SerializeField] [Range(0, 100)] int chanceToReact;
    [SerializeField] bool isChasing;
    [SerializeField] [Range(0, .5f)] float randomFactor;
    [SerializeField] float speed;
    [SerializeField] bool isGoingToPlayer = true;

    public Vector3 startingPosition;
    public Vector3 pos, dest;


    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    private void Start()
    {
        startingPosition = this.transform.position;
    }

    public void AgentReacting()
    {
        if (Random.Range(1, 100) <= chanceToReact)
        {
            StartCoroutine(AgentCycle());
        }
    }

    IEnumerator AgentCycle()
    {
        //Chasing Attackin donothing
            GetComponent<EnemyPathfinding>().isMovingAtFormation = false;

        while (isChasing)
        {


            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance > 3 && isGoingToPlayer)
            {
                //Randomization of speed
                speed = speed * (1 + Random.Range(-randomFactor / 2f, randomFactor / 2f));
                pos = this.transform.position;
                dest = player.transform.position;
            }
            if (distance < 3 && isGoingToPlayer)
            {
                isGoingToPlayer = false;

                speed = speed * (1 + Random.Range(-randomFactor / 2f, randomFactor / 2f));
                pos = this.transform.position;
                dest = startingPosition;
                GetComponent<Enemy>().FireFromChasing();
            }

            transform.position = Vector3.MoveTowards(pos, dest, speed * Time.deltaTime);
            Debug.Log("trnsform");
            yield return null;
        }
    }
}

