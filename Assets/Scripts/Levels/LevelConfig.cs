using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] List<WaveConfig> waves;

    public List<WaveConfig> GetWaves()
    {
        return waves;
    }
}
