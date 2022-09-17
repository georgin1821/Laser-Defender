using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public static SoundEffectController instance = null;

    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource UISource;


    [Header("Audio")]
    [SerializeField] [Range(0, 1f)] float randomVolume;

    [SerializeField] AudioClip playerDeathClip;
    [SerializeField] [Range(0, 1)] float playerDeathVolume;

    [SerializeField] AudioClip enemyDeathClip;
    [SerializeField] [Range(0, 1)] float enemyDeathVolume;

    [SerializeField] AudioClip powerUp;
    [SerializeField] [Range(0, 1)] float powerUpVolume;

    [SerializeField] AudioClip collectGold;
    [SerializeField] [Range(0, 1)] float collectGoldVolume;

    [SerializeField] AudioClip shootRockets;
    [SerializeField] [Range(0, 1)] float shootRocketsVolume;

    [SerializeField] AudioClip shields;
    [SerializeField] [Range(0, 1)] float shieldsVolume;

    [SerializeField] AudioClip defeatClip;
    [SerializeField] [Range(0, 1)] float defeatClipVolume;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayAudioClip(AudioClip clip, float volume)
    {
        UISource.PlayOneShot(clip, volume);
    }

    public void StopSFXAudio()
    {
        SFXSource.Stop();
        UISource.Stop(); 
    }
    public void EnemyDeathSound()
    {
        AudioComtroller.Instance.PlayAudio(AudioType.EnemyDeathSound, true);
      //  SFXSource.volume = enemyDeathVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
      //  SFXSource.PlayOneShot(enemyDeathClip);
    }
    public void PlayDefeatClip()
    {
        SFXSource.volume = defeatClipVolume;
        SFXSource.PlayOneShot(defeatClip);
    }
    public void PlayerDeathSound()
    {
        SFXSource.volume = playerDeathVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        SFXSource.PlayOneShot(playerDeathClip);
    }
    public void PlayerHasShields()
    {
        SFXSource.volume = shieldsVolume;
        SFXSource.PlayOneShot(shields);
    }
    public void PowerUpSFX()
    {
        AudioComtroller.Instance.PlayAudio(AudioType.PowerUp, true);
       // SFXSource.volume = enemyDeathVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
       // SFXSource.PlayOneShot(powerUp);
    }
    public void PlayerShootRockets()
    {
        SFXSource.PlayOneShot(shootRockets, shootRocketsVolume);
    }
    public void CollectGoldSound()
    {
        SFXSource.volume = collectGoldVolume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        SFXSource.PlayOneShot(collectGold);
    }



}
