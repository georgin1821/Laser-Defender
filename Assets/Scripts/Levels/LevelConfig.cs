using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] List<WaveScripts> waves;

    public List<WaveScripts> GetWaves()
    {

        
        return waves;

    }
}
