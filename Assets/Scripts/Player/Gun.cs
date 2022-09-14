using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun instance;

    [SerializeField] public Transform firePoint;
    [SerializeField] PlayerProjectile projectile;
    [SerializeField] float msBetweenShots;
    [SerializeField] bool isAlwaysShooting;
    public int Upgrade { get; set; }
    public float projectileVelocity;

    private float nextShotTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (GamePlayController.instance.state != GameState.PLAY)
        {
            return;
        }

        if (isAlwaysShooting)
        {
            Shooting();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Shooting();
            }
        }
    }

    private void Shooting()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            firePoint.position = new Vector3(firePoint.position.x, firePoint.position.y, 0);

            {
                InstatiateProjectiles(Upgrade);
            }
        }
    }
    private PlayerProjectile SpawnProjectileWithAngle(int projectiles, float angle)
    {
        PlayerProjectile newProjectile = Instantiate(projectile, firePoint.position, Quaternion.Euler(0, 0, angle)) as PlayerProjectile;
        return newProjectile;
    }
    private void InstatiateProjectiles(int upgrades)
    {
        if (upgrades == 1)
        {
            Instantiate(projectile, firePoint.position, Quaternion.identity);
            return;
        }
        if (upgrades == 2)
        {
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            return;

        }
        if (upgrades == 3)
        {
            Instantiate(projectile, firePoint.position, Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            return;

        }
        if (upgrades == 4)
        {
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            return;

        }
        if (upgrades == 5)
        {
            Instantiate(projectile, firePoint.position, Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            return;

        }
        if (upgrades == 6)
        {
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.5f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.5f, 0, 0), Quaternion.identity);
            return;

        }
        if (upgrades == 7)
        {

            Instantiate(projectile, firePoint.position, Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.1f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.3f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.1f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.3f, 0, 0), Quaternion.identity);
            return;

        }
        if (upgrades == 8)
        {
            Instantiate(projectile, firePoint.position - new Vector3(.1f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.3f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.1f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.3f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);
            return;

        }
        if (upgrades == 9)
        {
            Instantiate(projectile, firePoint.position, Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.1f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.3f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(.4f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.1f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.2f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.3f, 0, 0), Quaternion.identity);
            Instantiate(projectile, firePoint.position - new Vector3(-.4f, 0, 0), Quaternion.identity);

        }

    }

}
