using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionEndlessMove : DivisionAbstract
{
    [SerializeField] private GameObject Path;
    private void OnValidate()
    {
        divisionConfig.endlessMove = true;
    }
    private void Awake()
    {
        this.path = Path;
    }
    protected override IEnumerator InstantiateDivision()
    {
        GameObject[] enemyPrefabs = divisionConfig.enemyPrefabs;
        GameObject obj = new GameObject("Wave");

        for (int index = 0; index < divisionConfig.numberOfEnemies; index++)
        {
            int i = Random.Range(0, enemyPrefabs.Length);

            GameObject newEnemy = Instantiate(enemyPrefabs[i],
                                              waypoints[0].position,
                                              enemyPrefabs[i].transform.rotation) as GameObject;
            newEnemy.transform.SetParent(obj.transform);

            EnemyPathfinding ep = newEnemy.GetComponent<EnemyPathfinding>();
            ep.SetDivisionConfiguration(divisionConfig, divisionAbstract: this);
            ep.UpdateState(EnemyState.SinglePath);

            yield return new WaitForSeconds(divisionConfig.timeBetweenSpawns);
        }

    }
}
