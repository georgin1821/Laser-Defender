using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillImpact : PlayerProjectileImpact
{
    [SerializeField] AudioClip impactClip;
    [SerializeField] float impactClipVolume;
    override public void ImapctProcess()
    {
        GameObject explotion = Instantiate(impactVFX, transform.position, Quaternion.identity);
        if (impactClip != null)
        {
            SoundEffectController.instance.PlayAudioClip(impactClip, impactClipVolume);
        }

        Destroy(explotion, 1f);

    }
}
