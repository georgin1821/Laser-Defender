using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindingVertical : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject line;
    [SerializeField] AudioClip laserClip;

    float yMax;
    float randFactor = 0.4f;
    IEnumerator co1, co2;
    private void Start()
    {
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + 2;
    }
    private void Update()
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
        co2 = LaserSkill();
        StartCoroutine(co1);
        StartCoroutine(co2);
    }
    void StopRoutine()
    {
        StopAllCoroutines();
    }

    IEnumerator Deployment()
    {
        speed = Random.Range(speed - randFactor, speed + randFactor);

        while (true)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator LaserSkill()
    {
        float range = Random.Range(-1f, 4.5f);
        while (transform.position.y > range)
        {
            yield return null;
        }

        line.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        line.SetActive(false);
        laser.SetActive(true);
        AudioController.Instance.PlayAudio(AudioType.EnemyLaserSkill);

        float time = 3;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        laser.SetActive(false);
       // SoundEffectController.instance.StopSFXAudio();
        AudioController.Instance.StopAudio(AudioType.EnemyLaserSkill);
    }

}



