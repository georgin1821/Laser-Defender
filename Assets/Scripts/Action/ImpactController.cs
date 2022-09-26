using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{

    [SerializeField] int damage = 100;
    [SerializeField] AudioType audioType;
    public int Damage { get; set; }

    private void Start()
    {
        SetLevelOfDifficulty();
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
