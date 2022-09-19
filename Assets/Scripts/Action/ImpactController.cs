using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : GameElement
{

    [SerializeField] int damage = 100;
    public AudioType audioType;

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
        if (audioType != AudioType.None)
        {
            AudioController.Instance.PlayAudio(audioType);
        }
        Destroy(gameObject);
    }

    public void SetLevelOfDifficulty()
    {
        float diff = GamePlayController.instance.difficulty;
        damage += (int)(diff * .5f);
    }

}
