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
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float chanceToFire;
    [Header("VFX")]
    [SerializeField] GameObject deathVFX;

    [Header("PowerUps")]
    [SerializeField] int chanchToDropPower;
    [SerializeField] int chanceOfDropingGold;

    private void Start()
    {
        SetLevelOfDifficulty();
        InvokeRepeating("FireChance", 1f, 1f);
    }

    public void Fire()
    {
        //float firePadding = GetComponent<Renderer>().bounds.size.y / 2;
        Vector3 firePos = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab,
            firePos,
            Quaternion.identity) as GameObject;
        //projectile.transform.SetParent(this.transform);

    }

    private void OnTriggerEnter(Collider other)
    {
        
        // trigger when player projectile hits the enemy (Layers)
        PlayerProjectileImpact impactProcess = other.gameObject.GetComponent<PlayerProjectileImpact>();
        if (impactProcess == null) { return; }
        ProcessHit(impactProcess);
    }

    private void ProcessHit(PlayerProjectileImpact impactProcess)
    {
        health -= impactProcess.GetDamage();
        impactProcess.ImapctProcess();
        if (health <= 0)
        {
            Die();
            OnDieDropPower();
            OnDieDropGold();
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
        EnemyCount.instance.count--;
        SoundEffectController.instance.EnemyDeathSound();
        //  GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        //  Destroy(explosion, 1f);
    }

    public void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.instance.difficulty;
        health += health * diff;
        scoreValue += (int)(diff * .5f);
    }

    void FireChance()
    {
        if (Random.Range(1, 100) <= chanceToFire)
        {
            Fire();
        }

    }

}
