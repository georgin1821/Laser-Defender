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
    [SerializeField] ParticleSystem engineFlames;
    [SerializeField] ParticleSystem shootingFlames;
    [SerializeField] GameObject rocketPrefab, skill1;
    [SerializeField] Transform firePos;
    [SerializeField] GameObject shieldsVFX;

    PlayerController playerController;
    GunController gunController;
    AudioSource audioSource;
    Animator anim;
    float arcAngle = 40;
    bool playerHasShield;

    Coroutine co;
    GameObject shields;

    public GameObject[] Targets { get; set; }

    public int UpgradeRank { get; private set; }

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

    private void OnEnable()
    {
        GameUIController.instance.SetPlayerStatus();
    }
    private void OnDestroy()
    {
        GamePlayController.OnGameStateChange -= GameStateChangeHandle;
    }
    private void Start()
    {
        audioSource.Stop();
        UpgradeRank = 1;
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
            gunController.Shoot(UpgradeRank);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                gunController.Shoot(UpgradeRank);
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

                case "gunUpgrade":
                    if (UpgradeRank < 6)
                    {
                        UpgradeRank++;
                        GameUIController.instance.UpdateRankStatus();
                    }
                    break;
                case "gainRockets":
                    FireRockets();
                    break;
                case "Enemy":
                    if (collideWithEnemy)
                    {
                        if (!playerHasShield)
                        {
                            PlayerDeath();
                        }
                                            }
                    break;
                case "shield":
                    if (co != null)
                    {
                        StopCoroutine(co);
                        Destroy(shields);
                    }
                    co = StartCoroutine(ShieldsCountDown());
                    break;
            }

        }
    }
    IEnumerator ShieldsCountDown()
    {

        playerHasShield = true;
        shields = Instantiate(shieldsVFX, transform.position, Quaternion.identity);
        shields.transform.SetParent(gameObject.transform);
        SoundEffectController.instance.PlayerHasShields();
        yield return new WaitForSeconds(4);
        playerHasShield = false;
        Destroy(shields);

    }
    private void ProcessHit(ImpactController impactController)
    {
        if (!playerHasShield)
        {
            health -= impactController.GetDamage();

            //Delegate
            GameUIController.instance.UpdateHealthText();
            impactController.ImapctProcess();
        }

        if (health <= 0)
        {
            PlayerDeath();
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
        anim.enabled = (state != GameState.PLAY);
        isGameStatePLAY = (state == GameState.PLAY);

        if (state == GameState.PLAY)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            audioSource.Play();
            //anim.enabled = false;
            engineFlames.Play();
            shootingFlames.Play();
        }
        if (state == GameState.INIT)
        {
            UpgradeRank = 1;
        }
    }
    public void PlayerAnimation()
    {
        //Debug.Log("Player Intro Anim");
        anim.enabled = true;
        anim.Play("Intro");
    }
    public void FireRockets()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab, firePos.position, transform.rotation);
            rocket.transform.Rotate(0, 0, arcAngle - i * 15);
            Targets = GameObject.FindGameObjectsWithTag("Enemy");
            SoundEffectController.instance.PlayerShootRockets();
        }
    }

    public void Skill1()
    {
        Instantiate(skill1, transform.position, Quaternion.identity);
    }

}

