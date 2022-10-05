using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Division2WaysPath : DivisionAbstract
{
    [SerializeField] GameObject Path;
    [SerializeField] GameObject Formation;

    private void Awake()
    {
        this.path = Path;
        this.formation = Formation;
    }

    protected override IEnumerator InstantiateDivision()
    {
        GameObject[] enemyPrefabs = divisionConfig.enemyPrefabs;
        GameObject obj = new GameObject("Wave");

        for (int index = 0; index < divisionConfig.numberOfEnemies; index++)
        {

            int i = Random.Range(0, enemyPrefabs.Length);

            GameObject newEnemy = Instantiate(enemyPrefabs[i],
            waypoints[index].position,
            enemyPrefabs[i].transform.rotation) as GameObject;

            newEnemy.transform.SetParent(obj.transform);

            EnemyPathfinding ep = newEnemy.GetComponent<EnemyPathfinding>();
            ep.SetDivisionConfiguration(divisionConfig,this, id: index);
            ep.UpdateState(EnemyState.PointToPoint);

            yield return new WaitForSeconds(divisionConfig.numberOfEnemies);
        }
    }
}

