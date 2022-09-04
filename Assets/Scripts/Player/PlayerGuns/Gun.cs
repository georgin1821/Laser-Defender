using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun instance;

    [SerializeField] public Transform firePoint;
    [SerializeField] Projectile projectile;
    [SerializeField] float msBetweenShots;
    [SerializeField] float projectileVelocity;

    private float nextShotTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void Shoot(int upgrades)
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            firePoint.position = new Vector3(firePoint.position.x, firePoint.position.y, 0);
            int projectiles = projectile.projectilesCount;

            if (projectiles == 1)

            {
                InstatiateDoublePRojectiles(upgrades);
            }
            else
            {
                SpawnProjectileWithAngle(projectiles, 0);

            }
            //else
            //{
            //    float angle = (fireArcAngle / 2);

            //    for (int i = 0; i < projectiles; i++)
            //    {
            //        SpawnProjectileWithAngle(projectiles, angle);
            //        angle -= fireArcAngle / (projectiles - 1);
            //    }
            //}
            if (!Player.instance.GetIsAlwaysShooting())
            {
                SoundEffectController.instance.PlayerShootClip();
            }

        }
    }

    private Projectile SpawnProjectileWithAngle(int projectiles, float angle)
    {
        Projectile newProjectile = Instantiate(projectile, firePoint.position, Quaternion.Euler(0, 0, angle)) as Projectile;
        return newProjectile;
    }

    private void InstatiateDoublePRojectiles(int upgrades)
    {
        if (upgrades == 1)
        {
            Instantiate(projectile, firePoint.position, Quaternion.identity);

        }
        if (upgrades == 2)
        {
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);

        }
        if (upgrades == 3)
        {
            Instantiate(projectile, firePoint.position, Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
        }
        if (upgrades == 4)
        {
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.6f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.6f, 0, 0), Quaternion.identity);
        }
        if (upgrades == 5)
        {
            Instantiate(projectile, firePoint.position, Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.6f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.6f, 0, 0), Quaternion.identity);

        }
        if (upgrades == 6)
        {
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.6f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.6f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.8f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.8f, 0, 0), Quaternion.identity);

        }

    }

    public float GetProjectileSpeed()
    {
        return projectileVelocity;
    }

}
