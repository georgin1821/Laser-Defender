using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public Transform firePoint;
    [SerializeField] Projectile projectile;
    [SerializeField] float msBetweenShots = 100;
    [SerializeField] float projectileVelocity = 35;

    private int fireArcAngle = 50;
    private float nextShotTime;

    // testing

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            firePoint.position = new Vector3(firePoint.position.x, firePoint.position.y, 0);
            int projectiles = projectile.projectilesCount;

            if (projectiles == 1)

            {
                if (Player.instance.gunIsUpgraded)
                {
                    InstatiateDoublePRojectiles();
                }
                else
                {
                    SpawnProjectileWithAngle(projectiles, 0);

                }
            }
            else
            {
                float angle = (fireArcAngle / 2);

                for (int i = 0; i < projectiles; i++)
                {
                    SpawnProjectileWithAngle(projectiles, angle);
                    angle -= fireArcAngle / (projectiles - 1);
                }
            }
            SoundEffectController.instance.PlayerShootClip();

        }
    }

    private Projectile SpawnProjectileWithAngle(int projectiles, float angle)
    {
        Projectile newProjectile = Instantiate(projectile, firePoint.position, Quaternion.Euler(0, 0, angle)) as Projectile;
        newProjectile.SetSpeed(projectileVelocity);
        return newProjectile;
    }

    private void InstatiateDoublePRojectiles()
    {
        Projectile newProjectile1 = Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity) as Projectile;
        newProjectile1.SetSpeed(projectileVelocity);
        Projectile newProjectile2 = Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity) as Projectile;
        newProjectile2.SetSpeed(projectileVelocity);

    }

}
