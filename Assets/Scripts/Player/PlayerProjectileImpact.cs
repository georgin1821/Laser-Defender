using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileImpact : MonoBehaviour
{
    [SerializeField] int damage = 100;
    [SerializeField] GameObject impactVFX;
    [SerializeField] AudioClip impactClip;
    [SerializeField] float impactClipVolume;

    public int Damage
    {
        get { return Damage; }
        set { }
    }


    public int GetDamage()
    {
        damage += GamePlayController.instance.ShipPower / 10;
        return damage;
    }

    public void ImapctProcess()
    {
        GameObject explotion = Instantiate(impactVFX, transform.position, Quaternion.identity);
        if (impactClip != null)
        {
            SoundEffectController.instance.PlayAudioClip(impactClip, impactClipVolume);
        }

        Destroy(explotion, 1f);

        Destroy(gameObject);
    }

}
