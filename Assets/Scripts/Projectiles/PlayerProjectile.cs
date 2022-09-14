using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    public float Speed => Gun.instance.projectileVelocity;
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * Speed, Space.World);
    }


}
