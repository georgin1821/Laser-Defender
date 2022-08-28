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
    [SerializeField] bool isMovingFromKeybord;
    [SerializeField] bool isMovingwithMouse;

    [SerializeField] bool canPlayerCollideWithEnemy = true;

    private PlayerController playerController;
    private GunController gunController;

    //private bool gunIsUpgaded;
    public bool gunIsUpgraded
    { get; set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
    }
    void Update()
    {
        Move();

        Shoot();
    }

    void Move()
    {
        if (isMovingFromKeybord)
        {
            Vector3 translation = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
            translation *= speed * Time.deltaTime;
            playerController.KeyboardMovement(translation);
        }
        else if (Input.touchCount > 0)
        {
            playerController.MobileMovement(speed);
        }
        else if (isMovingwithMouse)
        {
            MovingWithMouse();
        }
    }
    void Shoot()
    {
        if (isAlwaysShooting)
        {
            Fire();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        gunController.Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        ImpactController damageDealer = other.gameObject.GetComponent<ImpactController>();
        //if (other.tag == "Projectile")
        {
            switch (other.tag)
            {
                case "Projectile":
                    if (damageDealer == null) { return; }
                    ProcessHit(damageDealer);
                    break;
                case "powerUpgradeGun":
                    gunIsUpgraded = true;
                    break;
                case "Enemy":
                    if (canPlayerCollideWithEnemy)
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

    void MovingWithMouse()
    {
        playerController.MoveTowards(speed);
    }

}

