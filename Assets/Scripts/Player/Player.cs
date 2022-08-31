using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    public static Player instance;


    [SerializeField] private float speed = 5f;
    [SerializeField] int health;

    [SerializeField] bool isAlwaysShooting = false;
    [SerializeField] bool isMovingWithMouse = true;
    [SerializeField] ParticleSystem rocketFlames;
    [SerializeField] ParticleSystem shootingFlames;

    PlayerController playerController;
    GunController gunController;
    AudioSource audioSource;
    Animator anim;

    int upgradeRank = 1;
    bool isGameStatePLAY;

    [Header("GameDev Settings")]
    [SerializeField] bool collideWithEnemy = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        GamePlayController.OnGameStateChange += GameStateChangeHandle;
    }

    private void OnDestroy()
    {
        GamePlayController.OnGameStateChange -= GameStateChangeHandle;
    }
    private void Start()
    {
        audioSource.Stop();
    }
    void Update()
    {
        if (isGameStatePLAY)
        {
            audioSource.loop = isAlwaysShooting;
            Move();

            Shoot();

        }
    }

    void Move()
    {
        // Player Inputs From keyboard
        Vector3 translation = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        translation *= speed * Time.deltaTime;
        playerController.KeyboardMovement(translation);

        if (isMovingWithMouse)
        {
            playerController.MoveTowards(speed);
        }

        if (Input.touchCount > 0)
        {
            playerController.MoveTowards(speed);
        }
    }
    void Shoot()
    {
        if (isAlwaysShooting)
        {
            gunController.Shoot(upgradeRank);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                gunController.Shoot(upgradeRank);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Projectile")
        {
            switch (other.tag)
            {
                case "Projectile":
                    ImpactController damageDealer = other.gameObject.GetComponent<ImpactController>();
                    if (damageDealer == null) { return; }
                    ProcessHit(damageDealer);
                    break;
                case "powerUpgradeGun":
                    upgradeRank++;
                    break;
                case "Enemy":
                    if (collideWithEnemy)
                    {
                        PlayerDeath();
                    }
                    break;
            }

        }
    }

    /* comuniacates with the impact controller of each projectile ans process damage health
    and die method 
    */

    private void ProcessHit(ImpactController impactController)
    {

        health -= impactController.GetDamage();
        GameUIController.instance.UpdateHealthText();
        impactController.Hit();

        if (health <= 0)
        {
            PlayerDeath();
        }
        else
        {
            impactController.ImapctProcess();
        }
    }

    private void PlayerDeath()
    {
        SoundEffectController.instance.PlayerDeathSound();
        //Destroy(gameObject);
        GamePlayController.instance.UpdateState(GameState.DEFEAT);
    }
    public int GetHealth()
    {
        return health;
    }

    public bool GetIsAlwaysShooting()
    {
        return isAlwaysShooting;
    }

    void GameStateChangeHandle(GameState state)
    {
        isGameStatePLAY = (state == GameState.PLAY);
        if (state == GameState.PLAY)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            audioSource.Play();
            anim.enabled = false;
            rocketFlames.Play();
            shootingFlames.Play();
        }
    }

    public void PlayerAnimation()
    {
        anim.Play("Intro");
    }
}

