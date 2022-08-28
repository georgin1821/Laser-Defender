using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject chasingProjectilePrefab;

    [Header("VFX")]
    [SerializeField] GameObject deathVFX;

    [Header("PowerUps")]
    [SerializeField] int chanchToDropPower;
    [SerializeField] int chanceOfDropingGold;


    [HideInInspector] public int placeAtWave;
    private void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Update()
    {
        CountDownShoot();
    }

    void CountDownShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        float firePadding = GetComponent<Renderer>().bounds.size.y / 2;
        Vector3 firePos = new Vector3(transform.position.x, transform.position.y - firePadding, transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab,
            firePos,
            projectilePrefab.transform.rotation) as GameObject;
    }

    public void FireFromChasing()
    {
        float firePadding = GetComponent<Renderer>().bounds.size.y / 2;
        Vector3 firePos = new Vector3(transform.position.x, transform.position.y - firePadding, transform.position.z);
        GameObject projectile = Instantiate(chasingProjectilePrefab,
            firePos,
            projectilePrefab.transform.rotation) as GameObject;

    }
    private void OnTriggerEnter(Collider other)
    {
        // trigger when player projectile hits the enemy (Layers)
        ImpactController impactProcess = other.gameObject.GetComponent<ImpactController>();
        if (impactProcess == null) { return; }
        ProcessHit(impactProcess);
    }

    private void ProcessHit(ImpactController impactProcess)
    {
        health -= impactProcess.GetDamage();
        impactProcess.Hit();
        if (health <= 0)
        {
            Die();
            OnDieDropPower();
            OnDieDropGold();

        }
        else
        {
            impactProcess.ImapctProcess();
        }
    }

    private void OnDieDropGold()

    {
        if (Random.Range(1, 100) <= chanceOfDropingGold)
        {
            CoinsController.instance.DropGold(this.transform);
        }
    }

    private void OnDieDropPower()
    {
        if (Random.Range(1, 100) <= chanchToDropPower)
        {
            PowerUpController.instance.InstatiateRandomPower(this.transform);
        }
    }

    private void Die()
    {
        GamePlayController.instance.AddToScore(scoreValue);
        Destroy(gameObject);

        //Subtract enemies count
        LevelSpawner.instance.totalEnemiesPerWave--;
        SoundEffectController.instance.EnemyDeathSound();
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }
}
