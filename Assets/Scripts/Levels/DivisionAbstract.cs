using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DivisionAbstract : MonoBehaviour
{
    [SerializeField] protected DivisionConfiguration divisionConfig;

    protected GameObject path;
    protected GameObject formation;

    [HideInInspector] public List<Transform> waypoints;
    [HideInInspector] public List<Transform> formationWaypoints;

    protected virtual void Start()
    {
        waypoints = new List<Transform>();
        formationWaypoints = new List<Transform>();

        if (path != null)
        {
            foreach (Transform child in path.transform)
            {
                waypoints.Add(child);
            }
        }

        if (formation != null)
        {
            foreach (Transform child in formation.transform)
            {
                formationWaypoints.Add(child);
            }
        }

        EnemyCount.instance.CountEnemiesAtScene(divisionConfig.numberOfEnemies);
        StartCoroutine(InstantiateDivision());
    }

    protected abstract IEnumerator InstantiateDivision();
}
