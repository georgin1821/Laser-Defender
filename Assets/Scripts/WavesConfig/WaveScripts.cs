using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Scripts")]

public class WaveScripts : ScriptableObject
{
    public List<GameObject> listWaveScripts;

    public List<GameObject> GetWaveScripts()
    {
        return listWaveScripts;
    }
}
