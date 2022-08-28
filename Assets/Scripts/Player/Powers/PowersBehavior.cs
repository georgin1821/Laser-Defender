using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersBehavior : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] GameObject gainPowerVFX;

    void Update()
    {
        transform.Translate(-Vector3.up * speed * Time.deltaTime);
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Power OnTriggerEnter()");
        Destroy(gameObject);
        SoundEffectController.instance.PowerUpSFX();
        GameObject vfx = Instantiate(gainPowerVFX, Player.instance.transform.position, Quaternion.identity);
        Destroy(vfx, 1.5f);
    }


}
