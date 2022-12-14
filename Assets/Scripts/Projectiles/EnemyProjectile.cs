using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] bool isTargetingPlayer;

    Vector3 target, dir;
    Quaternion rot;

    private void Start()
    {
        target = Player.instance.transform.position;
        if (isTargetingPlayer)
        {
        dir = target - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, dir);
        }
    }
    void Update()
    {
        if (isTargetingPlayer)
        {
            transform.Translate(Vector3.down* speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
        }
    }

}
