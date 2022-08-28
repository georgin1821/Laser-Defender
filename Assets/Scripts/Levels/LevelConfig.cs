using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] List<WaveConfig> waves;

    public List<WaveConfig> GetWaves()
    {
        var waveConfigs = new List<WaveConfig>();

        foreach (WaveConfig child in waves)
        {
            waveConfigs.Add(child);
        }
        return waveConfigs;

    }
}
