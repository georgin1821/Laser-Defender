using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] protected float chanceToFire;
    [SerializeField] protected GameObject prjectilePrefab;
    [SerializeField] protected int minTimeToFire, maxTimeToFire;

    abstract protected void FireChance();

}
