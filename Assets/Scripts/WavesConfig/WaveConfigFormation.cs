using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config Formation")]
public class WaveConfigFormation : ScriptableObject

{
    public GameObject formationPrefab;

   public List<Transform> GetFormWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in formationPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints; ;

    }

}





