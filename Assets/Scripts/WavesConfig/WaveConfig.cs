using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config")]

public class WaveConfig : ScriptableObject
{
    public List<GameObject> squads;

    public List<GameObject> GetSquads()
    {
        return squads;
    }

}
