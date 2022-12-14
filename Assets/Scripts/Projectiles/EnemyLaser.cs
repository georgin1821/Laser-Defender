using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : EnemyWeaponAbstract
{

    private void Start()
    {
        InvokeRepeating("FireChance", minTimeToFire, maxTimeToFire);

    }
    protected override void Fire()
    {
        Vector3 firePos = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        Instantiate(prjectilePrefab, firePos, Quaternion.identity);
    }
   protected override void FireChance()
    {
        if (UnityEngine.Random.Range(1, 100) <= chanceToFire)
        {
            Fire();
        }
    }

    public override void Firing()
    {
        Fire();
    }
}
