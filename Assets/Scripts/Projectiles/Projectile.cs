using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] public int projectilesCount;

    public float speed;

    private void Start()
    {
        speed = Gun.instance.GetProjectileSpeed();
    }
    void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }


}
