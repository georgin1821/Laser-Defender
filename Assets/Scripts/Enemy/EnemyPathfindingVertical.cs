using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindingVertical : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float speedRF = 0.4f;

    float yMax;
    IEnumerator co1;
    private void Start()
    {
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + 2;
    }
    private void Update()
    {
        DestroyShipIfOutsideView();
    }

    private void DestroyShipIfOutsideView()
    {
        if (transform.position.y < -yMax)
        {
            StopRoutine();
            Destroy(this.gameObject);
            EnemyCount.instance.Count--;
        }
    }

    public void StartDeployment()
    {
        StopRoutine();
        co1 = Deployment();
        StartCoroutine(co1);
    }
    void StopRoutine()
    {
        StopAllCoroutines();
    }

    IEnumerator Deployment()
    {
        speed = Random.Range(speed - speedRF, speed + speedRF);

        while (true)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }
    }


}
