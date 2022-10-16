using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Gun))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    #region All Variables
    public static Player instance;

    [SerializeField] int health;

    [SerializeField] ParticleSystem engineFlames;
    [SerializeField] ParticleSystem shootingFlames;
    [SerializeField] GameObject rocketPrefab;
    [Space(10)]
    [Tooltip("Fire posiyion")]
    [SerializeField] Transform firePos;
    [SerializeField] GameObject shieldsVFX;
    GameObject redFlashImage;

    AudioSource audioSource;
    Animator anim;
    float arcAngle = 40;
    bool playerHasShield;
    bool isGameStatePLAY;
    Coroutine co;
    GameObject shields;
    public int GameHealth { get; private set; }
    public GameObject[] Targets { get; set; }
    public int UpgradeRank { get; private set; }

    [Header("GameDev Settings")]
    [SerializeField] bool collideWithEnemy = true;
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        redFlashImage = GameObject.Find("Red Flash");
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
    void Update()
    {
        if (isGameStatePLAY)
        {
            audioSource.loop = true;
        }
    }

    public void StopShootingClip()
    {
        audioSource.Stop();
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
            GameHealth -= impactController.Damage;

            GameUIController.instance.UpdatePlayerHealthUI();
            impactController.ImapctProcess();
        }
        redFlashImage.GetComponent<RedFlashAnim>().Flash();
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

