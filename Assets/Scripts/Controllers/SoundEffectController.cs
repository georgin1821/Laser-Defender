using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public static SoundEffectController instance = null;

    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource shootSource;
    [SerializeField] AudioSource mousicSource;


    [Header("Audio")]
    [SerializeField] [Range(0, 1f)] float randomVolume;

    [SerializeField] AudioClip playerDeathClip;
    [SerializeField] [Range(0, 1)] float playerDeathVolume;

    [SerializeField] AudioClip playerShootClip;
    [SerializeField] [Range(0, 1)] float playerShootClipVolume;

    [SerializeField] AudioClip enemyDeathClip;
    [SerializeField] [Range(0, 1)] float enemyDeathVolume;

    [SerializeField] AudioClip powerUp;
    [SerializeField] [Range(0, 1)] float powerUpVolume;

    [SerializeField] AudioClip collectGold;
    [SerializeField] [Range(0, 1)] float collectGoldVolume;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void PlayAudioClip(AudioClip clip, float volume)
    {
       // Debug.Log("play " + clip + " volume: " + volume);
        SFXSource.PlayOneShot(clip, volume);
    }

    public void EnemyDeathSound()
    {
        SFXSource.volume = enemyDeathVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        SFXSource.PlayOneShot(enemyDeathClip);
    }

    public void PlayerDeathSound()
    {
        SFXSource.volume = playerDeathVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        SFXSource.PlayOneShot(playerDeathClip);
    }

    public void PowerUpSFX()
    {
        SFXSource.volume = enemyDeathVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        SFXSource.PlayOneShot(powerUp);
    }


    public void PlayerShootClip()
    {
        shootSource.volume = playerShootClipVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        shootSource.PlayOneShot(playerShootClip);
    }


    public void CollectGoldSound()
    {
        SFXSource.volume = collectGoldVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        SFXSource.PlayOneShot(collectGold);
    }



}
