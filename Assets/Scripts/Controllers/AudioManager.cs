using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    [SerializeField] AudioSource audioSource;

    public void PlayOneShotClip(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
