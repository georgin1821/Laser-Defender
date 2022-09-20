using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Gun))]
public class Player : MonoBehaviour
{

    public static Player instance;

    [SerializeField] int health;
    public int GameHealth { get; private set; }

    [SerializeField] ParticleSystem engineFlames;
    [SerializeField] ParticleSystem shootingFlames;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] Transform firePos;
    [SerializeField] GameObject shieldsVFX;

    AudioSource audioSource;
    Animator anim;
    float arcAngle = 40;
    bool playerHasShield;
    Coroutine co;
    GameObject shields;
    bool isGameStatePLAY;

    public GameObject[] Targets { get; set; }
    public int UpgradeRank { get; private set; }

    [Header("GameDev Settings")]
    [SerializeField] bool collideWithEnemy = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (GamePlayController.instance != null)
        {
            GamePlayController.OnGameStateChange += GameStateChangeHandle;
        }
        else
        {
            Debug.Log("GamePlayController instance is null");
        }
    }
    private void OnDestroy()
    {
        GamePlayController.OnGameStateChange -= GameStateChangeHandle;
    }
    private void Start()
    {
        StopShootingClip();
        GameUIController.instance.SetPlayerStatus();
    }

    public void StopShootingClip()
    {
        audioSource.Stop();
    }

    void Update()
    {
        if (isGameStatePLAY)
        {
            audioSource.loop = true;
        }
    }

    IEnumerator ShieldsCountDown()
    {

        playerHasShield = true;
        shields = Instantiate(shieldsVFX, transform.position, Quaternion.identity);
        shields.transform.SetParent(gameObject.transform);
        AudioController.Instance.PlayAudio(AudioType.PlayerShields);
        yield return new WaitForSeconds(4);
        playerHasShield = false;
        Destroy(shields);

    }
    private void ProcessHit(ImpactController impactController)
    {
        if (!playerHasShield)
        {
            GameHealth -= impactController.GetDamage();

            GameUIController.instance.UpdateHealthText();
            impactController.ImapctProcess();
        }

        if (GameHealth <= 0)
        {
            PlayerDeath();
        }
    }
    private void PlayerDeath()
    {
        //Destroy(gameObject);
        AudioController.Instance.PlayAudio(AudioType.PlayerDeath);
        GamePlayController.instance.UpdateState(GameState.DEFEAT);
    }
    void GameStateChangeHandle(GameState state)
    {
        anim.enabled = (state != GameState.PLAY);
        isGameStatePLAY = (state == GameState.PLAY);

        if (state == GameState.PLAY)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            audioSource.Play();
            engineFlames.Play();
            shootingFlames.Play();
        }
        if (state == GameState.INIT)
        {
            UpgradeRank = 1;
            Gun.instance.Upgrade = UpgradeRank;
            GameHealth = health;
        }
    }
    public void PlayerAnimation()
    {
        //Speed Level
        if (GameManager.Instance.isSpeedLevel)
        {
            return;
        }
        else
        {
            anim.Play("Intro");
        }
    }
    public void FireRockets()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab, firePos.position, transform.rotation);
            rocket.transform.Rotate(0, 0, arcAngle - i * 15);
            // SoundEffectController.instance.PlayerShootRockets();
            AudioController.Instance.PlayAudio(AudioType.PalyerShootRockets);
            Targets = GameObject.FindGameObjectsWithTag("Enemy");
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
                    if (UpgradeRank < 9)
                    {
                        UpgradeRank++;
                        Gun.instance.Upgrade = UpgradeRank;
                        GameUIController.instance.UpdateRankStatus();
                    }
                    break;
                case "gainRockets":
                    FireRockets();
                    break;
                case "Enemy":
                    if (GamePlayController.instance.state == GameState.PLAY)
                    {
                        if (collideWithEnemy)
                        {
                            if (!playerHasShield)
                            {
                                PlayerDeath();
                            }
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
}

