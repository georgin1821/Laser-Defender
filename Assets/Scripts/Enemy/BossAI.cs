using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject path1, path2;
    [SerializeField] float speed, rotaionSpeed;
    [HideInInspector] public List<Transform> routeTrans1, routeTrans2;
    FightStage stage;
    enum FightStage
    {
        INTRO,
        PHASE1,
        PHASE2,
        DEATH
    }
    private void Awake()
    {
        GetWaypointsFropPrefab();

        EnemyCount.instance.Count++;
    }

    private void GetWaypointsFropPrefab()
    {
        foreach (Transform child in path1.transform)
        {
            routeTrans1.Add(child);
        }
        foreach (Transform child in path2.transform)
        {
            routeTrans2.Add(child);
        }
    }

    private void Start()
    {
        UpdateState(FightStage.INTRO);
    }

    void UpdateState(FightStage newStage)
    {
        stage = newStage;
        switch (newStage)
        {
            case FightStage.INTRO:
                StartCoroutine(IntroStateRoutine());
                break;
            case FightStage.PHASE1:
                break;
            case FightStage.PHASE2:
                break;
            case FightStage.DEATH:
                break;
            default:
                break;
        }
    }


    IEnumerator IntroStateRoutine()
    {
        AudioController.Instance.LoopAudio(AudioType.bossMoving);
        int index = 0;
        Vector3 startPos = routeTrans1[0].position;
        transform.position = startPos;

        while (index < routeTrans1.Count - 1)
        {
            Vector3 nextPos = routeTrans1[index + 1].position;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPos) < 0.1f)
            {
                index++;
            }
            yield return null;
        }

    }
}
