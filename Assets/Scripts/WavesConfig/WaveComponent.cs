using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Component")]

public class WaveComponent : ScriptableObject
{
    public List<WaveScripts> waveScripts;
}
