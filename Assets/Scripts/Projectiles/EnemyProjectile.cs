using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject cocoonPrefab;
    [SerializeField] GameObject projectile;
    bool isDroping;
    private void Start()
    {

        StartCoroutine(FireProcess());
    }

    void Update()
    {
        if (isDroping)
        {
            transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;

        }

    }

    IEnumerator FireProcess()
    {
        yield return new WaitForSeconds(2);
        isDroping = true;
        cocoonPrefab.SetActive(false);
        projectile.SetActive(true);
    }
}
