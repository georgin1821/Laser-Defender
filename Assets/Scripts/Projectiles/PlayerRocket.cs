using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{

    [SerializeField] float rotationSpeed;
    [SerializeField] float speed;
    //movment
    Vector3 targetPos;

     GameObject[] targets;
    private void Start()
    {
        GameObject[] targets = Player.instance.Targets;
        int index = Random.Range(0, targets.Length);
        targetPos = targets[index].transform.position;

        StartCoroutine(SeekTargets());
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
    IEnumerator SeekTargets()
    {
        yield return new WaitForSeconds(.5f);

        while (true)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }
    }
}
