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
    float distance;
    bool hasFired;
    bool isGoingToPlayer = true;
    bool isChasing = true;
    public void AgentReacting(Transform trans)
    {

        if (Random.Range(1, 100) <= chanceToReact)
        {
            EnemyPathfinding.AiReacted = true;
            startingPosition = trans.position;
            StartCoroutine(AgentCycle());
        }
    }


    IEnumerator AgentCycle()
    {
        Quaternion rot;
        Vector3 dir;

        while (isChasing)
        {
            distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);

            while (distance >= 2 && isGoingToPlayer)
            {
                distance = Vector3.Distance(Player.instance.gameObject.transform.position, transform.position);

                //Randomization of speed
                speed = speed * (1 + Random.Range(-speedRF / 2f, speedRF / 2f));
                dir = (Player.instance.gameObject.transform.position - transform.position).normalized;
                rot = Quaternion.LookRotation(Vector3.forward, dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 600 * Time.deltaTime);

                // transform.Translate(Vector3.up * speed * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position,
                    Player.instance.gameObject.transform.position,
                    speed * Time.deltaTime);
                yield return null;

            }
            isGoingToPlayer = false;
            if (!hasFired)
            {
                GetComponent<Enemy>().Fire();
                hasFired = true;
            }


            speed = speed * (1 + Random.Range(-speedRF / 2f, speedRF / 2f));
            while (Vector3.Distance(transform.position, startingPosition) > .5f)
            {
                dir = (startingPosition - transform.position).normalized;
                rot = Quaternion.LookRotation(Vector3.forward, dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 300 * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position,
                    startingPosition,
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

            yield return null;

        }
    }
}

