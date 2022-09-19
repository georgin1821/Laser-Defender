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
    [SerializeField] int chanseOfDropGems;

    bool isDead;

    private void Start()
    {
        SetLevelOfDifficulty();
        InvokeRepeating("FireChance", 3f, 3f);
    }

    public void Fire()
    {
        Vector3 firePos = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        Instantiate(projectilePrefab,
             firePos,
             Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerProjectileImpact impactProcess = other.gameObject.GetComponent<PlayerProjectileImpact>();
        if (impactProcess == null) { return; }
        ProcessHit(impactProcess);
    }

    private void ProcessHit(PlayerProjectileImpact impactProcess)
    {
        health -= impactProcess.GetDamage();
        impactProcess.ImapctProcess();

        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
            OnDieDropPower();
            OnDieDropGold();
            OnDiewDropGems();
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
    private void OnDiewDropGems()
    {
        if (Random.Range(1, 100) <= chanchToDropPower)
        {
            PowerUpController.instance.InstatiateRandomPower(this.transform);
        }
    }
    private void Die()
    {
        EnemyCount.instance.count--;
        GamePlayController.instance.AddToScore(scoreValue);
        AudioController.Instance.PlayAudio(AudioType.EnemyDeathSound);
        VFXController.instance.EnemyDeath(transform);
        Destroy(gameObject);
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
