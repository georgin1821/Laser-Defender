using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCocoon : EnemyProjectileController
{
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] Transform fireTrans;
    [SerializeField] AudioType fireSound;


    GameObject projectile;
    private void Start()
    {
        InvokeRepeating("FireChance", minTimeToFire, maxTimeToFire);
    }


    IEnumerator FireProcess()
    {
        GameObject vfx = Instantiate(fireParticle, fireTrans.position, Quaternion.identity).gameObject;
        vfx.transform.SetParent(gameObject.transform);
        fireParticle.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(vfx);
        projectile = Instantiate(prjectilePrefab, fireTrans.position, Quaternion.identity);
        AudioController.Instance.PlayAudio(fireSound);
    }

    protected override void FireChance()
    {
        if (UnityEngine.Random.Range(1, 100) <= chanceToFire)
        {
            StartCoroutine(FireProcess());
        }
    }
}
