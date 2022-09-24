using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config")]

public class WaveConfig : ScriptableObject
{
    [SerializeField] List<GameObject> divisions;

    public List<GameObject> GetDivisions()
    {
        return divisions;
    }

}
