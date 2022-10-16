using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionSpawn : MonoBehaviour
{
    public DivSet divSet;
    [SerializeField] DivisionConfiguration divisionConfig;

    [HideInInspector] public List<Transform> waypoints;
    [HideInInspector] public List<Transform> formationWaypoints;

    GameObject[] enemyPrefabs;
    int index;
    private void OnValidate()
    {
        switch (divSet)
        {
            case DivSet.Endless:
                divisionConfig.general.endlessMove = true;
                divisionConfig.general.isFormMoving = false;
                divisionConfig.general.formation = null;
                divisionConfig.formMove = null;
                divisionConfig.aISettings = null;
                break;
            case DivSet.Formation:
                divisionConfig.general.endlessMove = false;
                break;
            case DivSet.TwoPoints:
                divisionConfig.general.endlessMove = false;
                divisionConfig.smooth.smoothMovement = true;
                break;
            case DivSet.ChasingPlayer:
                divisionConfig.general.isChasingPlayer = true;
                divisionConfig.general.isFormMoving = false;
                divisionConfig.general.endlessMove = false;
                divisionConfig.formMove = null;

                break;
        }
    }
    void Start()
    {
        waypoints = new List<Transform>();
        formationWaypoints = new List<Transform>();

        if (divisionConfig.general.path != null)
        {
            foreach (Transform child in divisionConfig.general.path.transform)
            {
                waypoints.Add(child);
            }
        }

        if (divisionConfig.general.formation != null)
        {
            foreach (Transform child in divisionConfig.general.formation.transform)
            {
                formationWaypoints.Add(child);
            }
        }

        EnemyCount.instance.CountEnemiesAtScene(divisionConfig.spawns.numberOfEnemies);
        StartCoroutine(InstantiateDivision());
    }
    IEnumerator InstantiateDivision()
    {

        enemyPrefabs = divisionConfig.general.enemyPrefabs;

        for (index = 0; index < divisionConfig.spawns.numberOfEnemies; index++)
        {

            GameObject newEnemy = InstatiatePrefab(divSet);
            EnemyPathfinding ep = newEnemy.GetComponent<EnemyPathfinding>();
            ep.SetDivisionConfiguration(divisionConfig, this, id: index);

            switch (divSet)
            {
                case DivSet.Endless:
                    ep.UpdateState(EnemyState.DivisionToPath);
                    break;

                case DivSet.Formation:
                    ep.UpdateState(EnemyState.DivisionToPath);
                    break;

                case DivSet.TwoPoints:
                    ep.UpdateState(EnemyState.PointToPoint);
                    break;
                case DivSet.ChasingPlayer:
                    ep.UpdateState(EnemyState.ChasingPlayer);
                    break;
            }

            yield return new WaitForSeconds(divisionConfig.spawns.timeBetweenSpawns);
        }
    }
    private GameObject InstatiatePrefab(DivSet set)
    {
        int i = Random.Range(0, enemyPrefabs.Length);
        int idPos;
        if (set == DivSet.TwoPoints) idPos = index;
        else idPos = 0;

        return Instantiate(enemyPrefabs[i],
                           waypoints[idPos].position,
                           enemyPrefabs[i].transform.rotation);
    }
}
public enum DivSet
{
    Endless,
    Formation,
    TwoPoints,
    ChasingPlayer
}