using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerOld : Singleton<AudioManagerOld>
{

    [SerializeField] AudioSource audioSource;

    public void PlayOneShotClip(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
