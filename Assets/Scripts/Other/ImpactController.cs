using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{

    public int Damage = 100;
    [SerializeField] AudioType audioType;

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
        float diff = GamePlayController.instance.Difficulty;
        Damage += (int)(diff * .5f);
    }

}
