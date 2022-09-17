﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : GameElement
{

    [SerializeField] int damage = 100;
    [SerializeField] AudioClip impactClip;
    [SerializeField] float impactClipVolume;

    private void Start()
    {
        SetLevelOfDifficulty();
    }

    public int Damage
    {
        get { return Damage; }
        set { }
    }
    public int GetDamage()
    {
        return damage;
    }
    public void ImapctProcess()
    {
        VFXController.instance.EnemyLaserImpact(transform);
        if (impactClip != null)
        {
            SoundEffectController.instance.PlayAudioClip(impactClip, impactClipVolume);
        }
                Destroy(gameObject);
    }

    public void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.instance.difficulty;
        damage += (int)(diff * .5f);
    }

}
