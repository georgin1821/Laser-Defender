using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionPathAndFormation : DivisionAbstract
{
    [SerializeField] GameObject Path;
    [SerializeField] GameObject Formation;

    [SerializeField] EnemyAISettings enemyAISettings;
    [SerializeField] FormMoveSettings formMoveSettings;
    [SerializeField] SpawnsSettings spawnsSettings;
    [SerializeField] SmoothDeltaSettings smoothDeltaSettings;
    [SerializeField] RotationSettings rotationSettings;

    private void OnValidate()
    {
        spawnsSettings.endlessMove = false;
    }
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
                                              waypoints[0].position,
                                              enemyPrefabs[i].transform.rotation) as GameObject;

            newEnemy.transform.SetParent(obj.transform);

            EnemyPathfinding ep = newEnemy.GetComponent<EnemyPathfinding>();

            ep.SetDivisionConfiguration(divisionConfig, divisionAbstract: this, id: index);
            ep.SetFormMoveConfg(formMoveSettings);
            ep.SetAIConfg(enemyAISettings);
            ep.SetSpawnConfig(spawnsSettings);
            ep.SetRotationConfig(rotationSettings);
            ep.SetSmoothDeltaConfig(smoothDeltaSettings);

            ep.UpdateState(EnemyState.DivisionToPath);


            yield return new WaitForSeconds(divisionConfig.timeBetweenSpawns);
        }
    }

}
