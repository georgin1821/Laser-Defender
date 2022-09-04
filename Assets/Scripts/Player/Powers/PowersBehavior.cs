using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersBehavior : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] GameObject gainPowerVFX;
    // [SerializeField] AudioClip gainPowerClip;

    void Update()
    {
        transform.Translate(-Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            SoundEffectController.instance.PowerUpSFX();
            // SoundEffectController.instance.PlayAudioClip(gainPowerClip, 1);
            if(gainPowerVFX != null)
            {
            GameObject vfx = Instantiate(gainPowerVFX, Player.instance.transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f);

            }
        }
    }


}
