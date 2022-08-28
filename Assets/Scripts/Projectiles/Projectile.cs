using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] public int projectilesCount;

    private float speed;

    void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
       transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }



}
