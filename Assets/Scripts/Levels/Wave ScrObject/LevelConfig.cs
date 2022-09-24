using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] List<WaveConfig> waves;

    public List<WaveConfig> GetWaves()
    {
        return waves;
    }
}
