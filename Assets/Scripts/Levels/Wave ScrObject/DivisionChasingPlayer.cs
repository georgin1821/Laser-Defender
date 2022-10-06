using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionChasingPlayer : DivisionAbstract
{
    [SerializeField] GameObject Path;
    [SerializeField] GameObject Formation;

    [SerializeField] SmoothDeltaSettings smoothDeltaSettings;
    [SerializeField] AIChasingPlayerSettings aIChasingPlayerSettings;

    private void OnValidate()
    {
        if (divisionConfig.isChasingPlayer == true) divisionConfig.isFormMoving = false;
           }
    private void Awake()
    {
        this.path = Path;
        this.formation = Formation;
    }
    protected override IEnumerator InstantiateDivision()
    {
        GameObject[] enemyPrefabs = divisionConfig.enemyPrefabs;

        for (int index = 0; index < divisionConfig.numberOfEnemies; index++)
        {
            int i = Random.Range(0, enemyPrefabs.Length);

            GameObject newEnemy = Instantiate(enemyPrefabs[i],
                                              waypoints[0].position,
                                              enemyPrefabs[i].transform.rotation) as GameObject;

            EnemyPathfinding ep = newEnemy.GetComponent<EnemyPathfinding>();

            ep.SetDivisionConfiguration(divisionConfig, divisionAbstract: this, id: index);
            ep.SetSmoothDeltaConfig(smoothDeltaSettings);
            ep.SetAIChasingPlayerConfig(aIChasingPlayerSettings);

            ep.UpdateState(EnemyState.PointToPoint);

            yield return new WaitForSeconds(divisionConfig.timeBetweenSpawns);

        }
    }
}
